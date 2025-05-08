using UnityEngine;
using UnityEditor;
[System.Serializable]
public class JsonToSO : MonoBehaviour
{
    [MenuItem("Tools/JsonToSO/CreateCharacterSO")]
    static void CharacterDataInit()
    {
        ScriptableObjectCreator.CreateScriptableObjectAssetsFromJson<CharacterData>("Character.json", typeof(CharacterSO));
    }
    [MenuItem("Tools/JsonToSO/CreateWeaponSO")]
    static void WeaponDataInit()
    {
        ScriptableObjectCreator.CreateScriptableObjectAssetsFromJson<WeaponData>("Weapon.json", typeof(WeaponSO));
    }
    [MenuItem("Tools/JsonToSO/CreateItemSO")]
    static void ItemDataInit()
    {
        ScriptableObjectCreator.CreateScriptableObjectAssetsFromJson<ItemData>("Item.json", typeof(ItemSO));
    }
    [MenuItem("Tools/JsonToSO/CreateTargetSO")]
    static void TargetDataInit()
    {
        ScriptableObjectCreator.CreateScriptableObjectAssetsFromJson<TargetData>("Target.json", typeof(TargetSO));
    }
    [MenuItem("Tools/JsonToSO/CreateRoomSO")]
    static void RoomDataInit()
    {
        ScriptableObjectCreator.CreateScriptableObjectAssetsFromJson<RoomData>("Room.json", typeof(RoomSO));
    }
}
