using Game;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.UICounter
{
    public class ScoreDisplay : MonoBehaviour
    {
	    [SerializeField] private TMP_Text scoreText;

	    private Player.Player _player;
	    private GameState _gameState;
	    
	    [Inject]
        private void Construct(GameState gameState, Player.Player player)
        {
	        _gameState = gameState;
	        _player = player;
	        
	        _gameState.OnGameOver += RefreshScore;
        }

        private void RefreshScore()
        {
	        scoreText.text = _player.GameStatistic.EnemyKilled.ToString();
        }
    }
}