using System;

public class ScoreSystem
{
    private const int MAX_SCORE = 999999;
    private int score;
    public int Score => Math.Min(MAX_SCORE, score);

    public int Combo { get; set; }

    public ScoreSystem()
    {
        score = 0;
    }

    public void AddScore(bool isHeadShot, int finalDamage, float targetDistance, bool isCombo)
    {
        score += isHeadShot ? HeadShotBonusScore(finalDamage) : NormalScore(finalDamage);
        score += RangeShotBonusScore(targetDistance);
        score += ComboBonusScore(Combo);
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