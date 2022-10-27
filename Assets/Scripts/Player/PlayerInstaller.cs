using Input;
using UnityEngine;
using Zenject;

namespace MonoInstallers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private Player.Player player;
        [SerializeField] private StarterAssetsInputs input;
        

        public override void InstallBindings()
        {
            Container.Bind<StarterAssetsInputs>().FromInstance(input);
            Container.Bind<Player.Player>().FromInstance(player).AsSingle();
        }
    }
}