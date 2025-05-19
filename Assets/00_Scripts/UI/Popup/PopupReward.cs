using System.Linq;
using DataDeclaration;
using UnityEngine;
using UnityEngine.UI;

public class PopupReward : PopupUI
{
    private ItemSO[] itemRewardPool;   // 모든 아이템 SO

    private ItemSO[] itemRewards;
    [SerializeField] private Image playerImage;
    private Sprite[] playerSprite;
    [SerializeField] private RewardCard[] rewardCards;

    [SerializeField] private Button cancelBuyBtn;

    private void Awake()
    {
        itemRewardPool = ResourceManager.Instance.LoadAll<ItemSO>("Data/SO/ItemSO");
        playerSprite = ResourceManager.Instance.LoadAll<Sprite>($"Sprites/{GameManager.Instance.selectedCharacter.ID}");
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
        playerImage.sprite = playerSprite[0];

        for (var i = 0; i < rewardCards.Length; i++)
        {
            //TODO: 아이템 아이콘 이미지 넣기 추가
            //rewardCards[i].rewardImage.sprite = itemRewards[i].Icon;
            rewardCards[i].rewardName.text = itemRewards[i].Name;
            rewardCards[i].timeCost.text = $"-{itemRewards[i].cost:N2}s";
            rewardCards[i].rewardButton.onClick.RemoveAllListeners();

            var item = itemRewards[i];

            rewardCards[i].rewardButton.onClick.AddListener(() =>
            {
                if (item.WeaponParts > 0) // 무기 파츠면
                {
                    GameObject part = FindWeaponPartByItemSO(item.ID);
                    if (part != null)
                    {
                        StageManager.Instance.Player.Weapon.ToggleAttachment(part.name);
                    }
                    else
                    {
                        Debug.LogWarning($"무기 파츠 {item.ID}를 게임오브젝트 찾을 수 없습니다.");
                    }
                }
                else if (item.ApplicationTarget == (int)ItemApplyType.Player)
                {
                    StageManager.Instance.Player.Stat.IncreaseStat(item.RCL, item.HDL, item.STP, item.SPD);
                    ((HUDUI)UIManager.Instance.CurMainUI).UpdateStatValue();
                }
                else
                {
                    StageManager.Instance.Player.Weapon.Stat.ChangeStat(item.DMG, item.ShootRecoil, item.MaxAmmo, item.WeaponParts);
                }
                StageManager.Instance.RemainTime -= item.cost;
                CloseUI();
            });

            // if (itemRewards[i].ApplicationTarget == (int)ItemApplyType.Player)
            // {
            //     rewardCards[i].rewardButton.onClick.AddListener(() => StageManager.Instance.Player.Stat.IncreaseStat(item.RCL, item.HDL, item.STP, item.SPD));
            //     rewardCards[i].rewardButton.onClick.AddListener(() =>
            //         ((HUDUI)UIManager.Instance.CurMainUI).UpdateStatValue());
            // }
            // else
            // {
            //     rewardCards[i].rewardButton.onClick.AddListener(() => StageManager.Instance.Player.Weapon.Stat.ChangeStat(item.DMG, item.ShootRecoil, item.MaxAmmo, item.WeaponParts));
            // }
            // rewardCards[i].rewardButton.onClick.AddListener(CloseUI);
            // rewardCards[i].rewardButton.onClick.AddListener(() => StageManager.Instance.RemainTime -= item.cost);
        }
    }

    private GameObject FindWeaponPartByItemSO(string itemID)
    {
        var weapon = StageManager.Instance.Player.Weapon;

        if (weapon == null)
        {
            Debug.LogWarning("Weapon 없음");
            return null;
        }

        var children = weapon.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
             Debug.LogWarning($"찾는중 {child.gameObject.name}");
            if (child.gameObject.name.Equals(itemID, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.LogWarning($"찾음 {itemID}");
                return child.gameObject;
            }
        }
        Debug.LogWarning($"무기 파츠 {itemID}를 찾을 수 없습니다.");
        return null;
    }


    protected override void CloseUI()
    {
        base.CloseUI();
        if (StageManager.Instance.Player.Controller != null)
        {
            UIManager.ToggleMouseCursor(false);
            StageManager.Instance.Player.Controller.enabled = true;
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