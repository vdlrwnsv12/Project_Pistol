using System;
using DataDeclaration;
using UnityEngine;
using UnityEngine.UI;

public class PopupReward : PopupUI
{
    private ItemData[] itemRewards;
    [SerializeField] private RewardCard[] rewardCards;
    
    [SerializeField] private Button cancelBuyBtn;

    private Action buttonClickAction;

    protected override void Awake()
    {
        base.Awake();
        itemRewards = new ItemData[rewardCards.Length];

        for (var i = 0; i < itemRewards.Length; i++)
        {
            var i1 = i;
            rewardCards[i].rewardButton.onClick.AddListener(() => RewardSelectAction(itemRewards[i1]));
            rewardCards[i].rewardButton.onClick.AddListener(CloseUI);
        }
        
        cancelBuyBtn.onClick.AddListener(OnClickCancelBuyButton);
    }

    private void OnEnable()
    {
        // Popup 활성화 할때 마다 보상 세팅하기
        InitReward();
    }

    private void OnClickRewardCard()
    {
        buttonClickAction?.Invoke();
    }

    private void OnClickCancelBuyButton()
    {
        CloseUI();
    }

    /// <summary>
    /// 보상 세팅하기
    /// </summary>
    private void InitReward()
    {
        //TODO: StageManager.Instance.RewardManager로 접근하기
        itemRewards = TestManager.Instance.RewardManager.GetRandomItemReward();
        for (var i = 0; i < rewardCards.Length; i++)
        {
            rewardCards[i].rewardName.text = itemRewards[i].itemName;
            rewardCards[i].timeCost.text = $"{itemRewards[i].cost:N2}s";
        }
    }

    private void RewardSelectAction(ItemData itemData)
    {
        TestManager.Instance.RemainTime -= itemData.cost;
        
        TestManager.Instance.HUDUI.UpdateStatValue(itemData.stat.rclValue, itemData.stat.hdlValue, itemData.stat.stpValue, itemData.stat.spdValue);
    }
}