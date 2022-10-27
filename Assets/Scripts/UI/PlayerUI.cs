using TMPro;
using UI.UICounter;
using UI.ValueBar;
using UnityEngine;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private FilledValueBar healthBar;
        [SerializeField] private FilledValueBar strengthBar;
        [SerializeField] private Counter enemyKilled; // rework
        
        
        public FilledValueBar HealthBar => healthBar;
        public FilledValueBar StrengthBar => strengthBar;

        
        public void UpdateEnemyKilledCounter(int killed)
        {
            enemyKilled.UpdateText(killed);
        }
    }
}