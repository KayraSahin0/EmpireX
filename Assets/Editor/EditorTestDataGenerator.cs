using UnityEngine;
using UnityEditor;
using EmpireX.Data;

public class EditorTestDataGenerator : EditorWindow
{
    [MenuItem("EmpireX/Test/Generate Configs")]
    public static void GenerateTestConfigs()
    {
        string path = "Assets/Resources/Configs";
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Configs");
        }

        // CompanyType
        CreateOrUpdateConfig<CompanyTypeSO>("TestCompanyType", path, cfg => 
        {
            cfg.Id = "comp_test_1";
            cfg.Name = "Test Tech Company";
            cfg.Category = "Teknoloji";
            cfg.BaseCost = 50000;
            cfg.BaseRevenue = 15000;
            cfg.BaseExpense = 8000;
        });

        // EmployeeType
        CreateOrUpdateConfig<EmployeeTypeSO>("TestEmployeeType", path, cfg =>
        {
            cfg.Id = "emp_test_1";
            cfg.Name = "Yazılım Uzmanı";
            cfg.Skill = 4.5f;
            cfg.Salary = 3000;
        });

        // ExecutiveType
        CreateOrUpdateConfig<ExecutiveTypeSO>("TestExecutiveType", path, cfg =>
        {
            cfg.Id = "exec_test_1";
            cfg.Name = "Genel Müdür";
            cfg.Salary = 10000;
        });

        // City
        CreateOrUpdateConfig<CitySO>("TestCity_Istanbul", path, cfg =>
        {
            cfg.Id = "city_ist";
            cfg.Name = "İstanbul";
            cfg.CountryId = "country_tr";
            cfg.Tax = 0.18f;
        });
        
        CreateOrUpdateConfig<CitySO>("TestCity_Ankara", path, cfg =>
        {
            cfg.Id = "city_ank";
            cfg.Name = "Ankara";
            cfg.CountryId = "country_tr";
            cfg.Tax = 0.15f;
        });

        // Country
        CreateOrUpdateConfig<CountrySO>("TestCountry_Turkey", path, cfg =>
        {
            cfg.Id = "country_tr";
            cfg.Name = "Türkiye";
            cfg.Tax = 0.20f;
            // Logo için test resmi atanamaz (manuel atanmalı)
        });

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Test Config dosyaları Assets/Resources/Configs klasörüne başarıyla oluşturuldu/güncellendi!");
    }

    private static void CreateOrUpdateConfig<T>(string fileName, string path, System.Action<T> onSetup) where T : ScriptableObject
    {
        string fullPath = $"{path}/{fileName}.asset";
        T asset = AssetDatabase.LoadAssetAtPath<T>(fullPath);
        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<T>();
            onSetup?.Invoke(asset);
            AssetDatabase.CreateAsset(asset, fullPath);
        }
        else
        {
            onSetup?.Invoke(asset);
            EditorUtility.SetDirty(asset);
        }
    }
}
