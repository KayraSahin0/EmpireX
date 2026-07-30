using System;
using System.Collections.Generic;
using UnityEngine;

namespace EmpireX.Core
{
    /// <summary>
    /// Statik konfigürasyon verilerini yönetir.
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
            // Resources/Configs klasöründeki tüm ScriptableObject konfigürasyonlarını otomatik yükle
            var allConfigs = Resources.LoadAll<ScriptableObject>("Configs");
            foreach (var config in allConfigs)
            {
                _configs[config.GetType()] = config;
            }
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
            
            // Eğer Dictionary içinde yoksa anlık olarak okumayı dene
            var loadedConfig = Resources.Load<T>($"Configs/{typeof(T).Name}");
            if (loadedConfig != null)
            {
                _configs[typeof(T)] = loadedConfig;
                return loadedConfig;
            }
            
            return null;
        }

        public void Dispose()
        {
            _configs.Clear();
        }
    }
}
