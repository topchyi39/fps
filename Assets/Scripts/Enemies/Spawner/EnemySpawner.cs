using System;
using System.Collections;
using System.Collections.Generic;
using Enemies.Bosses;
using UI;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

namespace Enemies.Spawner
{
    public class Wave
    {
        public bool WaveIsEnd => enemies.Count == 0;
        private List<Enemy> enemies = new ();

        public void AddEnemy(Enemy enemy)
        {
            enemies.Add(enemy);
            enemy.OnDie += () => enemies.Remove(enemy);
        }

        public void KillAllEnemy()
        {
            var aliveEnemies = new Enemy[enemies.Count];
            enemies.CopyTo(aliveEnemies);
            enemies.Clear();

            foreach (var aliveEnemy in aliveEnemies)
            {
                aliveEnemy.Die();
            }
        }
        
    }

    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float radius = 30;

        private Enemy.Factory<RedEnemy> _redFactory;
        private Enemy.Factory<BossEnemy> _bossFactory;

        private EnemiesUI _ui;
        private EnemySpawnerData _data;
        private Wave _currentWave;
        private float _delay;
        private float _time;
        private int _waveCountWithMinDelay = 1;

        [Inject]
        private void Construct(Enemy.Factory<RedEnemy> redFactory, Enemy.Factory<BossEnemy> bossFactory, EnemiesUI ui, EnemySpawnerData data)
        {
            _redFactory = redFactory;
            _bossFactory = bossFactory;
            _ui = ui;
            _data = data;
            _delay = _data.DelayBetweenWaves;
        }

        private void Start()
        {
            StartCoroutine(Waves());
        }

        private void Update()
        {
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            _time += Time.deltaTime;
            _ui.TimerForNextWave.UpdateText(_delay - _time);
        }

        private IEnumerator Waves()
        {
            _currentWave = new Wave();
            while (this)
            {
                _time = 0f;
                
                if (_delay <= _data.MinDelayBetweenWaves)
                    _waveCountWithMinDelay++;
                
                SpawnWave();
                yield return new WaitForSeconds(_delay);
                _delay = Mathf.Clamp(_delay - 0.2f, _data.MinDelayBetweenWaves, _data.DelayBetweenWaves);
            }
        }

        private void SpawnWave()
        {
            for (var j = 0; j < _waveCountWithMinDelay; j++)
            {
                for (var i = 0; i < _data.BossesInWave; i++)
                {
                    var enemy = _bossFactory.Create();
                    enemy.transform.SetParent(transform);
                    RandomNavmeshLocation(enemy.GetComponent<NavMeshAgent>());
                    _currentWave.AddEnemy(enemy);
                }

                for (var i = 0; i < _data.EnemiesInWave; i++)
                {
                    var enemy = _redFactory.Create();
                    enemy.transform.SetParent(transform);
                    RandomNavmeshLocation(enemy.GetComponent<NavMeshAgent>());
                    _currentWave.AddEnemy(enemy);
                }
            }
        }

        public void KillAllEnemies()
        {
            _currentWave.KillAllEnemy();
        }

        private void RandomNavmeshLocation(NavMeshAgent navmeshAgent) 
        {
            navmeshAgent.enabled = false;
            
            var randomDirection = Random.insideUnitSphere * radius;
            var finalPosition = Vector3.zero;
            
            if (NavMesh.SamplePosition(randomDirection, out var hit, radius, navmeshAgent.areaMask)) 
                finalPosition = hit.position;            
            
            navmeshAgent.transform.position = finalPosition + Vector3.up * navmeshAgent.height /2;
            
            navmeshAgent.enabled = true;
        }
    }
}