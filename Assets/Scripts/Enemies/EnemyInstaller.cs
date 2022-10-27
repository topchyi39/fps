using Enemies;
using Enemies.Bosses;
using Enemies.Spawner;
using UnityEngine;
using Zenject;

namespace MonoInstallers
{
    public class EnemyInstaller : MonoInstaller
    {
        [SerializeField] private EnemySpawner spawner;
        
        [SerializeField] private GameObject redEnemyPrefab;
        [SerializeField] private GameObject bossEnemyPrefab;

        public override void InstallBindings()
        {
            Container.Bind<EnemySpawner>().FromInstance(spawner).AsSingle();
            Container.BindFactory<RedEnemy, Enemy.Factory<RedEnemy>>().FromComponentInNewPrefab(redEnemyPrefab);
            Container.BindFactory<BossEnemy, Enemy.Factory<BossEnemy>>().FromComponentInNewPrefab(bossEnemyPrefab);
        }
    }
}