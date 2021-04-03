using UnityEngine;

public static class NumberUtil
{
    public static int GetVersionAsNumber()
    {
        int version = 0;
        string[] splites = Application.version.Split('.');
        if (splites.Length == 3)
        {
            version += (1000) * int.Parse(splites[0]);
            version += (100) * int.Parse(splites[1]);
            if (splites[2].Length > 2) splites[2] = splites[2].Substring(0, 2);
            version += int.Parse(splites[2]);
        }
        return version;
    }

    public static float RoundFloat(float v, int c = 2)
    {
        return (float)Mathf.RoundToInt(v * (Mathf.Pow(10, c))) / Mathf.Pow(10, c);
    }

    public static float CalRewardRate(int curStage, int bestStage)
    {
        float baseValue = (bestStage == 0) ? 1.5f : (float)curStage / (float)bestStage;
        return Mathf.Min(1.5f, RoundFloat(baseValue, 2));
    }

    public static float CalBounsRewardRate(int grade, int lv)
    {
        float baseValue = (float)grade * 0.02f;
        baseValue += (float)(lv - 1) * 0.001f;
        return Mathf.Min(2f, RoundFloat(baseValue, 2));
    }

    //rate 0f ~ 1f
    public static bool CheckProbability(float rate)
    {
        return Random.Range(0f, 1f) < rate;
    }

    public static int GetNeedPiece(int grade, int tier)
    {
        switch (tier)
        {
            case 1: return 10 * grade;
            case 2: return 20 + (20 * grade);
            case 3: return 50 + (50 * grade);
        }
        return 0;
    }

    public static int GetTierUpPrice(int grade, int tier)
    {
        switch (tier)
        {
            case 1: return 500 * grade;
            case 2: return 1000 + (1000 * grade);
            case 3: return 5000 + (5000 * grade);
        }
        return 0;
    }

    public static int GetDrawPrice(int drawCount)
    {
        return 10 + (10 * drawCount);
    }
}
