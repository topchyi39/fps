using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

namespace Player
{
    public class TeleportingFromEdge : MonoBehaviour
    {
        [SerializeField] private float range;

        private Player _player;
        
        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }
        
        private void Update()
        {
            if (!CheckForInRange())
                _player.TeleportToPosition(GetRandomPosition());
        }

        private bool CheckForInRange()
        {
            var position = _player.transform.position;
            position.y = 0;
            return Vector3.Distance(Vector3.zero, position) <= range;
        }

        private Vector3 GetRandomPosition()
        {
            var randomDirection = Random.insideUnitSphere * range / 2;
            var finalPosition = Vector3.zero;
            
            if (NavMesh.SamplePosition(randomDirection, out var hit, range / 2, 1)) 
                finalPosition = hit.position;

            return finalPosition;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(Vector3.zero, range);
        }
    }
}