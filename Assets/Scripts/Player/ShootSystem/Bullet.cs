using System.Collections;
using Enemies;
using ObjectPoolSystem;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Player.ShootSystem
{
    public class Bullet : PoolObject
    {
        [SerializeField] private LayerMask enemyMask;
        [SerializeField] private AudioSource audioSource;
        
        private Player _player;
        private Collider _collider;
        private Transform _transform;
        private Enemy _lastEnemy;
        private BulletData _data;
        private Vector3 _direction;
        private bool _rebound;
        private int _damage;
        private ReboundChanceCalculator _chanceCalculator;
        
        private  UnityAction<bool, EnemyType> _onKill;

        private const float TimeToDie = 20f;

        private void Start()
        {
            _transform = transform;
            _collider = GetComponent<Collider>();
        }

        public override void Enable(Transform parent)
        {
            base.Enable(parent);
            audioSource.Play();
        }

        public void SetParams(ReboundChanceCalculator calculator, Vector3 direction, int damage, BulletData data, UnityAction<bool, EnemyType> onKill = null)
        {
            _chanceCalculator = calculator;
            _rebound = false;
            _data = data;
            _direction = direction;
            transform.forward = _direction;
            _damage = damage;
            _onKill = onKill;
        }
        
        private void OnTriggerEnter(Collider other)
        { 
            if (!other.TryGetComponent<Enemy>(out var enemy))
            {
                return;
            }

            _collider.enabled = false;
            _lastEnemy = enemy;
            _lastEnemy.OnDie += OnKill;
            _lastEnemy.Hit(_damage);
        }
        
        private bool TryRebound()
        {
            _rebound = Random.Range(0f, 100f) < _chanceCalculator.ReboundChance;

            return _rebound && TryReboundToNearestEnemyInRange(_data.ReboundRange);
        }

        private bool TryReboundToNearestEnemyInRange(float range)
        {
            var result = new Collider[2];
            var size = Physics.OverlapSphereNonAlloc(_transform.position, range, result, enemyMask);

            if (result.Length <= 0) return false;

            Enemy enemy = null;
            
            foreach (var collider in result)
            {
                if (!collider) continue;
                if (collider.TryGetComponent<Enemy>(out var tempEnemy))
                {
                    if (tempEnemy != _lastEnemy)
                    {
                        enemy = tempEnemy;
                        break;
                    }
                }
            }

            
            if (!enemy) return false;
            _collider.enabled = true;
            _direction = (enemy.transform.position - _transform.position).normalized;
            _transform.forward = _direction;
            
            return true;

        }
        
        private void OnKill()
        {
            _onKill?.Invoke(_rebound, _lastEnemy.EnemyType);
            
            if (!TryRebound())
            {
                Disable();
            }
        }
        
        private void Update()
        {
            Move();
            DisableIfOutOfRange();
        }

        private void DisableIfOutOfRange()
        {
            var position = transform.transform.position;
            position.y = 0;
            
            if (Vector3.Distance(Vector3.zero, position) >= 100f)
                Disable();
        }

        private void Move()
        {
            if (!enabled) return;
            
            if (_direction != Vector3.zero)
                _transform.position = transform.position + _transform.forward * (_data.BulletSpeed * Time.deltaTime);
        }
    }
}