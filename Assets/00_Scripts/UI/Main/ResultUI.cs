using DataDeclaration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MainUI
{
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [Space] [SerializeField] private TextMeshProUGUI remainTimeText;
    [SerializeField] private TextMeshProUGUI accuracyText;
    [SerializeField] private TextMeshProUGUI headAccuracyText;
    [SerializeField] private TextMeshProUGUI maxComboText;
    [Space] [SerializeField] private Button quitBtn;

    protected override void Awake()
    {
        base.Awake();
        uiType = MainUIType.Result;

        quitBtn.onClick.AddListener(OnClickQuitButton);
    }

    public override void SetActiveUI(MainUIType activeUIType)
    {
        gameObject.SetActive(uiType == activeUIType);
    }

    private void OnClickQuitButton()
    {
    }

    /// <summary>
    /// 결과값 갱신
    /// </summary>
    /// <param name="rank">등급</param>
    /// <param name="score">점수</param>
    /// <param name="remainTime">남은 시간</param>
    /// <param name="accuracy">명중률</param>
    /// <param name="headAccuracy">헤드샷 명중률</param>
    /// <param name="maxCombo">최대 콤보</param>
    private void UpdateResultValue(RankType rank, int score, float remainTime, float accuracy, float headAccuracy, int maxCombo)
    {
        rankText.text = rank.ToString();
        scoreText.text = $"<size=30>Score</size>\\n<size=40>{score:D6}</size>";

        remainTimeText.text = $"남은 시간\\n{remainTime:N2}초";
        accuracyText.text = $"명중률\\n{accuracy:N2}%";
        headAccuracyText.text = $"헤드 명중률\\n{headAccuracy:N2}%";
        maxComboText.text = $"최대 콤보\\n{maxCombo:D2} Combo";
    }
}