using System;
using DataDeclaration;
using UnityEngine;
using UnityEngine.UI;

public class PopupReward : PopupUI
{
    private ItemData[] itemRewards;
    [SerializeField] private RewardCard[] rewardCards;

    [SerializeField] private Button cancelBuyBtn;

    public event Action<ItemData> OnClickItemReward;

    protected override void Awake()
    {
        base.Awake();
        itemRewards = new ItemData[rewardCards.Length];

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
        //TODO: StageManager.Instance.RewardSystem 접근하기
        itemRewards = TestManager.Instance.RewardSystem.GetRandomItemReward();
        for (var i = 0; i < rewardCards.Length; i++)
        {
            rewardCards[i].rewardImage.sprite = itemRewards[i].Icon;
            rewardCards[i].rewardName.text = itemRewards[i].ItemName;
            rewardCards[i].timeCost.text = $"-{itemRewards[i].Cost:N2}s";
            rewardCards[i].rewardButton.onClick.RemoveAllListeners();
            if (itemRewards[i].type == ItemApplyType.Player)
            {
                var playerItem = (PlayerItemData)itemRewards[i];
                rewardCards[i].rewardButton.onClick.AddListener(() => TestManager.Instance.PlayerStatHandler.IncreaseStat(playerItem.Stat));
                rewardCards[i].rewardButton.onClick.AddListener(() =>
                    ((HUDUI)UIManager.Instance.CurMainUI).UpdateStatValue(TestManager.Instance.PlayerStatHandler.Stat));
                rewardCards[i].rewardButton.onClick.AddListener(CloseUI);
            }
        }
    }
}