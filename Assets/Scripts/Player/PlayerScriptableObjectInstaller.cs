using Player;
using UnityEngine;
using Zenject;

namespace MonoInstallers
{
    [CreateAssetMenu (fileName = "PlayerScriptableObjectInstaller", menuName = "Installers/PlayerScriptableObjectInstaller")]
    public class PlayerScriptableObjectInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private ShootData data;

        public override void InstallBindings()
        {
            Container.Bind<ShootData>().FromInstance(data);
        }
    }
}