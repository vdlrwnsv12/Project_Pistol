#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class ScriptableObjectCreator
{
    public static void CreateScriptableObjectAssetsFromJson<T>(string jsonFileName, Type type) where T : new()
    {
        var path = $"Assets/Resources/Data/JSON/{jsonFileName}";
        var json = File.ReadAllText(path);
        var dataArray = new List<T>(FromJson<T>(json));

        for (var i = 0; i < dataArray.Count; i++)
        {
            CreateScriptableObject(dataArray[i], type, i);
        }
    }
    
    private static T[] FromJson<T>(string json)
    {
        var wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Data;
    }

    private static void CreateScriptableObject<T>(T data, Type type, int index) where T : new()
    {
        var asset = ScriptableObject.CreateInstance(type);
        JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(data), asset);

        var directoryPath = $"Assets/Resources/Data/SO/{type.Name}";

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        AssetDatabase.CreateAsset(asset, $"{directoryPath}/{GetFirstVariableValue(data)}.asset");
        AssetDatabase.SaveAssets();
    }
    
    private static string GetFirstVariableValue<T>(T data)
    {
        var firstField = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance)[0];
        var value = firstField.GetValue(data);
        return value.ToString();
    }
}

[Serializable]
public class Wrapper<T>
{
    public T[] Data;
}
#endif