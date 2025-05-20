using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PopupReward : PopupUI
{
    private ItemSO[] itemRewardPool;   // 모든 아이템 SO
    
    [SerializeField] private Image playerImage;
    private Sprite[] playerSprite;
    [SerializeField] private RewardItemCard[] rewardCards;
    [SerializeField] private Button cancelBuyBtn;
    
    public event Action OnClose;

    private void Awake()
    {
        itemRewardPool = ResourceManager.Instance.LoadAll<ItemSO>("Data/SO/ItemSO");
        playerSprite = ResourceManager.Instance.LoadAll<Sprite>($"Sprites/{GameManager.Instance.selectedCharacter.ID}");

        for (var i = 0; i < rewardCards.Length; i++)
        {
            rewardCards[i].RewardButton.onClick.AddListener(CloseUI);
        }
        
        cancelBuyBtn.onClick.AddListener(CloseUI);
    }

    private void OnEnable()
    {
        // Popup 활성화 할때 마다 보상 아이템 세팅하기
        InitReward();
    }

    /// <summary>
    /// 보상 아이템 세팅하기
    /// </summary>
    private void InitReward()
    {
        playerImage.sprite = playerSprite[0];

        var showItemData = GetRandomItemReward();
        
        for (var i = 0; i < rewardCards.Length; i++)
        {
            rewardCards[i].SetItemData(showItemData[i]);
        }
    }
    
    protected override void CloseUI()
    {
        base.CloseUI();
        if (StageManager.Instance.Player.Controller != null)
        {
            UIManager.ToggleMouseCursor(false);
            StageManager.Instance.Player.Controller.enabled = true;
        }
        OnClose?.Invoke();
        OnClose = null;
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