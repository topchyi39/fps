using Cinemachine;
using Input;
using UnityEngine;
using Zenject;

namespace Utilities
{
    public class PlayerFovChanger : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        
        [SerializeField] private float defaultValue;
        [SerializeField] private float sprintValue;
        [SerializeField] private float speedChange;

        private StarterAssetsInputs _input;

        [Inject]
        private void Construct(StarterAssetsInputs input)
        {
            _input = input;
        }

        private void Update()
        {
            var value = _input.sprint ? sprintValue : defaultValue;

            var oldValue = virtualCamera.m_Lens.FieldOfView;
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(oldValue, value, Time.deltaTime * speedChange);
        }
    }
}