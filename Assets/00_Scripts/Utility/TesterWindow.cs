// #if UNITY_EDITOR
// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
//
// public class TesterWindow : EditorWindow
// {
//     /// <summary>
//     /// 사용법:
//     /// Tools > Tester Window 선택 후 Player와 WeaponHandler를 드래그해서 삽입
//     /// </summary>
//
//     private Player player;
//     private WeaponStatHandler weaponHandler;
//     private WeaponSO weaponData;
//
//     [MenuItem("Tools/Tester Window")]
//     public static void ShowWindow()
//     {
//         GetWindow<TesterWindow>("Tester");
//     }
//
//     private void OnGUI()
//     {
//         GUILayout.Label("=== Character Editor ===", EditorStyles.boldLabel);
//
//         // 플레이어 객체와 데이터 연결
//         player = (Player)EditorGUILayout.ObjectField("Player", player, typeof(Player), true);
//         player.Data = (CharacterSO)EditorGUILayout.ObjectField("Player Data", player.Data, typeof(CharacterSO), true);
//
//         if (player != null && player.Data != null)
//         {
//             EditorGUILayout.Space();
//             EditorGUILayout.LabelField("캐릭터 스탯", EditorStyles.boldLabel);
//             EditorGUI.BeginChangeCheck();
//
//             player.Data.RCL = EditorGUILayout.Slider("RCL", player.Data.RCL, 0f, 99f);
//             player.Data.STP = EditorGUILayout.Slider("STP", player.Data.STP, 0f, 99f);
//             player.Data.SPD = EditorGUILayout.Slider("SPD", player.Data.SPD, 0f, 99f);
//             player.Data.HDL = EditorGUILayout.Slider("HDL", player.Data.HDL, 0f, 99f);
//             player.Data.Cost = EditorGUILayout.IntField("Cost", player.Data.Cost);
//
//             player.adsSpeedMultiplier = EditorGUILayout.Slider("ADS 속도 비율", player.adsSpeedMultiplier, 0f, 1f);
//
//             if (EditorGUI.EndChangeCheck())
//             {
//                 EditorUtility.SetDirty(player.Data);
//             }
//         }
//         else
//         {
//             EditorGUILayout.HelpBox("Player 넣어주세요", MessageType.Warning);
//         }
//
//         EditorGUILayout.Space();
//         GUILayout.Label("=== Weapon Editor ===", EditorStyles.boldLabel);
//
//         // 무기 핸들러 연결
//         //weaponHandler = (WeaponStatHandler)EditorGUILayout.ObjectField("Weapon Handler", weaponHandler, typeof(WeaponStatHandler), true);
//         AutoAssignWeaponDataFromHandler(); // 🔹 자동 무기 데이터 연결
//
//         if (weaponData != null)
//         {
//             EditorGUI.BeginChangeCheck();
//
//             EditorGUILayout.LabelField("무기 이름: " + weaponData.Name);
//             weaponData.DMG = EditorGUILayout.FloatField("데미지", weaponData.DMG);
//             weaponData.ShootRecoil = EditorGUILayout.FloatField("반동", weaponData.ShootRecoil);
//             weaponData.ReloadTime = EditorGUILayout.FloatField("재장전 시간", weaponData.ReloadTime);
//             weaponData.MaxAmmo = EditorGUILayout.IntField("최대 탄창", weaponData.MaxAmmo);
//             weaponData.Cost = EditorGUILayout.IntField("무기 비용", weaponData.Cost);
//
//             if (EditorGUI.EndChangeCheck())
//             {
//                 EditorUtility.SetDirty(weaponData);
//             }
//         }
//         else
//         {
//             EditorGUILayout.HelpBox("WeaponStatHandler 넣어주세요 (또는 무기 데이터 없음)", MessageType.Warning);
//         }
//     }
//
//     private void AutoAssignWeaponDataFromHandler()
//     {
//         if (weaponHandler != null && weaponHandler.weaponData != null)
//         {
//             weaponData = weaponHandler.weaponData;
//         }
//     }
// }
// #endif
