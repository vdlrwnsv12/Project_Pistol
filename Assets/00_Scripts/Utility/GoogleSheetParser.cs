#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataDeclaration;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Unity.EditorCoroutines.Editor;
using Newtonsoft.Json.Linq;

public class GoogleSheetParser : EditorWindow
{
    

    private List<SheetInfo> sheetInfoList = new();

    private bool isFetching = false;
    private bool isFetchComplete = false;
    private int selectedSheetIndex = 0;

    [MenuItem("Tools/Google Sheet Parsing Tool")]
    private static void OpenWindow()
    {
        var window = GetWindow(typeof(GoogleSheetParser));
        // 윈도우 타이틀 이름 지정
        window.titleContent = new GUIContent("Google Sheet Parsing Tool");

        // 윈도우 최소/최대 사이즈 지정 (가로, 세로)
        window.minSize = new Vector2(300, 300);
        window.maxSize = new Vector2(300, 300);
        window.maximized = false;
    }

    #region OnGUI

    private void OnGUI()
    {
        GUILayout.Space(20);

        if (isFetching)
        {
            EditorGUILayout.LabelField("구글 시트 데이터 불러오는 중...");
        }
        else
        {
            if (sheetInfoList.Count > 0)
            {
                // 불러온 시트 데이터에서 시트 이름을 값으로 갖는 배열 생성
                var sheetNames = sheetInfoList.Select(o => o.sheetName).ToArray();
                selectedSheetIndex = EditorGUILayout.Popup("Select Sheet", selectedSheetIndex, sheetNames);
                isFetchComplete = true;
            }
            else
            {
                isFetchComplete = false;
                EditorGUILayout.LabelField("시트를 찾을 수 없음");
            }
        }

        GUILayout.Space(20);

        if (GUILayout.Button("구글 시트 데이터 불러오기", GUILayout.Height(40)) && !isFetching)
        {
            EditorCoroutineUtility.StartCoroutine(FetchSheetList(), this);
        }

        if (isFetchComplete)
        {
            GUILayout.Space(40);

            if (GUILayout.Button("선택한 시트 Json으로 변환", GUILayout.Height(40)))
            {
                ParseSheetToJson();
            }

            GUILayout.Space(40);

            if (GUILayout.Button("선택한 시트의 데이터를 SO로 생성", GUILayout.Height(40)))
            {
                EditorCoroutineUtility.StartCoroutine(MakeSOAssetsFromJson(), this);
            }
        }
    }

    #endregion

    #region FetchSheetInfo

    /// <summary>
    /// 시트 이름과 ID를 배열로 저장
    /// </summary>
    private IEnumerator FetchSheetList()
    {
        isFetching = true;

        // API URL로 요청
        var request = UnityWebRequest.Get(Constants.API_URL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("불러오기 성공");

            // 모든 시트 데이터 불러오기 진행
            sheetInfoList.Clear();
            var sheetsJsonData = JsonUtility.FromJson<SheetInfoArray>(request.downloadHandler.text);
            sheetInfoList.AddRange(sheetsJsonData.sheetData);
        }
        else
        {
            Debug.LogError("불러오기 실패" + request.error);
        }

        isFetching = false;

        // 에디터 창 갱신
        Repaint();
    }

    #endregion

    #region ParseSheetToJson

    private void ParseSheetToJson()
    {
        var selectedSheet = sheetInfoList[selectedSheetIndex];
        var sheetName = selectedSheet.sheetName;
        Debug.Log($"Selected Sheet: {selectedSheet.sheetName}, Sheet ID: {selectedSheet.sheetId}");

        EditorCoroutineUtility.StartCoroutine(ParseGoogleSheet(sheetName, selectedSheet.sheetId.ToString()), this);
    }

    /// <summary>
    /// 선택한 구글 시트의 데이터를 Json 파일과 SO 스크립트로 생성
    /// </summary>
    /// <param name="selectedSheetName">선택한 시트 이름</param>
    /// <param name="selectedSheetGID">선택한 시트 ID</param>
    private IEnumerator ParseGoogleSheet(string selectedSheetName, string selectedSheetGID, bool notice = true)
    {
        var sheetURL = $"{Constants.Google_Sheet_URL}/export?format=tsv&gid={selectedSheetGID}";

        var request = UnityWebRequest.Get(sheetURL);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            EditorUtility.DisplayDialog("변환 실패!", "구글 연결 실패!", "확인");
            yield break;
        }

        var data = request.downloadHandler.text;
        var rows = ParseTSVData(data);

        if (rows == null || rows.Count < 3)
        {
            Debug.LogError("Not enough data rows to parse.");
            yield break;
        }

        JArray jArray = new();

        var keys = rows[0].Split('\t').ToList();
        var types = rows[1].Split('\t').ToList();

        for (var i = 2; i < rows.Count; i++)
        {
            var rowData = rows[i].Split('\t').ToList();

            var rowObject = ParseDataToJObject(keys, types, rowData);

            if (rowObject != null)
            {
                jArray.Add(rowObject);
            }
        }
        
        SaveJsonToFile(selectedSheetName, jArray);
        CreateSOClass(selectedSheetName, keys, types);
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 선택한 구글 시트의 데이터를 Json 파일로 생성
    /// </summary>
    /// <param name="jsonFileName">생성될 Json 파일 이름</param>
    /// <param name="jArray">Json 데이터를 저장할 매개변수</param>
    private void SaveJsonToFile(string jsonFileName, JArray jArray)
    {
        var directoryPath = Path.Combine(Application.dataPath, "Resources", "Data", "JSON");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var jsonFilePath = Path.Combine(directoryPath, $"{jsonFileName}.json");
        var jsonData = "{\"Data\":" + jArray.ToString() + "}";

        File.WriteAllText(jsonFilePath, jsonData);
        Debug.Log($"{jsonFilePath} 경로에 JSon 파일 저장");
    }
    
    /// <summary>
    /// 선택한 구글 시트의 데이터를 SO 스크립트로 생성
    /// </summary>
    /// <param name="sheetName">생성될 스크립트 파일 이름</param>
    /// <param name="keys">변수 이름 배열</param>
    /// <param name="types">변수 타입 배열</param>
    private void CreateSOClass(string sheetName, List<string> keys, List<string> types)
    {
        var className = $"{sheetName}SO";
        var dataClassName = $"{sheetName}Data";
        var directoryPath = Path.Combine(Application.dataPath, "00_Scripts", "Data", "SO");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        
        var filePath = Path.Combine(directoryPath, $"{className}.cs");

        using (var sw = new StreamWriter(filePath))
        {
            sw.WriteLine("using UnityEngine;");
            sw.WriteLine("using System.Collections.Generic;");
            sw.WriteLine($"[CreateAssetMenu(fileName = \"{className}\", menuName = \"SO/{className}\")]\r");
            sw.WriteLine($"public class {className} : ScriptableObject");
            sw.WriteLine("{");

            // 스크립트 데이터 필드 입력
            for (var i = 0; i < keys.Count; i++)
            {
                var fieldType = ConvertTypeToCSharp(types[i]);
                var fieldName = keys[i];

                if (!string.IsNullOrEmpty(fieldName))
                {
                    sw.WriteLine($"    public {fieldType} {fieldName};");
                }
            }
            sw.WriteLine("}");
            sw.WriteLine();
            
            sw.WriteLine("[System.Serializable]");
            sw.WriteLine($"public class {dataClassName}");
            sw.WriteLine("{");
            for (var i = 0; i < keys.Count; i++)
            {
                var fieldType = ConvertTypeToCSharp(types[i]);
                var fieldName = keys[i];

                if (!string.IsNullOrEmpty(fieldName))
                {
                    sw.WriteLine($"    public {fieldType} {fieldName};");
                }
            }
            sw.WriteLine("}");
        }
    }

    /// <summary>
    /// TSV를 문자열 List로 변환
    /// </summary>
    private List<string> ParseTSVData(string data)
    {
        return data.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    private JObject ParseDataToJObject(List<string> keys, List<string> types, List<string> rowData)
    {
        JObject rowObject = new();

        for (var i = 0; i < keys.Count && i < rowData.Count; i++)
        {
            var key = keys[i];
            var type = types[i];
            var value = rowData[i].Trim();

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                continue;
            }

            rowObject[key] = ConvertValue(value, type);
        }

        return rowObject.HasValues ? rowObject : null;
    }

    private JToken ConvertValue(string value, string type)
    {
        switch (type.Trim())
        {
            case "int": return int.TryParse(value, out int intValue) ? intValue : 0;
            case "long": return long.TryParse(value, out long longValue) ? longValue : 0L;
            case "float": return float.TryParse(value, out float floatValue) ? floatValue : 0.0f;
            case "double": return double.TryParse(value, out double doubleValue) ? doubleValue : 0.0d;
            case "bool": return bool.TryParse(value, out bool boolValue) ? boolValue : false;
            case "byte": return byte.TryParse(value, out byte byteValue) ? byteValue : (byte)0;
            case "int[]":
                return JArray.FromObject(value.Split(',')
                    .Select(v => int.TryParse(v.Trim(), out int tempInt) ? tempInt : 0));
            case "float[]":
                return JArray.FromObject(value.Split(',')
                    .Select(v => float.TryParse(v.Trim(), out float tempFloat) ? tempFloat : 0.0f));
            case "string[]": return JArray.FromObject(value.Split(',').Select(v => v.Trim()));
            case "DateTime":
                return DateTime.TryParse(value, out DateTime dateTimeValue) ? dateTimeValue : DateTime.MinValue;
            case "TimeSpan":
                return TimeSpan.TryParse(value, out TimeSpan timeSpanValue) ? timeSpanValue : TimeSpan.Zero;
            case "Guid": return Guid.TryParse(value, out Guid guidValue) ? guidValue.ToString() : Guid.Empty.ToString();
            default: return value;
        }
    }
    
    private string ConvertTypeToCSharp(string type)
    {
        switch (type.Trim()) 
        {
            case "int": return "int";
            case "long": return "long";
            case "float": return "float";
            case "double": return "double";
            case "bool": return "bool";
            case "byte": return "byte";
            case "int[]": return "int[]";
            case "float[]": return "float[]";
            case "string[]": return "string[]";
            case "DateTime": return "System.DateTime"; 
            case "TimeSpan": return "System.TimeSpan";
            case "Guid": return "System.Guid";
            default: return "string";
        }
    }

    #endregion

    #region MakeSOAssets

    private IEnumerator MakeSOAssetsFromJson()
    {
        CreateSOToolMenu();
        yield return null;
    }

    private void CreateSOToolMenu()
    {
        var directoryPath = Path.Combine(Application.dataPath, "00_Scripts/Utility");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var dataClassPath = Path.Combine(directoryPath, $"JsonToSO.cs");
        
        using (var sw = new StreamWriter(dataClassPath))
        {
            sw.WriteLine("using UnityEngine;");
            sw.WriteLine("using UnityEditor;");
            sw.WriteLine("[System.Serializable]");
            sw.WriteLine("public class JsonToSO : MonoBehaviour");
            sw.WriteLine("{");

            for (var i = 0; i < sheetInfoList.Count; i++)
            {
                sw.WriteLine($"    [MenuItem(\"Tools/JsonToSO/Create{sheetInfoList[i].sheetName}SO\")]");
                sw.WriteLine($"    static void {sheetInfoList[i].sheetName}DataInit()");
                sw.WriteLine("    {");
                sw.WriteLine(
                    $"        ScriptableObjectCreator.CreateScriptableObjectAssetsFromJson<{sheetInfoList[i].sheetName}Data>(\"{sheetInfoList[i].sheetName}.json\", typeof({sheetInfoList[i].sheetName}SO));");
                sw.WriteLine("    }");
            }

            sw.WriteLine("}");
        }

        AssetDatabase.Refresh();
    }

    #endregion
}

[Serializable]
public class SheetInfo
{
    public string sheetName;
    public int sheetId;
}

[Serializable]
public class SheetInfoArray
{
    public SheetInfo[] sheetData;
}

#endif