using UnityEngine;

namespace EmpireX.Audio
{
    [CreateAssetMenu(fileName = "NewAudioConfig", menuName = "EmpireX/Data/AudioConfig", order = 2)]
    public class AudioConfigSO : ScriptableObject
    {
        [Header("MÃ¼zikler")]
        public AudioClip MainMenuMusic;
        public AudioClip GameMusic;

        [Header("ArayÃ¼z Sesleri")]
        public AudioClip ButtonClick;
        public AudioClip SuccessAction;
        public AudioClip ErrorAction;

        [Header("Ekonomi Sesleri")]
        public AudioClip MoneyGain;
        public AudioClip MoneySpend;

        [Header("Oyun Ä°Ã§i Olaylar")]
        public AudioClip AchievementUnlocked;
        public AudioClip BadEventTriggered;
        public AudioClip GoodEventTriggered;
    }
}

