using UnityEngine;

namespace UI.Menu
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MenuWindow : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;


        public void Show()
        {
            ChangeVisible(true);
        }

        public void Hide()
        {
            ChangeVisible(false);
        }

        private void ChangeVisible(bool visible)
        {
            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}