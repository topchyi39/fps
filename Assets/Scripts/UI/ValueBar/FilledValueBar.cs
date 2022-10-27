using MPUIKIT;
using UnityEngine;

namespace UI.ValueBar
{
    public class FilledValueBar : MonoBehaviour
    {
        [SerializeField] private MPImage filledImage;
        [SerializeField] private Gradient gradient;
        
        private int _maxValue;
        
        public void Initialize(int maxValue, int currentValue)
        {
            _maxValue = maxValue;
            UpdateValue(currentValue);
        }
        
        public void UpdateValue(int value)
        {
            filledImage.fillAmount = (float) value / _maxValue;
            
            // var gradienteff = filledImage.GradientEffect;
            // gradienteff.Gradient = gradient;
            // filledImage.GradientEffect = gradienteff;
        }
    }
}