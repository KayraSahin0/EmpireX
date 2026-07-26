using UnityEngine;

namespace EmpireX.UI
{
    /// <summary>
    /// UI hiyerarşisindeki tüm görsel bileşenlerin temel sınıfı.
    /// </summary>
    public abstract class BaseView : MonoBehaviour
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
