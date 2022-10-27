using System;
using Game;
using UnityEngine;
using Zenject;

namespace UI.Menu
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private MenuWindow gameWindow;
        [SerializeField] private MenuWindow pauseMenu;
        [SerializeField] private MenuWindow gameOverMenu;

        private MenuWindow _currentMenuWindow;
        private GameState _gameState;
        
        [Inject]
        private void Construct(GameState gameState)
        {
            _gameState = gameState;
            _gameState.OnPause += OnPaused;
            _gameState.OnContinue += OnContinued;
            _gameState.OnGameOver += OnGameOver;
        }

        private void Start()
        {
            OpenWindow(gameWindow);
        }

        private void OnPaused()
        {
            OpenWindow(pauseMenu);
        }

        private void OnContinued()
        {
            OpenWindow(gameWindow);
        }

        private void OnGameOver()
        {
            OpenWindow(gameOverMenu);
        }

        private void OpenWindow(MenuWindow window)
        {
            if(_currentMenuWindow) 
                _currentMenuWindow.Hide();

            _currentMenuWindow = window;
            
            if(_currentMenuWindow)
                _currentMenuWindow.Show();
        }
    }
}