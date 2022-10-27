using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Player.Characteristics
{
    [Serializable]
    public class CharacteristicPoints
    {
        [SerializeField] private int maxPoints;
        [SerializeField] private int current;
        [SerializeField] private bool canModify;


        public int MaxPoints => maxPoints;
        public int Current => current;
        public bool CanModify => canModify;

        public event Action<int> OnModified; 

        public CharacteristicPoints(int maxPoints, int current, bool canModify)
        {
            this.maxPoints = maxPoints;
            this.current = current;
            this.canModify = canModify;
        }
        public CharacteristicPoints(CharacteristicPoints points)
        {
            maxPoints = points.MaxPoints;
            current = points.Current;
            canModify = points.CanModify;
        }

        public void Modify(int value)
        {
            if (!CanModify) return;
            
            current = Mathf.Clamp(current + value, 0, maxPoints);
            OnModified?.Invoke(Current);
        }
    }

    public enum CharacteristicType
    {
        HealthPoint,
        StrengthPoint,
        Damage
    }

    [Serializable]
    public class CharacteristicContainer
    {
        [SerializeField] private CharacteristicType type;
        [SerializeField] private CharacteristicPoints points;

        public CharacteristicType Type => type;
        public CharacteristicPoints Points => points;
    }
    
    [CreateAssetMenu(fileName = "CharacteristicData", menuName = "Entities/Characteristic")]
    public class CharacteristicData : ScriptableObject
    {
        [SerializeField] private CharacteristicContainer[] characteristics;
        
        private Dictionary<CharacteristicType, CharacteristicPoints> _points;

        public void Initialize()
        {
            _points = new Dictionary<CharacteristicType, CharacteristicPoints>();
            
            foreach (var characteristic in characteristics)
            {
                if(_points.ContainsKey(characteristic.Type)) continue;
                
                _points.Add(characteristic.Type, new CharacteristicPoints(characteristic.Points));
            }
        }

        public void SubscribeOnModify(CharacteristicType characteristicType, Action<int> onModified)
        {
            if (!_points.ContainsKey(characteristicType)) return;

            _points[characteristicType].OnModified += onModified;
        }

        public void AddValue(CharacteristicType characteristicType, int value)
        {
            if (!_points.ContainsKey(characteristicType)) return;
            
            var points = _points[characteristicType];
            
            if(!points.CanModify) return;
            
            _points[characteristicType].Modify(value);
        }

        public int GetMaxValue(CharacteristicType characteristicType)
        {
            if (_points.ContainsKey(characteristicType))
                return _points[characteristicType].MaxPoints;
            
            return 0;
        }

        public int GetCurrentValue(CharacteristicType characteristicType)
        {
            if (_points.ContainsKey(characteristicType))
                return _points[characteristicType].Current;
            
            return 0;
        }
    }
}