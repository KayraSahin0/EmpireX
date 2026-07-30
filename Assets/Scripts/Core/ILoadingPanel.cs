using System;

namespace EmpireX.Core
{
    public interface ILoadingPanel
    {
        void Show();
        void UpdateProgress(float progress);
        void Hide(Action onHidden = null);
    }
}
