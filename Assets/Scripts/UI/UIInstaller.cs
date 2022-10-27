using Enemies;
using UI.ValueBar;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private PlayerUI playerUI;
        [SerializeField] private EnemiesUI enemiesUI;

        public override void InstallBindings()
        {
            Container.Bind<PlayerUI>().FromInstance(playerUI);
            Container.Bind<EnemiesUI>().FromInstance(enemiesUI);
        }
    }
}