using System.Collections.Generic;
using UnityEngine;

namespace EmpireX.Core
{
    [CreateAssetMenu(fileName = "LoadingConfig", menuName = "EmpireX/Config/LoadingConfig")]
    public class LoadingConfig : ScriptableObject
    {
        [Header("Görseller")]
        [Tooltip("Yükleme ekranında gösterilecek rastgele arkaplanlar")]
        public List<Sprite> Backgrounds;

        [Header("İpuçları")]
        [Tooltip("Yükleme ekranında gösterilecek rastgele oyun içi ipuçları")]
        [TextArea(2, 5)]
        public List<string> Hints;
    }
}
