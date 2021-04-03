using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ObjectPool
{
    [System.Serializable]
    public class PoolInfo
    {
        public PoolObjectType poolType;
        public GameObject prefab;
        public int poolSize;
        public bool fixedSize;
    }

    [System.Serializable]
    public class UIPoolInfo
    {
        public UIPoolObjectType poolType;
        public GameObject prefab;
        public int poolSize;
        public bool fixedSize;
    }

    public enum PoolObjectType
    {
        NONE = 0,

        game_show_hero = 101,
        game_show_damage = 111,
        game_show_soul = 112,
        game_equip_drop = 121,
        game_attached_curse = 131,
        game_attached_stun = 132,
    }

    public enum UIPoolObjectType
    {
        NONE = 0,
        ui_coin_fx = 101,
        ui_jewel_fx,
        ui_equip_fx,
        ui_time_fx,
        ui_coin_get = 111,
        ui_jewel_get,
        ui_equip_get,
        ui_time_get,
    }
}
