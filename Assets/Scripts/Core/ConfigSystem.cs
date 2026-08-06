using System;
using System.Collections.Generic;
using UnityEngine;

namespace EmpireX.Core
{
    /// <summary>
    /// Statik konfigürasyon verilerini yönetir. (Genellikle Global/Singleton Configler için kullanılır)
    /// </summary>
    public class ConfigSystem
    {
        private readonly Dictionary<Type, ScriptableObject> _configs;

        public ConfigSystem()
        {
            _configs = new Dictionary<Type, ScriptableObject>();
        }

        public void Initialize()
        {
            // Global configlerin önceden yüklenmesi istenirse buraya eklenebilir.
            // Data listeleri (CitySO vb.) zaten ihtiyaç duyuldukça Resources.LoadAll("City") ile UI taraflarından yükleniyor.
        }

        public void RegisterConfig<T>(T config) where T : ScriptableObject
        {
            _configs[typeof(T)] = config;
        }

        public T GetConfig<T>() where T : ScriptableObject
        {
            if (_configs.TryGetValue(typeof(T), out var config))
            {
                return config as T;
            }
            
            string typeName = typeof(T).Name;

            // 1. YÖNTEM: DATA Tipi (EmpireX/Data/...) 
            // Sonunda SO olanlar (Örn: CountrySO) kendi klasöründen (Resources/Country) yüklenir
            if (typeName.EndsWith("SO"))
            {
                string folderName = typeName.Replace("SO", "");
                var loadedConfigs = Resources.LoadAll<T>(folderName);
                if (loadedConfigs != null && loadedConfigs.Length > 0)
                {
                    _configs[typeof(T)] = loadedConfigs[0];
                    return loadedConfigs[0] as T;
                }
            }
            // 2. YÖNTEM: CONFIG Tipi (EmpireX/Config/...)
            // Direkt Assets/Resources klasöründen aranır. Dosya ismi Sınıf İsmi (örn: EconomyConfig.asset) veya Özellik ismi (örn: Economy.asset) olmalıdır.
            else
            {
                // Önce tam sınıf ismiyle ara
                var loadedConfig = Resources.Load<T>(typeName);
                
                // Bulunamazsa "Config" kelimesi olmadan ara (Örn: Economy)
                if (loadedConfig == null)
                {
                    loadedConfig = Resources.Load<T>(typeName.Replace("Config", ""));
                }

                if (loadedConfig != null)
                {
                    _configs[typeof(T)] = loadedConfig;
                    return loadedConfig;
                }
            }
            
            return null;
        }

        public void Dispose()
        {
            _configs.Clear();
        }
    }
}
