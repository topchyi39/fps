using System;
using UnityEngine;

namespace Enemies.Spawner
{

    [CreateAssetMenu(fileName = "EnemySpawnerData", menuName = "Enemies/EnemySpawnerData", order = 0)]
    public class EnemySpawnerData : ScriptableObject
    {
        [SerializeField] private int enemiesInWave;
        [SerializeField] private int bossesInWave;
        [SerializeField] private int delayBetweenWaves;
        [SerializeField] private int minDelayBetweenWaves = 2;
        

        public int EnemiesInWave => enemiesInWave;
        public int BossesInWave => bossesInWave;
        public int DelayBetweenWaves => delayBetweenWaves;
        public int MinDelayBetweenWaves => minDelayBetweenWaves;
    }
}