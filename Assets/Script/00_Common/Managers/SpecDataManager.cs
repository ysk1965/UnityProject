using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecDataManager : GameObjectSingleton<SpecDataManager>
{

    /////////////////////////////////////////////////////////////
    // public

    public int SpecVersion { get; set; }
    public bool IsDataLoaded { get => this.isDataLoaded; }
    public List<QuestMetaData> Quests { get => this.specData.Quest; }
    public List<UpgradeMetaData> Upgrades { get => this.specData.Upgrade; }
    public List<StageMetaData> Stages { get => this.specData.Stage; }
    public List<SkillMetaData> Skills { get => this.specData.Skill; }

    public void SetServerSpecData(ServerSpecData data)
    {
        this.specData = data;
        this.isDataLoaded = true;
    }

    public LanguageMetaData GetLanguageData(int id)
    {
        return this.specData.Language.Find(l => l.id == id);
    }

    public QuestMetaData GetQuestData(int id)
    {
        return this.specData.Quest.Find(s => s.id == id);
    }

    public UpgradeMetaData GetUpgdradeMetaData(int id)
    {
        return this.specData.Upgrade.Find(u => u.id == id);
    }

    /////////////////////////////////////////////////////////////
    // private

    private ServerSpecData specData;
    private bool isDataLoaded = false;
}
