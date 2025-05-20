using DataDeclaration;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HUDUI : MainUI
{
    #region UI Object

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject scorePrefab;
    [SerializeField] private Transform scorePosition;
    [SerializeField] private TextMeshProUGUI remainTimeText;
    [SerializeField] private TextMeshProUGUI curStageText;
    [SerializeField]
    private TextMeshProUGUI comboText
    ;
    [SerializeField] private Image[] stageIndex;
    [Space][SerializeField] private Image equipImage;
    [SerializeField] private TextMeshProUGUI ammoText;
    [Space][SerializeField] private Image spdGauge;
    [SerializeField] private TextMeshProUGUI spdText;
    [SerializeField] private Image hdlGauge;
    [SerializeField] private TextMeshProUGUI hdlText;
    [SerializeField] private Image rclGauge;
    [SerializeField] private TextMeshProUGUI rclText;
    [SerializeField] private Image stpGauge;
    [SerializeField] private TextMeshProUGUI stpText;

    #endregion

    private Color32 originalColor;
    private Color32 currentColor;

    public override MainUIType UIType { get; protected set; }

    private void Awake()
    {
        UIType = MainUIType.HUD;

        originalColor = new Color32(221, 234, 249, 255);
        currentColor = new Color32(252, 192, 1, 255);
    }

    private void Start()
    {
        RoomManager.Instance.RoomChangedAction += UpdateStageInfo;
        UpdateStageInfo();
        UpdateStatValue();

        var weapon = GameManager.Instance.selectedWeapon;
        var image = ResourceManager.Instance.Load<Sprite>($"Sprites/{weapon.ID}");
        SetEquipImage(image);
    }

    private void Update()
    {
        UpdateRealTimeChanges();
    }

    public override void SetActiveUI(MainUIType activeUIType)
    {
        gameObject.SetActive(UIType == activeUIType);
    }

    /// <summary>
    /// 실시간의로 변경되는 UI 갱신 메서드
    /// </summary>
    private void UpdateRealTimeChanges()
    {
        scoreText.text = $"<size=36>Score</size>\n<size=80>{StageManager.Instance.GameScore:D6}</size>";
        remainTimeText.text = $"<size=120>{StageManager.Instance.RemainTime:N2}</size>";
        ammoText.text = $"{StageManager.Instance.Player.Weapon.CurAmmo} / {StageManager.Instance.Player.Weapon.MaxAmmo}";
        comboText.text = $"<size=36>Combo</size>\n<size=80>{StageManager.Instance.DestroyTargetCombo}</size>";
    }

    /// <summary>
    /// Stage 정보 UI 갱신 메서드
    /// </summary>
    private void UpdateStageInfo()
    {
        curStageText.text = $"Stage {RoomManager.Instance.CurStageIndex}";

        for (var i = 1; i <= stageIndex.Length; i++)
        {
            if (RoomManager.Instance.CurRoomIndex == 0)
            {
                stageIndex[i - 1].color = originalColor;
            }
            else
            {
                stageIndex[i - 1].color = currentColor;
                stageIndex[i - 1].color = (i == RoomManager.Instance.CurRoomIndex) ? currentColor : originalColor;
            }
        }
    }

    public void ShowScoreEffect(bool isHeadShot, int headShotScore, int comboScore, int quickShotScore, int rangeScore)
    {
        if (scorePrefab == null || scorePosition == null) return;

        void CreateScoreText(string label, int score)
        {
            var go = ObjectPoolManager.Instance.GetObject(scorePrefab, Vector3.zero, Quaternion.identity, 2f);
            go.transform.SetParent(scorePosition, true);

            var text = go.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = $"{label} +{score:0000}";
            }
        }

        string hitLabel = isHeadShot ? "Head Shot" : "Body Shot";
        CreateScoreText(hitLabel, headShotScore);

        if (comboScore > 0)
            CreateScoreText("Combo Bonus", comboScore);

        if (quickShotScore > 0)
            CreateScoreText("Quick Shot Bonus", quickShotScore);

        if (rangeScore > 0)
            CreateScoreText("Range Bonus", rangeScore);
    }





    /// <summary>
    /// 캐릭터 스탯 UI 갱신 메서드
    /// </summary>
    public void UpdateStatValue()
    {
        var stat = StageManager.Instance.Player.Stat;

        rclGauge.fillAmount = stat.RCL / Constants.MAX_STAT;
        rclText.text = stat.RCL.ToString("N0");

        hdlGauge.fillAmount = stat.HDL / Constants.MAX_STAT;
        hdlText.text = stat.HDL.ToString("N0");

        stpGauge.fillAmount = stat.STP / Constants.MAX_STAT;
        stpText.text = stat.STP.ToString("N0");

        spdGauge.fillAmount = stat.SPD / Constants.MAX_STAT;
        spdText.text = stat.SPD.ToString("N0");
    }

    /// <summary>
    /// 캐릭터가 장착 중인 무기 이미지 UI 갱신 메서드
    /// </summary>
    public void SetEquipImage(Sprite weaponImage)
    {
        equipImage.sprite = weaponImage;
    }
}