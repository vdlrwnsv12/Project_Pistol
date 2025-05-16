using UnityEngine;
using UnityEngine.UI;

public class RankItem : MonoBehaviour
{
    #region Variables

    public Text userNameText;   // 플레이어 이름 텍스트
    public Text scoreText;      // 점수 텍스트
    public Text rankGradeText;  // 최고 랭크 텍스트

    #endregion

    #region Public Methods

    /// <summary>
    /// RankData를 받아서 UI를 세팅합니다.
    /// </summary>
    /// <param name="data">랭크 데이터</param>
    public void Setup(RankData data)
    {
        userNameText.text = data.userName;
        scoreText.text = data.score.ToString();
        rankGradeText.text = data.rankGrade;
    }

    #endregion
}
