using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData
{
    public int id;
    public int atk;
    public int def;
    public float range;
    public int maxHp;
    public int curHp;
    public float speed;
    public float atkDelay;
}

public class HeroData : CharacterData
{

}

[System.Serializable]
public class HeroMetaData : LocalizedMetaData
{

}

public class MonsterData : CharacterData
{

}
