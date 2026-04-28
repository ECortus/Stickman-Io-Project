using UnityEngine;
using GameDevUtils.Runtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;

namespace StickmanIo.Runtime.Player
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private PlayerHeader playerPrefab;

        [Space(5)]
        [SerializeField] private Transform dotsParent;
        [SerializeField] private Transform ownerParent;
        [SerializeField] private Transform playersParent;

        List<Transform> dots = new List<Transform>();

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            CreateDotsList();

            var randomIndex = Random.Range(0, dots.Count - 1);

            var spawnPosition = dots[randomIndex].position;
            SpawnPlayer(spawnPosition, ownerParent);
        }

        void CreateDotsList()
        {
            PlaceDotsOnNavMesh();

            dots = dotsParent.GetComponentsInChildren<Transform>().ToList();
            dots.RemoveAt(0);
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

        void SpawnPlayer(Vector3 position, Transform parent)
        {
            ObjectInstantiator.InstantiatePrefabForComponent(playerPrefab, position, Quaternion.identity, parent);
        }
    }
}