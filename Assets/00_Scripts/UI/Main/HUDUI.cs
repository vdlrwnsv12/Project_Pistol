using DataDeclaration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDUI : MainUI
{
    #region UI Object

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI remainTimeText;
    [SerializeField] private TextMeshProUGUI curStageText;
    [SerializeField] private Image[] stageIndex;
    [Space] [SerializeField] private Image equipImage;
    [SerializeField] private TextMeshProUGUI ammoText;
    [Space] [SerializeField] private Image spdGauge;
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
        StageManager.Instance.roomCreator.RoomChangedAction += UpdateStageInfo;
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
        scoreText.text = $"<size=36>Score</size>\n<size=50>{StageManager.Instance.GameScore:D6}</size>";
        remainTimeText.text = $"TIME\n{StageManager.Instance.RemainTime:N2}";
        ammoText.text = $"{StageManager.Instance.Player.Weapon.CurAmmo} / {StageManager.Instance.Player.Weapon.MaxAmmo}";
    }

    /// <summary>
    /// Stage 정보 UI 갱신 메서드
    /// </summary>
    private void UpdateStageInfo()
    {
        curStageText.text = $"현재 스테이지\tStage {StageManager.Instance.roomCreator.CurStageIndex}";

        for (var i = 1; i <= stageIndex.Length; i++)
        {
            if (StageManager.Instance.roomCreator.CurRoomIndex == 0)
            {
                stageIndex[i - 1].color = originalColor;
            }
            else
            {
                stageIndex[i - 1].color = currentColor;
                stageIndex[i - 1].color = (i == StageManager.Instance.roomCreator.CurRoomIndex) ? currentColor : originalColor;
            }
        }
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
        Debug.LogWarning($"{weaponImage.name}");
    }
}