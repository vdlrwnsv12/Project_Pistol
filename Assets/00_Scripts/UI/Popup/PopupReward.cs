using DataDeclaration;
using UnityEngine;
using UnityEngine.UI;

public class PopupReward : PopupUI
{
    private ItemSO[] itemRewards;
    [SerializeField] private RewardCard[] rewardCards;

    [SerializeField] private Button cancelBuyBtn;

    protected override void Awake()
    {
        base.Awake();
        itemRewards = new ItemSO[rewardCards.Length];

        cancelBuyBtn.onClick.AddListener(OnClickCancelBuyButton);
    }

    private void OnEnable()
    {
        // Popup 활성화 할때 마다 보상 아이템 세팅하기
        InitReward();
    }

    private void OnClickCancelBuyButton()
    {
        CloseUI();
    }

    /// <summary>
    /// 보상 아이템 세팅하기
    /// </summary>
    private void InitReward()
    {
        //TODO: 아이템 아이콘 이미지 넣기 추가
        itemRewards = StageManager.Instance.RewardSystem.GetRandomItemReward();
        for (var i = 0; i < rewardCards.Length; i++)
        {
            //rewardCards[i].rewardImage.sprite = itemRewards[i].Icon;
            rewardCards[i].rewardName.text = itemRewards[i].Name;
            rewardCards[i].timeCost.text = $"-{itemRewards[i].cost:N2}s";
            rewardCards[i].rewardButton.onClick.RemoveAllListeners();
            var item = itemRewards[i];

            //TODO: 아이템 선택 시 실행할 로직 넣기
            // if (itemRewards[i].ApplicationTarget == (int)ItemApplyType.Player)
            // {
            //     rewardCards[i].rewardButton.onClick.AddListener(() => StageManager.Instance.Player.StatHandler.IncreaseStat(item.RCL, item.HDL, item.STP, item.SPD));
            //     rewardCards[i].rewardButton.onClick.AddListener(() =>
            //         ((HUDUI)UIManager.Instance.CurMainUI).UpdateStatValue(StageManager.Instance.Player.StatHandler.Stat));
            // }
            // else
            // {
            //     rewardCards[i].rewardButton.onClick.AddListener(() => StageManager.Player.Equipment.WeaponStatHandler);
            // }
            rewardCards[i].rewardButton.onClick.AddListener(CloseUI);
        }
    }

    protected override void CloseUI()
    {
        base.CloseUI();
        if (StageManager.Instance.Player.Controller != null && StageManager.Instance.Player.Controller.isActiveAndEnabled)
        {
            UIManager.ToggleMouseCursor(false);
            StageManager.Instance.PauseGame(false);
        }
    }
}