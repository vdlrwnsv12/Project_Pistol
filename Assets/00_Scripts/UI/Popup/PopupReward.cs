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
        // //TODO: StageManager.Instance.RewardSystem 접근하기
        // itemRewards = TestManager.Instance.RewardSystem.GetRandomItemReward();
        // for (var i = 0; i < rewardCards.Length; i++)
        // {
        //     rewardCards[i].rewardName.text = itemRewards[i].Name;
        //     rewardCards[i].timeCost.text = $"-{itemRewards[i].cost:N2}s";
        //     rewardCards[i].rewardButton.onClick.RemoveAllListeners();
        //     
        //     var playerItem = itemRewards[i];
        //     if (itemRewards[i].ApplicationTarget == (int)ItemApplyType.Player) // 플레이어 적용 아이템
        //     {
        //         rewardCards[i].rewardButton.onClick.AddListener(() =>
        //             TestManager.Instance.StatHandler.IncreaseStat(playerItem.RCL, playerItem.HDL, playerItem.STP,
        //                 playerItem.SPD));
        //         rewardCards[i].rewardButton.onClick.AddListener(() =>
        //             ((HUDUI)UIManager.Instance.CurMainUI).UpdateStatValue(TestManager.Instance.StatHandler.Stat));
        //     }
        //     else if (itemRewards[i].ApplicationTarget == (int)ItemApplyType.Weapon) // 무기 적용 아이템
        //     {
        //         rewardCards[i].rewardButton.onClick.AddListener(() =>
        //             StageManager.Instance.Player.PlayerEquipment.WeaponStatHandler.ApplyItemStats(playerItem));
        //     }
        //
        //     rewardCards[i].rewardButton.onClick.AddListener(CloseUI);
        // }
    }
}