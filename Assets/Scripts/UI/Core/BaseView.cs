using UnityEngine;
using DG.Tweening;

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
            DOTween.Kill(transform);
            transform.localScale = Vector3.one * 0.9f;
            transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetUpdate(true);
        }

        public virtual void Hide()
        {
            DOTween.Kill(transform);
            transform.DOScale(0.9f, 0.2f).SetEase(Ease.InBack).SetUpdate(true).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }

        public virtual void ShowImmediate()
        {
            DOTween.Kill(transform);
            transform.localScale = Vector3.one;
            gameObject.SetActive(true);
        }

        public virtual void HideImmediate()
        {
            DOTween.Kill(transform);
            gameObject.SetActive(false);
        }
    }
}
