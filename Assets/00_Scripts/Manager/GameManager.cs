public sealed class GameManager : SingletonBehaviour<GameManager>
{
    public CharacterSO selectedCharacter;
    public WeaponSO selectedWeapon;

    public static void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}