using UnityEngine;
using UnityEditor;
using System.IO;
using System.Globalization;

public class DataImporter : EditorWindow
{
    [MenuItem("Tool/Import Data from CSV")]
    public static void ImportData()
    {
        // CSV 경로 설정
        string targetCsvPath = Application.dataPath + "/01_Resources/Resources/Data/CSV/Target.csv";
        string itemCsvPath = Application.dataPath + "/01_Resources/Resources/Data/CSV/Item.csv";

        string targetSavePath = "Assets/01_Resources/Resources/Data/SO/Target/";
        string itemSavePath = "Assets/01_Resources/Resources/Data/SO/Item/";

        // 폴더 없으면 생성
        if (!Directory.Exists(targetSavePath))
            Directory.CreateDirectory(targetSavePath);
        if (!Directory.Exists(itemSavePath))
            Directory.CreateDirectory(itemSavePath);

        // 타겟 CSV 읽기
        ReadTargetData(targetCsvPath, targetSavePath);
        // 아이템 CSV 읽기
        ReadItemData(itemCsvPath, itemSavePath);
    }

    private static void ReadTargetData(string csvPath, string savePath)
    {
        string[] lines = File.ReadAllLines(csvPath);

        if (lines.Length <= 2)
        {
            Debug.LogError("CSV에 데이터가 없습니다.");
            return;
        }

        // 데이터 읽기
        for (int i = 2; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            if (values.Length < 8)
            {
                Debug.LogWarning($"줄 {i + 1} 데이터가 부족함: {lines[i]}");
                continue;
            }

            // TargetDatas SO 생성
            TargetDatas asset = ScriptableObject.CreateInstance<TargetDatas>();
            asset.ID = values[0];
            asset.Name = values[1];
            asset.Description = values[2];
            asset.Type = int.Parse(values[3]);
            asset.Hp = float.Parse(values[4], CultureInfo.InvariantCulture);
            asset.Level = float.Parse(values[5], CultureInfo.InvariantCulture);
            asset.Speed = float.Parse(values[6], CultureInfo.InvariantCulture);
            asset.DamageRate = float.Parse(values[7], CultureInfo.InvariantCulture);

            // SO 저장
            string assetPath = savePath + asset.ID + ".asset";
            AssetDatabase.CreateAsset(asset, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("🎉 TargetData import 완료!");
    }

    private static void ReadItemData(string csvPath, string savePath)
    {
        string[] lines = File.ReadAllLines(csvPath);

        if (lines.Length <= 2)
        {
            Debug.LogError("아이템 CSV에 데이터가 없습니다.");
            return;
        }

        // 데이터 읽기
        for (int i = 2; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            if (values.Length < 15)
            {
                Debug.LogWarning($"줄 {i + 1} 아이템 데이터가 부족함: {lines[i]}");
                continue;
            }

            // ItemDatas SO 생성
            ItemDatas asset = ScriptableObject.CreateInstance<ItemDatas>();
            asset.ID = values[0];
            asset.Name = values[1];
            asset.Description = values[2];
            asset.ApplicationTarget = int.Parse(values[3]);
            asset.cost = float.Parse(values[4], CultureInfo.InvariantCulture);
            asset.RCL = float.Parse(values[5], CultureInfo.InvariantCulture);
            asset.HDL = float.Parse(values[6], CultureInfo.InvariantCulture);
            asset.STP = float.Parse(values[7], CultureInfo.InvariantCulture);
            asset.SPD = float.Parse(values[8], CultureInfo.InvariantCulture);
            asset.DMG = float.Parse(values[9], CultureInfo.InvariantCulture);
            asset.ShootRecoil = float.Parse(values[10], CultureInfo.InvariantCulture);
            asset.MaxAmmo = int.Parse(values[11]);
            asset.WeaponParts = int.Parse(values[12]);
            asset.AppearanceRate = float.Parse(values[13], CultureInfo.InvariantCulture);

            // SO 저장
            string assetPath = savePath + asset.ID + ".asset";
            AssetDatabase.CreateAsset(asset, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("🎉 ItemData import 완료!");
    }
}
