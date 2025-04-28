using System;
using DataDeclaration;

public class HitTracker
{
    private const int MAX_SCORE = 999999;
    private int gameScore = 0;

    private int destroyTargetCombo = 0;
    private int maxDestroyTargetCombo = 0;
    private bool isQuickShot = false;
    private float quickShotTimer = 0f;
    private const float QUICK_SHOT_TIME = 0f;

    private float remainTime = 30f;

    private int shotCount = 0;
    private int hitCount = 0;
    int headHitCount = 0;

    #region Properties

    public int GameScore => Math.Min(MAX_SCORE, gameScore);

    public RankType Rank
    {
        get
        {
            switch (gameScore)
            {
                case < 500:
                    return RankType.F;
                case < 1000:
                    return RankType.C;
                case < 2000:
                    return RankType.B;
                case < 3000:
                    return RankType.A;
                default:
                    return RankType.S;
            }
        }
    }

    public int MaxDestroyTargetCombo => maxDestroyTargetCombo;
    public bool IsQuickShot => isQuickShot;
    public float QuickShotTimer => quickShotTimer;

    public float RemainTime
    {
        get => Math.Max(0f, remainTime);
        set => remainTime = value;
    }

    public float ShotAccuracy
    {
        get
        {
            if (shotCount == 0)
            {
                return 0;
            }

            return (float)hitCount / (float)shotCount * 100f;
        }
    }

    public float HeadShotAccuracy
    {
        get
        {
            if (hitCount == 0)
            {
                return 0;
            }

            return (float)headHitCount / (float)hitCount * 100f;
        }
    }

    #endregion
    
    public void AddScore(bool isHeadShot, int finalDamage, float targetDistance, bool isCombo)
    {
        gameScore += isHeadShot ? HeadShotBonusScore(finalDamage) : NormalScore(finalDamage);
        gameScore += RangeShotBonusScore(targetDistance);
    }

    private int NormalScore(int finalDamage)
    {
        return finalDamage;
    }

    private int HeadShotBonusScore(int finalDamage)
    {
        return (int)(finalDamage * 1.5f);
    }

    private int RangeShotBonusScore(float distance)
    {
        return (int)distance;
    }

    private int ComboBonusScore(int combo)
    {
        return combo < 2 ? 0 : Math.Min(combo * 100, 900);
    }

    private int ContinuousDestroyBonusScore(int finalDamage)
    {
        return 0;
    }
}