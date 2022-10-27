using UnityEngine;

namespace Player
{
    public class ReboundChanceCalculator
    {
        private Player _player;
        private float _defaultChance;

        public float ReboundChance => Calculate(_player.Health, _defaultChance);
        
        public ReboundChanceCalculator(Player player, float defaultChance)
        {
            _player = player;
            _defaultChance = defaultChance;
        }
        
        private float Calculate(int health, float chance)
        {
            Debug.Log(Remap(health, 20f, 100f, 100f, chance));
            return Remap(health, 20f, 100f, 100f, chance);
        }
        
        private float Remap (float value, float from1, float to1, float from2, float to2) 
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
    
}