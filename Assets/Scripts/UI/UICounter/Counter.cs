using TMPro;
using UnityEngine;

namespace UI.UICounter
{
    public class Counter : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private string template;
        
        public void UpdateText(int count)
        {
            text.text = string.Format(template, count.ToString());
        }
        
        public void UpdateText(float value)
        {
            text.text = string.Format(template, value.ToString("F1"));

        }
    }
}