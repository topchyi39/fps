using UI.UICounter;
using UnityEngine;

namespace UI
{
    public class EnemiesUI : MonoBehaviour
    {
        [SerializeField] private Counter timerForNextWave;

        public Counter TimerForNextWave => timerForNextWave;
    }
}