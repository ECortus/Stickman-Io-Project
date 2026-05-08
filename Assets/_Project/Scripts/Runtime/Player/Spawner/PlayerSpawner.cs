using UnityEngine;
using GameDevUtils.Runtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;
using PurrNet;
using PurrNet.Logging;
using PurrNet.Modules;
using System;
using StickmanIo.Runtime.LevelDesign;

namespace StickmanIo.Runtime.Player
{
    public class PlayerSpawner : PurrMonoBehaviour
    {
        [Serializable]
        class SpawnedInstance
        {
            public PlayerID ID;
            public NetworkIdentity Instance;
        }

        [SerializeField, HideInInspector] private NetworkIdentity playerPrefab;
        [SerializeField] private GameObject _playerPrefab;
        [Tooltip("Even if rules are to not despawn on disconnect, this will ignore that and always spawn a player.")]
        [SerializeField] private bool _ignoreNetworkRules;
        [SerializeField] private Transform dotsParent;
        [SerializeField] private Transform playersParent;

        [Space(10)]
        [SerializeField] List<SpawnedInstance> spawnedPlayers = new List<SpawnedInstance>();

        List<Transform> spawnPoints = new List<Transform>();
        private int _currentSpawnPoint;

        private IProvideSpawnPoints _spawnPointProvider;

        private void Awake()
        {
            CleanupSpawnPoints();
            CreateDotsList();
        }

        void Start()
        {
            SpawnPlayers();
        }

        private void CleanupSpawnPoints()
        {
            bool hadNullEntry = false;
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                if (!spawnPoints[i])
                {
                    hadNullEntry = true;
                    spawnPoints.RemoveAt(i);
                    i--;
                }
            }

            if (hadNullEntry)
                PurrLogger.LogWarning($"Some spawn points were invalid and have been cleaned up.", this);
        }

        void CreateDotsList()
        {
            PlaceDotsOnNavMesh();

            spawnPoints = dotsParent.GetComponentsInChildren<Transform>().ToList();
            spawnPoints.RemoveAt(0);
        }

        [ContextMenu("Place Dots On NavMesh")]
        void PlaceDotsOnNavMesh()
        {
            var array = dotsParent.GetComponentsInChildren<Transform>().ToList();
            array.RemoveAt(0);

            foreach (var dot in array)
            {
                var position = dot.position;
                if (NavMesh.SamplePosition(position, out NavMeshHit hit, 25f, NavMesh.AllAreas))
                {
                    position = hit.position;
                }
                dot.position = position;
            }
        }

        private void OnValidate()
        {
            if (playerPrefab)
            {
                _playerPrefab = playerPrefab.gameObject;
                playerPrefab = null;
            }
        }

        public override void Subscribe(NetworkManager manager, bool asServer)
        {
            if (asServer && manager.TryGetModule(out ScenePlayersModule scenePlayersModule, true))
            {
                scenePlayersModule.onPlayerLoadedScene += OnPlayerLoadedScene;
                SpawnPlayers();
            }
        }

        void SpawnPlayers()
        {
            NetworkManager manager = NetworkManager.main; 
            bool asServer = true;

            if (asServer && manager && manager.TryGetModule(out ScenePlayersModule scenePlayersModule, true))
            {
                if (!manager.TryGetModule(out ScenesModule scenes, true))
                    return;

                if (!scenes.TryGetSceneID(gameObject.scene, out var sceneID))
                    return;

                if (scenePlayersModule.TryGetPlayersInScene(sceneID, out var players))
                {
                    foreach (var player in players)
                        OnPlayerLoadedScene(player, sceneID, true);
                }
            }
        }

        public override void Unsubscribe(NetworkManager manager, bool asServer)
        {
            if (asServer && manager.TryGetModule(out ScenePlayersModule scenePlayersModule, true))
            {
                scenePlayersModule.onPlayerLoadedScene -= OnPlayerLoadedScene;
            }
        }

        private void OnDestroy()
        {
            if (NetworkManager.main &&
                NetworkManager.main.TryGetModule(out ScenePlayersModule scenePlayersModule, true))
            {
                scenePlayersModule.onPlayerLoadedScene -= OnPlayerLoadedScene;
            }
        }

        private void OnPlayerLoadedScene(PlayerID player, SceneID scene, bool asServer)
        {
            var main = NetworkManager.main;
            if (!main)
            {
                return;
            }
            
            var sceneID = GetCurrentSceneID();
            if (sceneID != scene)
            {
                return;
            }

            if (!asServer)
            {
                return;
            }

            bool isDestroyOnDisconnectEnabled = main.networkRules.ShouldDespawnOnOwnerDisconnect();
            if (!_ignoreNetworkRules && !isDestroyOnDisconnectEnabled && main.TryGetModule(out GlobalOwnershipModule ownership, true) &&
                ownership.PlayerOwnsSomething(player))
            {
                return;
            }

            CleanupSpawnPoints();

            PlayersLogger.LogAdded($"Player ID-{player}");
            SpawnPlayer(player, sceneID);
        }

        SceneID? GetCurrentSceneID()
        {
            var main = NetworkManager.main;

            if (!main || !main.TryGetModule(out ScenesModule scenes, true))
            {
                return null;
            }

            var unityScene = gameObject.scene;

            if (!scenes.TryGetSceneID(unityScene, out var sceneID))
            {
                return null;
            }

            return sceneID;
        }

        public void RespawnPlayer(PlayerID player, SceneID? scene = null)
        {
            SpawnPlayer(player, scene);
        }

        void SpawnPlayer(PlayerID player, SceneID? scene = null)
        {
            var spawned = spawnedPlayers.Find(c => c.ID == player);
            if (spawned != null)
            {
                var instance = spawned.Instance;
                if (instance)
                {
                    DebugHelper.LogWarning($"Player {player} is already spawned. Decline to spawn.");
                    return;

                    /* instance.Despawn(); */
                }

                spawnedPlayers.Remove(spawned);
            }

            GameObject newPlayer;

            if (scene == null)
            {
                scene = GetCurrentSceneID();
            }

            Vector3 position;
            Quaternion rotation;

            if (_spawnPointProvider != null)
            {
                var point = _spawnPointProvider.NextSpawnPoint(player, scene.Value);

                position = point.position;
                rotation = point.rotation;
            }
            else if (spawnPoints.Count > 0)
            {
                var spawnPoint = spawnPoints[_currentSpawnPoint];
                _currentSpawnPoint = (_currentSpawnPoint + 1) % spawnPoints.Count;

                position = spawnPoint.position;
                rotation = spawnPoint.rotation;
            }
            else
            {
                _playerPrefab.transform.GetPositionAndRotation(out position, out rotation);
            }

            var unityScene = gameObject.scene;

            newPlayer = UnityProxy.Instantiate(_playerPrefab, position, rotation, unityScene);
            newPlayer.transform.SetParent(playersParent);

            PlayersLogger.LogSpawned($"Player ID-{player.id}");

            /* newPlayer = ObjectInstantiator.InstantiatePrefab(_playerPrefab, position, rotation, playersParent); */
            newPlayer.name = newPlayer.name + $"_(id-{player.id})_(spawned-at-{Time.time})";

            if (newPlayer.TryGetComponent(out NetworkIdentity identity))
            {
                identity.GiveOwnership(player);

                var spawnedInstance = new SpawnedInstance()
                {
                    ID = player,
                    Instance = identity
                };

                spawnedPlayers.Add(spawnedInstance);
                if (newPlayer.TryGetComponent(out IHealth health))
                {
                    health.OnDied += () =>
                    {
                        spawnedPlayers.Remove(spawnedInstance);
                    };
                }
            }
        }
    }
}