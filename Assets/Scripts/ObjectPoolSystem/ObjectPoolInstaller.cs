using UnityEngine;
using Zenject;

namespace ObjectPoolSystem
{
    public class ObjectPoolInstaller : MonoInstaller
    {
        [SerializeField] private ObjectPool objectPool;

        public override void InstallBindings()
        {
            Container.Bind<IObjectPool<PoolObject>>().FromInstance(objectPool).AsSingle();
        }
    }
}