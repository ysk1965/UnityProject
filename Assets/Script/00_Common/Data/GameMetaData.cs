using System;

public enum RewardType
{
    COIN = 1000,
    JEWEL,
    CHEST = 1100,
    CARD = 1200,
    CARD_1,
    CARD_2,
    CARD_3,
    CARD_4,
}

public enum ValueType
{
    NUMBER,
    TIME,
    PERCENT,
}

public enum CurrencyType
{
    COIN,
    JEWEL,
    CROWN,
}


[Serializable]
public class GameMetaData
{
    public int id;
}

[Serializable]
public class LocalizedMetaData : GameMetaData
{
    public string name_kr;
    public string name_en;
    public string name_jp;
    public string name_tc;
    public string name_sc;

    public string desc_kr;
    public string desc_en;
    public string desc_jp;
    public string desc_tc;
    public string desc_sc;

    public string GetName()
    {
        switch (LanguageManager.Instance.Language)
        {
            case Language.EN: return name_en;
            case Language.KR: return name_kr;
            case Language.JP: return name_jp;
            case Language.TC: return name_tc;
            case Language.SC: return name_sc;
        }
        return string.Empty;
    }

    public string GetDesc()
    {
        switch (LanguageManager.Instance.Language)
        {
            case Language.EN: return desc_en;
            case Language.KR: return desc_kr;
            case Language.JP: return desc_jp;
            case Language.TC: return desc_tc;
            case Language.SC: return desc_sc;
        }
        return string.Empty;
    }
}