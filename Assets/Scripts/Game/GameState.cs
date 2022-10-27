using Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game
{
    public interface IGameStateObserver
    {
        
    }
    
    public class GameState : MonoBehaviour
    {
        [SerializeField] private UnityEvent onPause;
        [SerializeField] private UnityEvent onContinue;
        [SerializeField] private UnityEvent onGameOver;

        private StarterAssetsInputs _input;

        public event UnityAction OnPause
        {
            add => onPause.AddListener(value);
            remove => onPause.RemoveListener(value);
        }
        
        public event UnityAction OnContinue
        {
            add => onContinue.AddListener(value);
            remove => onContinue.RemoveListener(value);
        }
        
        public event UnityAction OnGameOver
        {
            add => onGameOver.AddListener(value);
            remove => onGameOver.RemoveListener(value);
        }
        
        [Inject]
        private void Construct(StarterAssetsInputs input)
        {
            _input = input;
        }

        [ContextMenu("Pause")]
        public void Pause(bool callbackEvent = true)
        {
            _input.Disable();
            Time.timeScale = 0;
            if(callbackEvent)
                onPause?.Invoke();
        }

        public void Continue()
        {
            _input.Enable();
            Time.timeScale = 1;
            onContinue?.Invoke();
        }

        public void Restart()
        {
            SceneManager.LoadScene(0);
            Continue();
        }

        public void GameOver()
        {
            Pause(false);
            onGameOver?.Invoke();
        }
    }
}