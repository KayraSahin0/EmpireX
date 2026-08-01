namespace EmpireX.UI
{
    public class PausePanel : BasePanel
    {
        public BasePanel LoadMenuPanel;
        public BasePanel SettingsPanel;

        public override void Show()
        {
            base.Show();
            
            // Oyun zamanını durdur (Event tetikleyerek)
            if (EmpireX.Core.GameManager.Instance != null && EmpireX.Core.GameManager.Instance.EventManager != null)
            {
                EmpireX.Core.GameManager.Instance.EventManager.EventBus.Publish(new EmpireX.Events.GamePaused());
            }
        }

        public void OnContinueClicked()
        {
            if (EmpireX.Core.GameManager.Instance != null && EmpireX.Core.GameManager.Instance.EventManager != null)
            {
                EmpireX.Core.GameManager.Instance.EventManager.EventBus.Publish(new EmpireX.Events.GameResumed());
            }
            
            UINavigation.Instance.GoBack();
        }

        public void OnLoadClicked()
        {
            if (LoadMenuPanel != null)
                UINavigation.Instance.ShowPanel(LoadMenuPanel, keepHistory: true);
        }

        public void OnSettingsClicked()
        {
            if (SettingsPanel != null)
                UINavigation.Instance.ShowPanel(SettingsPanel, keepHistory: true);
        }

        public void OnMainMenuClicked()
        {
            if (EmpireX.Core.GameManager.Instance != null && EmpireX.Core.GameManager.Instance.EventManager != null)
            {
                EmpireX.Core.GameManager.Instance.EventManager.EventBus.Publish(new EmpireX.Events.ShowSystemPopupEvent
                {
                    Title = "Ana Menüye Dön",
                    Message = "Ana menüye dönmek istediğinize emin misiniz? Otomatik kayıt alınacaktır.",
                    Severity = EmpireX.Events.ErrorSeverity.Warning,
                    Button1Text = "Evet",
                    Button1Callback = () => 
                    {
                        if (EmpireX.Core.GameManager.Instance.SaveManager != null)
                        {
                            EmpireX.Core.GameManager.Instance.SaveManager.AutoSave();
                        }
                        
                        // Zamanı tekrar akıt (Sahne değiştiğinde normal hızda başlaması için)
                        EmpireX.Core.GameManager.Instance.EventManager.EventBus.Publish(new EmpireX.Events.GameResumed());
                        
                        // Yükleme ekranıyla Ana Menüye dön
                        EmpireX.Core.GameManager.Instance.SceneManager.LoadSceneAsync("MainMenu");
                    },
                    Button2Text = "Hayır",
                    Button2Callback = () => {}
                });
            }
        }
    }
}
