using System.Security.AccessControl;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

[Serializable]
public class UserData
{
    public int uid;
    public int level;
    public int exp;
    public int ver;
    public string createdTime;
    public string nickName;
    public int weekIndex; //서버기준 주단위 인덱스
    public int crown;
    public int bCrown; //베스트 기록
    public int victory;
    public int wStreak; //연승
    public int lStreak; //연패
    public int defeat;
    public int panelty;
    public long coin;
    public long jewel;
    public int tuto = 0; //튜토리얼 단계
    public string lan = ""; // 언어

    public long accGetJewel;
    public long accUseJewel;
    public string nVersion = "";
    public bool isCheater = false;
}

[Serializable]
public class TimeData
{
    ///<summary>Next Free Chest Time</summary>
    public long nFCT;

    ///<summary>Last Login Day</summary>
    public long latLogin;

    ///<summary>Last login Week</summary>
    public int latWeekIndex;

    ///<summary>Last Rewarded Week</summary>
    public int lastRWeek;

    ///<summary>accumulated Login Day</summary>
    
    public long accLogin;
    
    ///<summary>출석 날짜</summary>
    public int attDay;

    ///<summary>출석 보상 받은지 여부</summary>
    public bool bAttR;

    ///<summary>Next Review Ask Time</summary>
    public long nReviewT;

    public List<TimePackage> legendPacks = new List<TimePackage>();
    public TimePackage revengePack;
    public TimePackage d1Pack;
}

[Serializable]
public class RankInfo
{
    public int lv;
    public float wRate;
}

[Serializable]
public class TimePackage
{
    public int id;
    public long endTime;
}