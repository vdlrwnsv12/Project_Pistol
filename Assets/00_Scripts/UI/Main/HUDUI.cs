using DataDeclaration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDUI : MainUI
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI remainTime;
    [SerializeField] private TextMeshProUGUI curStage;
    [SerializeField] private Image[] stageIndex;

    [SerializeField] private Image equipImage;
    [SerializeField] private TextMeshProUGUI ammoText;
    
    [SerializeField] private TextMeshProUGUI spdText;
    [SerializeField] private TextMeshProUGUI hdlText;
    [SerializeField] private TextMeshProUGUI rclText;
    [SerializeField] private TextMeshProUGUI stpText;

    private StageData curStageInfo;
    private PlayerEquipment curEquip;
    private PlayerStatHandler curStat;
    
    protected override void Awake()
    {
        base.Awake();
        uiType = MainUIType.HUD;
    }

    public override void SetActiveUI(MainUIType activeUIType)
    {
        gameObject.SetActive(uiType == activeUIType);
    }
}
