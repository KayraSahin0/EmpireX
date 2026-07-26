namespace EmpireX.UI
{
    /// <summary>
    /// Ekranı kaplayan büyük panellerin temel sınıfı. (Örn: MainMenu, GameHUD)
    /// </summary>
    public abstract class BasePanel : BaseView
    {
        public virtual void Initialize() { }
    }
}
