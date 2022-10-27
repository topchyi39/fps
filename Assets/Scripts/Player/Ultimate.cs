using System;
using Enemies.Spawner;
using Input;
using UnityEngine;
using Zenject;

namespace Player
{
    public class Ultimate : MonoBehaviour
    {
        private Player _player;
        private StarterAssetsInputs _input;
        private EnemySpawner _enemySpawner;
        
        [Inject]
        private void Construct(StarterAssetsInputs input, Player player, EnemySpawner enemySpawner)
        {
            _input = input;
            _player = player;
            _enemySpawner = enemySpawner;
        }
        
        private void Update()
        {
            if(_input.ultimate)
                TryUltimate();
        }

        private void TryUltimate()
        {
            if (_player.CanUltimate)
            {
                _player.StealStrength(100);
                _enemySpawner.KillAllEnemies();
            }
        }
    }
}