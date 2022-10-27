using System;
using ObjectPoolSystem;
using UnityEngine;

namespace Enemies.Bosses
{
    public class BossRocket : PoolObject
    {
        [SerializeField] private float speed;

        private Transform _transform;
        private Player.Player _player;
        private int _damage;
        private Vector3 _offset = Vector3.up;

        private bool _playerWasTeleported;
        private Vector3 _teleportedPosition;

        private void Start()
        {
            _transform = transform;
        }

        private void Update()
        {
            MoveToPlayer();
        }

        public override void Disable()
        {
            base.Disable();
            
            if(_player)
                _player.OnTeleported -= OnPlayerTeleported;
            
            _player = null;
            _playerWasTeleported = false;
        }

        public void SetDamage(int damage)
        {
            if(damage > 0)
                _damage = damage;
        }
        
        public void SetTarget(Player.Player player)
        {
            _player = player;
            _player.OnTeleported += OnPlayerTeleported;
        }
        

        private void OnPlayerTeleported()
        {
            _teleportedPosition = _player.transform.position;
            _playerWasTeleported = true;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Player.Player>(out var player)) return;

            _player.StealStrength(_damage);
            Disable();
        }


        private void MoveToPlayer()
        {
            if (!_player) return;
            
            var position = _transform.position;
            var targetPosition = TargetPosition();

            _transform.forward = (targetPosition + _offset - position).normalized;

            position = position + _transform.forward * (speed * Time.deltaTime);
            _transform.position = position;
            
            
            CheckForDestination();
        }

        private Vector3 TargetPosition()
        {
            var targetPosition = _player.transform.position;

            if (_playerWasTeleported)
            {
                targetPosition = _teleportedPosition;
            }

            return targetPosition;
        }

        private void CheckForDestination()
        {
            var position = _transform.position;
            var targetPosition = TargetPosition();
            
            if(Vector3.Distance(position, targetPosition + _offset) < 0.01f)
                Disable();
        }
        
    }
}