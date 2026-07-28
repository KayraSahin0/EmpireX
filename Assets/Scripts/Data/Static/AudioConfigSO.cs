using UnityEngine;

namespace EmpireX.Audio
{
    [CreateAssetMenu(fileName = "NewAudioConfig", menuName = "EmpireX/Data/AudioConfig")]
    public class AudioConfigSO : ScriptableObject
    {
        [Header("Müzikler")]
        public AudioClip MainMenuMusic;
        public AudioClip GameMusic;

        [Header("Arayüz Sesleri")]
        public AudioClip ButtonClick;
        public AudioClip SuccessAction;
        public AudioClip ErrorAction;

        [Header("Ekonomi Sesleri")]
        public AudioClip MoneyGain;
        public AudioClip MoneySpend;

        [Header("Oyun İçi Olaylar")]
        public AudioClip AchievementUnlocked;
        public AudioClip BadEventTriggered;
        public AudioClip GoodEventTriggered;
    }
}
