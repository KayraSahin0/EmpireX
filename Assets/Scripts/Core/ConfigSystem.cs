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
            // Addressables veya Resources üzerinden config yüklemeleri burada yapılacak
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
            return null;
        }

        public void Dispose()
        {
            _configs.Clear();
        }
    }
}
