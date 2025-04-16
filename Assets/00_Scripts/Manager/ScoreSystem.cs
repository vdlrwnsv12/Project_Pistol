using System;
using DataDeclaration;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    private int score;

    public RankType RankType
    {
        get
        {
            // 점수에 따른 등급
            if (score < 10000)
            {
                return RankType.F;
            }
            else if (score < 50000)
            {
                return RankType.C;
            }
            else if (score < 100000)
            {
                return RankType.B;
            }
            else if (score < 500000)
            {
                return RankType.A;
            }
            else
            {
                return RankType.S;
            }
        }
    }
    
    /// <summary>
    /// 기본 점수 계산
    /// </summary>
    public int BasicScore(float damage)
    {
        score += (int)damage;
        Debug.Log($"기본 점수 획득: {score}");
        return score;
    }

    /// <summary>
    /// 헤드샷 보너스 점수 계산
    /// </summary>
    public int HeadShotScore(float damage)
    {
        score += (int)(damage * 1.5f);
        Debug.Log($"헤드샷 보너스: {score}");
        return score;
    }

    /// <summary>
    /// 원거리 보너스 점수 계산
    /// </summary>
    public int RangeBonusScore(Vector3 playerPos, Vector3 targetPos)
    {
        var distance = Vector3.Distance(playerPos, targetPos);
        Debug.Log($"원거리 점수 보너스: {distance}");
        
        return 0;
    }

    /// <summary>
    /// 콤보 점수 계산
    /// </summary>
    public int ComboBonusScore(int combo)
    {
        return combo < 2 ? 0 : Math.Min(combo * 100, 900);
    }

    /// <summary>
    /// 연속 파괴 보너스 점수 계산
    /// </summary>
    public int ContinuousDestructionBonusScore()
    {
        return 0;
    }
}
