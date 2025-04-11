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

    protected override void Awake()
    {
        base.Awake();
        uiType = MainUIType.HUD;

        originalColor = new Color32(221, 234, 249, 255);
        currentColor = new Color32(252, 192, 1, 255);
    }

    public override void SetActiveUI(MainUIType activeUIType)
    {
        gameObject.SetActive(uiType == activeUIType);
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
        rclGauge.fillAmount = stat.rclValue / 100f;
        rclText.text = stat.rclValue.ToString("N0");

        hdlGauge.fillAmount = stat.hdlValue / 100f;
        hdlText.text = stat.hdlValue.ToString("N0");

        stpGauge.fillAmount = stat.stpValue / 100f;
        stpText.text = stat.stpValue.ToString("N0");

        spdGauge.fillAmount = stat.spdValue / 100f;
        spdText.text = stat.spdValue.ToString("N0");
    }

    /// <summary>
    /// 캐릭터가 장착 중인 무기 이미지 UI 갱신 메서드
    /// </summary>
    public void SetEquipImage(Image weaponImage)
    {
        equipImage = weaponImage;
    }
}