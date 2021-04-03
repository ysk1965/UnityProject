using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public enum QuestType
{
    QUEST,
    REWARD,
}

public enum QuestID
{
    DAILY_LOGIN = 10001,
    BATTLE_PLAY,
    BATTLE_WIN,
    PVP_TROPHY,
    PARTY_CROWN,
    PARTY_PLAY,
    CROWN_CHEST_OPEN,
    FREE_CHEST_OPEN,
    PLAY_WITH_FRIEND,
    BUY_SPECIAL_SHOP,
    QUEST_DONE_2 = 10101,
    QUEST_DONE_5,
    QUEST_DONE_8,
}

[Serializable]
public class QuestData
{
    public QuestData() { }

    public QuestData(QuestMetaData metaData)
    {
        this.id = metaData.id;
        this.metaData = metaData;
    }

    [JsonIgnore]
    public int Cur { get => this.cur; }
    [JsonIgnore]
    public int Max { get => this.metaData.value; }
    [JsonIgnore]
    public float Rate { get => (float)this.cur / (float)this.metaData.value; }
    [JsonIgnore]
    public bool IsDone { get => this.cur >= this.metaData.value; }
    [JsonIgnore]
    public bool IsRewarded { get => this.rewarded; }
    [JsonIgnore]
    public QuestMetaData MetaData { get => this.metaData; }

    public int id;
    public int cur;
    public bool rewarded = false;

    public void UpdateSpecData()
    {
        this.metaData = SpecDataManager.Instance.GetQuestData(this.id);
    }

    private QuestMetaData metaData;
}


[Serializable]
public class QuestMetaData : LocalizedMetaData
{
    [JsonConverter(typeof(StringEnumConverter))]
    public QuestType type;
    public int value;

    [JsonConverter(typeof(StringEnumConverter))]
    public RewardType reward_type;
    public int reward_count;
}