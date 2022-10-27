using System;
using Input;
using ObjectPoolSystem;
using Player.Characteristics;
using Player.ShootSystem;
using Sounds;
using UnityEngine;
using Zenject;

namespace Player
{
    public class ShootController : MonoBehaviour
    {
        [SerializeField] private Transform camera;
        
        private ReboundChanceCalculator _calculator;
        private StarterAssetsInputs _input;

        private Player _player;
        private ShootData _shootData;
        private IObjectPool<PoolObject> _objectPool;
        private float _time;

        [Inject]
        private void Construct(Player player, StarterAssetsInputs input, IObjectPool<PoolObject> objectPool, ShootData shootData)
        {
            _player = player;
            _input = input;
            _objectPool = objectPool;
            _shootData = shootData;
        }

        private void Start()
        {
            _time = _shootData.FireRate;
            _calculator = new ReboundChanceCalculator(_player, _shootData.BulletData.ChangeToRebound);
        }

        private void Update()
        {
            UpdateFireRate();
            CheckFire();
        }
        
        private void Fire()
        {
            _time = 0f;
            var startPosition = camera.position;
            var direction = camera.forward.normalized;
            var bullet = _objectPool.Instantiate<Bullet>(startPosition, Quaternion.identity, null);
            PlaySound();
            bullet.SetParams(_calculator, direction, _player.Damage, _shootData.BulletData, _player.OnEnemyKill);
        }

        private void PlaySound()
        {
            var soundObject = _objectPool.Instantiate<SoundObject>(transform);
            soundObject.Play(_shootData.Clip);
        }

        public void UpdateFireRate()
        {
            _time += Time.deltaTime;
        }

        private void CheckFire()
        {
            if(_input.fire && _time >= _shootData.FireRate)
                Fire();
        }
    }
}