using UnityEngine;
using UnityEngine.UI;

namespace MyRPGGame.Enemies
{
    public class PopupText : MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField] private Animator animator;
        public void ShowText(string textToShow, Color color, int fontSize)
        {
            text.color = color;
            text.fontSize = fontSize;
            text.text = textToShow;
            Destroy(transform.parent.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
}
