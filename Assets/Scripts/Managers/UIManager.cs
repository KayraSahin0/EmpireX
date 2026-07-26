using EmpireX.Events;

namespace EmpireX.Core
{
    /// <summary>
    /// UIManager sınıfı. Sistem Eventlerini dinleyerek Navigation veya UI sistemlerine komut iletir.
    /// </summary>
    public class UIManager : BaseManager
    {
        public UIManager(IEventBus eventBus) : base(eventBus) { }

        public override void Initialize()
        {
            _eventBus.Subscribe<GameStarted>(OnGameStarted);
        }
        
        private void OnGameStarted(GameStarted e)
        {
            // Oyun başladığında MainMenu veya GameHUD geçişi tetiklenir
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<GameStarted>(OnGameStarted);
        }
    }
}
