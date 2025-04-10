using DataDeclaration;
using UnityEngine;
using UnityEngine.UI;

public class PopupReward : PopupUI
{
    [SerializeField] private RewardCard[] rewardCards;
    [SerializeField] private Button cancelBuyBtn;

    protected override void Awake()
    {
        base.Awake();
        cancelBuyBtn.onClick.AddListener(OnClickCancelBuyButton);
    }

    private void OnEnable()
    {
        // Popup 활성화 할때 마다 보상 세팅하기
        //InitReward();
    }

    private void OnClickCancelBuyButton()
    {
        CloseUI();
    }

    /// <summary>
    /// 보상 세팅하기
    /// </summary>
    // private void InitReward()
    // {
    //     var rewardList = TestManager.Instance.RewardManager.GetRandomItemReward();
    //     for (var i = 0; i < rewardCards.Length; i++)
    //     {
    //         rewardCards[i].rewardName.text = rewardList[i].itemName;
    //         rewardCards[i].timeCost.text = $"{rewardList[i].cost:N2}s";
    //     }
    // }
}