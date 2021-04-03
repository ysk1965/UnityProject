using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

public class NumberConst
{
    public static int MAX_SORTING_ORDER = 32767;

    public static float exp = 10;

    public static int MAX_LV = 100;
    public static int MAX_SHOP_ITEM_LV = 50;
    public static int MONSTER_LOOP_COUNT = 30;

    // SKILL
    public static int MAX_SKILL_LV = 10;

    // UNLOCK
    public static int UNLOCK_SHOP_LV = 9999999;

    // REBIRTH
    public static int REBIRTH_STAGE = 30;
    public static int REBIRTH_UP_PER_LV = 5;
    public static int REBIRTH_FEVER_TIME_SCALE = 4;

    // TIME
    public static int DUNGEON_COIN_TIME = 600;
    public static int DUNGEON_EQUIP_TIME = 600;
    public static int DUNGEON_EXP_TIME = 600;
    public static int DUNGEON_RUNE_TIME = 1800;
    public static int CHALLENGE_TREASURE_TIME = 10;
    public static float CHALLENGE_TREASURE_ADD_TIME = 0.15f;
    public static int CHALLENGE_BOSS_TIME = 25;
    public static float DEFAULT_SAFE_TIME = 60f;
    public static double REWARD_REST_COIN_PER_MIN = 200;
    public static float CHECK_AUTO_SKILL_DELAY_TIME = 0.2f;
    public static int STAGE_BOSS_TIME = 30;



    // REINFORCE
    public static int Addtional1Lv = 1;
    public static int Addtional2Lv = 4;
    public static int Addtional3Lv = 7;
    public static int Addtional4Lv = 10;

    // PRICE
    public static int MASTERY_PICK_PRICE = 30;
    public static int MASTERY_UPGRADE_PRICE = 20;
    public static int EQUIP_UPGRADE_REQUIRE_JEWEL = 10000;

    // CHALLENGE
    public static int CHALLENGE_OPEN_ABILITY_JEWEL = 100;
    public static float CHALLENGE_TREASURE_WAVE_VALUE = 40;
    public static int CHALLENGE_BOSS_BASE_DMG = 2500;
    public static float CHALLENGE_BOSS_RESIST = 0.0015f;

    // MASTERY
    public static int MASTERY_MAX_VALUE = 5;
    public static int MASTERY_10_VALUE = 5;

    public static int MASTERY_NOR_VALUE = 1;

    //REST
    public static double MIN_REST_TIME = 5;
    public const int REST_EXP_BOOST_PER_MIN = 10;
    public const float REST_EXP_LEVEL_MULTIPLE = 2;

    //HERO
    public const int HERO_ATK_PER_UP = 1;
    public const int HERO_HP_PER_UP = 100;
    public const int HERO_HEAL_PER_UP = 5;
    public const float HERO_CRI_POWER_PER_UP = 0.005f;
    public const int HERO_DEF_PER_UP = 5;
    public const int HERO_DEF_PER_LV = 0;
    public const float DEFAULT_CRI_POWER = 1.5f; //기본 크리티컬은 150%
    public const float DEFAULT_ATK_SPEED = 1f;
    public const float DEFAULT_MOVE_SPEED = 12f;
    public const float DEFAULT_CRI_RATE = 0.1f;
    public const float DEFAULT_HP_HEAL_TIME = 1f;
    public const float DEFAULT_MANA_HEAL_TIME = 1f;
    public const int DEFAULT_MANA = 100;
    public const float DEFAULT_MANA_HEAL = 15f;
    public const float DEFAULT_DAMAGE_RED = 0f;
    public const int UPGRADE_LV2_MULTIPLE = 2;
    public static ObscuredInt DEFALUT_INVEN_MAX = 300;

    //UPGRADE
    public static ObscuredInt UP_PRICE_PER_LV_COIN = 10;
    public static ObscuredInt UP_PRICE_PER_LV_STONE = 1;
    public static ObscuredInt UP_FOR_EQUIP_LV = 50;

    //COIN
    public const int DEFAULT_COIN_PER_MONSTER = 20;
    public const int STAGE_COIN_PER_MONSTER = 2;
    public const int BOSS_COIN_MULTIPLIER = 10;
    public const int DEFAULT_REWARD_COIN = 25000;
    public const int REWARD_COIN_PER_LEVEL = 20000;
    public const int REWARD_COIN_PER_STAGE = 10000;
    public static ObscuredInt PIGGY_DEFAULT = 5000000;
    public static ObscuredInt PIGGY_ADD_LV = 100000;

    //DUNGEON
    public const int DUNGEON_MAX_LV = 25;
    public const int DUNGEON_BASE_ATTR = 2500;
    public const float DUNGEON_DIFFICULTY_MULTIPLE = 1.5F;
    public const float DUNGEON_COIN_MULTIPLE = 1.1F;
    public const float DUNGEON_EXP_MULTIPLE = 1.2F;

    // PVP
    public static ObscuredInt PVP_LEVEL = 20;
    public static ObscuredInt PVP_MAX_TIME = 15;
    public static ObscuredInt PVP_JEWEL_PRICE = 50;
    public static ObscuredInt PVP_REGEN_TIME = 15 * 60;
    public static ObscuredInt PVP_SCORE = 10;

    // attr multiple
    public const int ATTR_ATK = 10;
    public const int ATTR_DMG_RED = 1;
    public const int ATTR_DEF = 10;
    public const int ATTR_HP_MAX = 1;
    public const int ATTR_HP_REGEN = 10;
    public const int ATTR_MANA_MAX = 10;
    public const int ATTR_MANA_REGEN = 100;
    public const int ATTR_SKILL_COMMON = 1000;

}