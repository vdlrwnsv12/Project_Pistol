using UnityEngine;
using UnityEngine.UI;
using DataDeclaration;

/// <summary>
/// 흐린 배경 위에 좌측부터 점점 뚜렷해지는 로딩 로고 연출 → 완료 시 다음 씬 전환
/// </summary>
public class LoadingUI : MainUI
{
    [Header("로고 이미지")]
    [SerializeField] private Image logoImage; // 반드시 'Filled', 'Horizontal'

    private void Awake()
    {
        SceneLoadManager.Instance.OnLoadProgress += FillByLoadingLogo;
    }

    private void OnDestroy()
    {
        SceneLoadManager.Instance.OnLoadProgress -= FillByLoadingLogo;
    }

    private void FillByLoadingLogo(float progress)
    {
        logoImage.fillAmount = progress;
    }

    public override MainUIType UIType { get; protected set; }
    public override void SetActiveUI(MainUIType activeUIType)
    {
        gameObject.SetActive(activeUIType == UIType);
    }
}
