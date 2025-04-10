using DataDeclaration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDUI : MainUI
{
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

    private Color32 originalColor;
    private Color32 currentColor;

    private StageData curStageInfo;
    private PlayerEquipment curEquip;
    private PlayerStatHandler curStat;

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

    public void UpdateRealTimeChanges(int score, float remainTime, int curAmmo, int remainAmmo)
    {
        scoreText.text = $"<size=36>Score</size>\n<size=50>{score:D6}</size>";
        remainTimeText.text = $"TIME\n{remainTime:N2}";
        ammoText.text = $"{curAmmo} / {remainAmmo}";
    }

    public void UpdateStageInfo(int curStage, int curStageIndex)
    {
        curStageText.text = $"현재 스테이지\tStage {curStage}";

        for (var i = 0; i < stageIndex.Length; i++)
        {
            stageIndex[i].color = currentColor;
            stageIndex[i].color = (i == curStageIndex) ? currentColor : originalColor;
        }
    }

    public void UpdateStatValue(float rclValue, float hdlValue, float stpValue, float spdValue)
    {
        rclGauge.fillAmount = rclValue / 100f;
        rclText.text = rclValue.ToString("N0");

        hdlGauge.fillAmount = hdlValue / 100f;
        hdlText.text = hdlValue.ToString("N0");

        stpGauge.fillAmount = stpValue / 100f;
        stpText.text = stpValue.ToString("N0");

        spdGauge.fillAmount = spdValue / 100f;
        spdText.text = spdValue.ToString("N0");
    }

    private void SetEquipImage(Image weaponImage)
    {
        equipImage = weaponImage;
    }
}