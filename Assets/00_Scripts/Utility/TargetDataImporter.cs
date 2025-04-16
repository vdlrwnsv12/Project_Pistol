using UnityEngine;
using UnityEditor;
using System.IO;
using System.Globalization;

public class TargetDataImporter : EditorWindow
{
    [MenuItem("Tool/Import Target Data from CSV")]
    public static void ImportTargetData()
    {
        // 경로 설정
        string csvPath = Application.dataPath + "/01_Resources/Resources/Data/CSV/Target.csv";
        string soSavePath = "Assets/01_Resources/Resources/Data/SO/Target/";

        // 저장 폴더 없으면 생성
        if (!Directory.Exists(soSavePath))
            Directory.CreateDirectory(soSavePath);

        // CSV 파일 읽기
        string[] lines = File.ReadAllLines(csvPath);

        if (lines.Length <= 2)
        {
            Debug.LogError("CSV에 데이터가 없습니다.");
            return;
        }

        // 헤더 + 타입 줄 스킵하고 데이터 시작
        for (int i = 2; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            if (values.Length < 8)
            {
                Debug.LogWarning($"줄 {i + 1} 데이터가 부족함: {lines[i]}");
                continue;
            }

            // ScriptableObject 생성
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
            string assetPath = soSavePath + asset.ID + ".asset";
            AssetDatabase.CreateAsset(asset, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("🎉 TargetData import 완료!");
    }
}
