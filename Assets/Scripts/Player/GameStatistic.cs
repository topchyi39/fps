

namespace Player
{
    public interface IGameStatistic
    {
        int EnemyKilled { get; }
    }
    
    public class GameStatistic : IGameStatistic
    {
        private int _enemyKilled;

        public int EnemyKilled => _enemyKilled;

        public GameStatistic()
        {
            _enemyKilled = 0;
        }

        public void OnEnemyKilled()
        {
            _enemyKilled++;
        }
    }
}