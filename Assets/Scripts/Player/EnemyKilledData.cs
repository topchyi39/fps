using System;
using Enemies;
using Player.Characteristics;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class EnemyKilledData
    {
        [Serializable]
        private class CharacteristicAndValuePair
        {
            [SerializeField] private CharacteristicType characteristic;
            [SerializeField] private int value;

            public void ApplyValues(CharacteristicData characteristics)
            {
                characteristics.AddValue(characteristic, value);
            }
            
        }
        
        [SerializeField] private CharacteristicAndValuePair[] bossKill;
        [SerializeField] private CharacteristicAndValuePair[] redKill;
        [SerializeField] private CharacteristicAndValuePair[] reboundKill;
        

        public void OnEnemyKilled(bool withRebound, EnemyType enemyType, CharacteristicData characteristics)
        {

            CharacteristicAndValuePair[] values;
                
            if (withRebound)
            {
                values = reboundKill;
            }
            else
            {
                values = enemyType switch
                {
                    EnemyType.Boss => bossKill,
                    EnemyType.Red => reboundKill,
                };
            }
            
            ApplyValues(values, characteristics);
        }

        private void ApplyValues(CharacteristicAndValuePair[] values, CharacteristicData characteristics)
        {
            if (values == null) return;
            
            foreach (var pair in values)
            {
                pair.ApplyValues(characteristics);
            }
        }
    }
}