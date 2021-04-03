using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ServerSpecData
{
    /////////////////////////////////////////////////////////////
    // public

    public List<LanguageMetaData> Language = new List<LanguageMetaData>();
    public List<HeroMetaData> Hero = new List<HeroMetaData>();
    public List<QuestMetaData> Quest = new List<QuestMetaData>();
    public List<UpgradeMetaData> Upgrade = new List<UpgradeMetaData>();
    public List<StageMetaData> Stage = new List<StageMetaData>();
    public List<SkillMetaData> Skill = new List<SkillMetaData>();
}