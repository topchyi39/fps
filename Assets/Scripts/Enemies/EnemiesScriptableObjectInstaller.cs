using Enemies.Spawner;
using Player;
using UnityEngine;
using Zenject;

namespace MonoInstallers
{
    [CreateAssetMenu (fileName = "EnemiesScriptableObjectInstaller", menuName = "Installers/EnemiesScriptableObjectInstaller")]
    public class EnemiesScriptableObjectInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private EnemySpawnerData data;

        public override void InstallBindings()
        {
            Container.Bind<EnemySpawnerData>().FromInstance(data);
        }
    }
}