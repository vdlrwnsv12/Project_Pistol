using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterSO))]
public class TesterWindow : EditorWindow
{
    /// <summary>
    /// 사용법
    /// Tools Player에 
    /// </summary>
    private Player player;
    [MenuItem("Tools/Tester Window")]
    public static void ShowWindow()
    {
        // 윈도우를 띄운다
        GetWindow<TesterWindow>("Tester");

    }
    private void OnGUI()
    {
        GUILayout.Label("=== Character Editor ===", EditorStyles.boldLabel);

        player = (Player)EditorGUILayout.ObjectField("Player", player, typeof(Player), true);
        player.Data = (CharacterSO)EditorGUILayout.ObjectField("Player Data", player.Data, typeof(CharacterSO), true);

        if (player != null && player.Data != null)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("캐릭터 스탯", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();

            player.Data.RCL = EditorGUILayout.Slider("RCL", player.Data.RCL, 0f, 99f);
            player.Data.STP = EditorGUILayout.Slider("STP", player.Data.STP, 0f, 99f);
            player.Data.SPD = EditorGUILayout.Slider("SPD", player.Data.SPD, 0f, 99f);
            player.Data.HDL = EditorGUILayout.Slider("HDL", player.Data.HDL, 0f, 99f);
            player.Data.Cost = EditorGUILayout.IntField("Cost", player.Data.Cost);

            player.adsSpeedMultiplier = EditorGUILayout.Slider("ADS 속도 비율", player.adsSpeedMultiplier, 0f, 1f);
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(player.Data);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Player 넣어주세요", MessageType.Warning);
        }

    }


}
