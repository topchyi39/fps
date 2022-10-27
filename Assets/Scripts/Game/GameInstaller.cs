using UnityEngine;
using Zenject;

namespace Game
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameState gameState;

        public override void InstallBindings()
        {
            Container.Bind<GameState>().FromInstance(gameState).AsSingle();
        }
    }
}