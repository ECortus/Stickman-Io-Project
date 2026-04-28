using UnityEngine;
using GameDevUtils.Runtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;

namespace StickmanIo.Runtime.Player.Spawner
{
    public class PlaceDotsOnNavMesh : MonoBehaviour
    {
        [SerializeField] Transform dotsParent;

        [ContextMenu("Update dots positions...")]
        void PlaceDotsOnNavMesh_Function()
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
    }
}