using System.Linq;
using DataDeclaration;
using UnityEngine;
using UnityEngine.UI;

public class PopupReward : PopupUI
{
    private ItemSO[] itemRewardPool;   // 모든 아이템 SO
    
    private ItemSO[] itemRewards;
    [SerializeField] private RewardCard[] rewardCards;

    [SerializeField] private Button cancelBuyBtn;

    protected override void Awake()
    {
        base.Awake();
        itemRewardPool = ResourceManager.Instance.LoadAll<ItemSO>("Data/SO/ItemSO");
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
        itemRewards = GetRandomItemReward();
        for (var i = 0; i < rewardCards.Length; i++)
        {
            //TODO: 아이템 아이콘 이미지 넣기 추가
            //rewardCards[i].rewardImage.sprite = itemRewards[i].Icon;
            rewardCards[i].rewardName.text = itemRewards[i].Name;
            rewardCards[i].timeCost.text = $"-{itemRewards[i].cost:N2}s";
            rewardCards[i].rewardButton.onClick.RemoveAllListeners();
            var item = itemRewards[i];
            
            if (itemRewards[i].ApplicationTarget == (int)ItemApplyType.Player)
            {
                rewardCards[i].rewardButton.onClick.AddListener(() => StageManager.Instance.Player.Stat.IncreaseStat(item.RCL, item.HDL, item.STP, item.SPD));
                rewardCards[i].rewardButton.onClick.AddListener(() =>
                    ((HUDUI)UIManager.Instance.CurMainUI).UpdateStatValue(StageManager.Instance.Player.Stat.RCL, StageManager.Instance.Player.Stat.HDL, StageManager.Instance.Player.Stat.STP, StageManager.Instance.Player.Stat.SPD));
            }
            else
            {
                rewardCards[i].rewardButton.onClick.AddListener(() => StageManager.Instance.Player.Weapon.Stat.ChangeStat(item.DMG, item.ShootRecoil, item.MaxAmmo, item.WeaponParts));
            }
            rewardCards[i].rewardButton.onClick.AddListener(CloseUI);
            rewardCards[i].rewardButton.onClick.AddListener(() => StageManager.Instance.RemainTime -= item.cost);
        }
    }

    protected override void CloseUI()
    {
        base.CloseUI();
        if (StageManager.Instance.Player.Controller != null)
        {
            UIManager.ToggleMouseCursor(false);
            StageManager.Instance.PauseGame(false);
        }
    }

    //TODO: 조건 더 추가해야 함
    /// <summary>
    /// Item Pool에서 중복 없이 랜덤으로 3개 반환
    /// </summary>
    private ItemSO[] GetRandomItemReward()
    {
        return itemRewardPool.OrderBy(o => Random.value).Take(3).ToArray();
    }
}