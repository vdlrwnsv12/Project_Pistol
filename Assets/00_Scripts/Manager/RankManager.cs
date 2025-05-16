using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    #region Variables

    public GameObject rankPopup; // RankPopup 프리팹 또는 오브젝트

    #endregion

    #region Public Methods

    /// <summary>
    /// 랭크 팝업을 활성화합니다.
    /// </summary>
    public void OpenRankPopup()
    {
        rankPopup.SetActive(true);
    }

    #endregion
}
