using UnityEngine;

public class AchievementSystemLauncher : SingletonBehaviour<AchievementSystemLauncher>
{
    [SerializeField] private AchievementUIPrefabSet prefabSet;

    private AchievementMainUI mainUIInstance;

    private static Transform GetOrCreatePopupCanvas()
    {
        var existing = GameObject.Find("PopupCanvas");
        if (existing != null) return existing.transform;

        var prefab = Instance.prefabSet.popupCanvasPrefab;
        if (prefab == null)
        {
            Debug.LogError("PopupCanvas 프리팹이 지정되지 않았습니다.");
            return null;
        }

        var canvas = Instantiate(prefab);
        canvas.name = "PopupCanvas";
        return canvas.transform;
    }

    private static Transform GetOrCreateMainCanvas()
    {
        var existing = GameObject.Find("MainCanvas");
        if (existing != null) return existing.transform;

        var prefab = Instance.prefabSet.mainCanvasPrefab;
        if (prefab == null)
        {
            Debug.LogError("MainCanvas 프리팹이 지정되지 않았습니다.");
            return null;
        }

        var canvas = Instantiate(prefab);
        canvas.name = "MainCanvas";
        return canvas.transform;
    }

    public static void ShowAchievementUI()
    {
        var canvas = GetOrCreateMainCanvas();
        if (canvas == null) return;

        if (Instance.mainUIInstance == null)
        {
            Instance.mainUIInstance = Instantiate(Instance.prefabSet.achievementMainUIPrefab, canvas);
        }

        Instance.mainUIInstance.ToggleListPanel();
    }

    public static void ShowPopup(AchievementSO data)
    {
        if (Instance.prefabSet == null)
        {
            Debug.LogError("[Launcher] prefabSet이 연결되지 않았습니다.");
            return;
        }

        var canvas = GetOrCreatePopupCanvas();
        if (canvas == null) return;

        var popup = Instantiate(Instance.prefabSet.popupPrefab, canvas);
        popup.ShowPopup(data);
    }


    public void Initialize(AchievementUIPrefabSet so)
    {
        prefabSet = so;
        Debug.Log("[Launcher] PrefabSet 초기화 완료");
    }

    public class AchievementUIButton : MonoBehaviour
    {
        public void OnClickShowAchievement()
        {
            AchievementSystemLauncher.ShowAchievementUI();
        }
    }

}
