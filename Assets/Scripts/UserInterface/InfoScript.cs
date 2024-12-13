using UnityEngine;

namespace UserInterface
{
    public class InfoScript : MonoBehaviour
    {
        public object InfoObject { get; }

        public void Show(object info)
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}