using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public enum UpgradeID
{
    QUEST_REWARD = 10001,
    EARNING_CROWN,
    EARNING_TROPHY,
    FREE_CHEST_WAITING,
    CARD_UPGRADE_COST,
    POWER,
    CRITICAL_DAMAGE,
    SPECIAL_OFFER_PRICE,
    STARTING_STAR,
}

public class UpgradeData
{
    public UpgradeData() { }

    public UpgradeData(UpgradeMetaData metaData)
    {
        this.id = metaData.id;
        this.metaData = metaData;
    }

    [JsonIgnore]
    public int Lv { get => this.lv; }
    [JsonIgnore]
    public int Max { get => this.metaData.max_lv; }
    [JsonIgnore]
    public UpgradeMetaData MetaData { get => this.metaData; }

    public int id;
    public int lv;

    public void UpdateSpecData()
    {
        this.metaData = SpecDataManager.Instance.GetUpgdradeMetaData(this.id);
    }

    private UpgradeMetaData metaData;
}

[Serializable]
public class UpgradeMetaData : LocalizedMetaData
{
    public int limit;

    [JsonConverter(typeof(StringEnumConverter))]
    public ValueType value_type;
    public float value;
    public float inc_value;
    public int max_lv;
}
