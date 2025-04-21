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
    public override bool IsDestroy { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        UIType = MainUIType.HUD;
        IsDestroy = false;

        originalColor = new Color32(221, 234, 249, 255);
        currentColor = new Color32(252, 192, 1, 255);
    }

    public override void SetActiveUI(MainUIType activeUIType)
    {
        gameObject.SetActive(UIType == activeUIType);
    }

    /// <summary>
    /// 실시간의로 변경되는 UI 갱신 메서드
    /// </summary>
    /// <param name="score">현재 점수</param>
    /// <param name="remainTime">남은 시간</param>
    /// <param name="curAmmo">현재 탄</param>
    /// <param name="remainAmmo">남은 탄</param>
    public void UpdateRealTimeChanges(int score, float remainTime, int curAmmo, int remainAmmo)
    {
        scoreText.text = $"<size=36>Score</size>\n<size=50>{score:D6}</size>";
        remainTimeText.text = $"TIME\n{remainTime:N2}";
        ammoText.text = $"{curAmmo} / {remainAmmo}";
    }

    /// <summary>
    /// Stage 정보 UI 갱신 메서드
    /// </summary>
    /// <param name="curStage">현재 스테이지</param>
    /// <param name="curStageIndex">현재 스테이지 페이즈</param>
    public void UpdateStageInfo(int curStage, int curStageIndex)
    {
        curStageText.text = $"현재 스테이지\tStage {curStage}";

        for (var i = 0; i < stageIndex.Length; i++)
        {
            stageIndex[i].color = currentColor;
            stageIndex[i].color = (i == curStageIndex) ? currentColor : originalColor;
        }
    }

    /// <summary>
    /// 캐릭터 스탯 UI 갱신 메서드
    /// </summary>
    public void UpdateStatValue(CharacterStat stat)
    {
        rclGauge.fillAmount = stat.RCL / Constant.MAX_STAT;
        rclText.text = stat.RCL.ToString("N0");

        hdlGauge.fillAmount = stat.HDL / Constant.MAX_STAT;
        hdlText.text = stat.HDL.ToString("N0");

        stpGauge.fillAmount = stat.STP / Constant.MAX_STAT;
        stpText.text = stat.STP.ToString("N0");

        spdGauge.fillAmount = stat.SPD / Constant.MAX_STAT;
        spdText.text = stat.SPD.ToString("N0");
    }
    
    /// <summary>
    /// 캐릭터 스탯 UI 갱신 메서드
    /// </summary>
    public void UpdateStatValue(float rcl, float hdl, float stp, float spd)
    {
        rclGauge.fillAmount = rcl / Constant.MAX_STAT;
        rclText.text = rcl.ToString("N0");

        hdlGauge.fillAmount = hdl / Constant.MAX_STAT;
        hdlText.text = hdl.ToString("N0");

        stpGauge.fillAmount = stp / Constant.MAX_STAT;
        stpText.text = stp.ToString("N0");

        spdGauge.fillAmount = spd / Constant.MAX_STAT;
        spdText.text = spd.ToString("N0");
    }

    /// <summary>
    /// 캐릭터가 장착 중인 무기 이미지 UI 갱신 메서드
    /// </summary>
    public void SetEquipImage(Image weaponImage)
    {
        equipImage = weaponImage;
    }
}