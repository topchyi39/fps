using ObjectPoolSystem;
using Player.Characteristics;
using Sounds;
using UnityEngine;
using Zenject;

namespace Enemies.Bosses
{
    public class BossEnemy : Enemy
    {
        [SerializeField] private float fireRate;
        [SerializeField] private AudioClip rocketLaunchClip;
        
        private IObjectPool<PoolObject> _objectPool;
        private float _time = 0;
        
        public override EnemyType EnemyType => EnemyType.Boss;

        [Inject]
        private void Construct(IObjectPool<PoolObject> objectPool)
        {
            _objectPool = objectPool;
        }

        protected override void Start()
        {
            RandomNavmeshPath();
        }

        protected override bool TickAction()
        {
            _time += Time.deltaTime;
            TryToSetNewRandomPath();
            if(_time >= fireRate)
                LaunchRocket();

            return true;
        }

        private void LaunchRocket()
        {
            _time = 0f;
            var rocket = _objectPool.Instantiate<BossRocket>(transform.position, Quaternion.identity, null);
            rocket.SetDamage(characteristicData.GetCurrentValue(CharacteristicType.Damage));
            PlaySound();
            rocket.SetTarget(_player);
        }
        
        private void PlaySound()
        {
            var soundObject = _objectPool.Instantiate<SoundObject>(transform);
            soundObject.Play(rocketLaunchClip);
        }
    }
}