using UnityEngine;
using UnityEngine.UI;

namespace MyRPGGame.Enemies
{
    public class PopupText : MonoBehaviour
    {
        public Text text;
        public Animator animator;
        public void ShowText(string textToShow)
        {
            if(text&&animator)
            {
                text.text = textToShow;
                Destroy(transform.parent.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
            }
            else
            {
                Debug.LogError(GetType() + " couldn't find one of its required components");
            }
        }
    }
}
