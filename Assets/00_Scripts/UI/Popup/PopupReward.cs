using DataDeclaration;
using UnityEngine;
using UnityEngine.UI;

public class PopupReward : PopupUI, IPoolable
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
        InitReward();
    }
    
    private void OnClickCancelBuyButton()
    {
        UIManager.Instance.ClosePopUpUI();
    }

    /// <summary>
    /// 보상 세팅하기
    /// </summary>
    private void InitReward()
    {
        //TODO: 보상 데이터 풀에서 보상 3개 가져오기
        for (var i = 0; i < rewardCards.Length; i++)
        {
            //rewardCards[i].rewardImage = ;
            //rewardCards[i].rewardName = ;
            //rewardCards[i].timeCost = ;
        }
    }

    public int ResourceInstanceID { get; set; }
    public GameObject GameObject => gameObject;
}
