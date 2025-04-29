using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankPopupUI : MonoBehaviour
{
    #region Variables

    public Transform content;             // RankItem 부모 오브젝트
    public GameObject rankItemPrefab;      // RankItem 프리팹
    public Button closeButton;             // 닫기 버튼

    public Image bestCharacterImage;       // 최고 기록 캐릭터 이미지
    public Text bestRankGradeText;          // 최고 랭크 (S~F)
    public Text bestScoreText;              // 최고 점수

    private List<RankData> rankDatas = new List<RankData>(); // 유저 랭킹 데이터

    #endregion

    #region Unity Methods

    private void Start()
    {
        closeButton.onClick.AddListener(ClosePopup);
        LoadMockData();
        CreateRankList();
        UpdateBestRecord();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// 테스트용 가짜 유저 랭킹 데이터 생성
    /// (실제 적용 시 서버 통신으로 대체)
    /// </summary>
    private void LoadMockData()
    {
        for (int i = 0; i < 20; i++)
        {
            int score = Random.Range(0, 10000);
            rankDatas.Add(new RankData
            {
                userName = $"User_{i + 1}",
                score = score,
                rankGrade = GetRankGrade(score)
            });
        }
    }

    /// <summary>
    /// RankItem을 생성하여 Content에 추가합니다.
    /// </summary>
    private void CreateRankList()
    {
        foreach (var data in rankDatas)
        {
            GameObject item = Instantiate(rankItemPrefab, content);
            item.GetComponent<RankItem>().Setup(data);
        }
    }

    /// <summary>
    /// 최고 기록을 찾아서 상단 UI를 업데이트합니다.
    /// </summary>
    private void UpdateBestRecord()
    {
        if (rankDatas.Count == 0)
        {
            bestRankGradeText.text = "-";
            bestScoreText.text = "-";
            return;
        }

        RankData bestData = rankDatas[0];

        foreach (var data in rankDatas)
        {
            if (data.score > bestData.score)
            {
                bestData = data;
            }
        }

        bestCharacterImage.sprite = GetCharacterSprite(bestData.rankGrade);
        bestRankGradeText.text = bestData.rankGrade;
        bestScoreText.text = bestData.score.ToString();
    }

    /// <summary>
    /// 팝업을 닫습니다.
    /// </summary>
    private void ClosePopup()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 점수에 따라 랭크 등급(S~F)을 반환합니다.
    /// </summary>
    /// <param name="score">점수</param>
    /// <returns>랭크 등급</returns>
    private string GetRankGrade(int score)
    {
        if (score >= 9000) return "S";
        if (score >= 7000) return "A";
        if (score >= 5000) return "B";
        if (score >= 3000) return "C";
        if (score >= 1000) return "D";
        return "F";
    }

    /// <summary>
    /// 랭크 등급에 따라 캐릭터 스프라이트를 반환합니다.
    /// </summary>
    /// <param name="rankGrade">S~F 등급</param>
    /// <returns>캐릭터 스프라이트</returns>
    private Sprite GetCharacterSprite(string rankGrade)
    {
        switch (rankGrade)
        {
            case "S":
                return Resources.Load<Sprite>("Characters/Character_S");
            case "A":
                return Resources.Load<Sprite>("Characters/Character_A");
            case "B":
                return Resources.Load<Sprite>("Characters/Character_B");
            default:
                return Resources.Load<Sprite>("Characters/Character_Default");
        }
    }

    #endregion
}
