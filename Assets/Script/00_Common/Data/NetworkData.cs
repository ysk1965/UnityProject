using System;
using UnityEngine;
using System.Collections.Generic;

/////////////////////////////////////////////////////////////////////////////////////////////
//Request

[Serializable]
public class InfoBase
{
    public InfoBase() { }
    public InfoBase(int uid)
    {
        this.uid = uid;
    }
    public int uid;
}

[Serializable]
public class RequestBase : InfoBase
{
    public RequestBase()
    {
        this.Init();
    }
    public RequestBase(int uid) : base(uid)
    {
        this.Init();
    }

    public int v;
    public string p;
    public string d;

    private void Init()
    {
        this.v = NumberUtil.GetVersionAsNumber();
        this.p = Application.platform == RuntimePlatform.IPhonePlayer ? "ios" : "android";
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(GameManager.Instance.unityDeviceId))
            this.d = Application.dataPath[0] + SystemInfo.deviceUniqueIdentifier;
        else
            this.d = GameManager.Instance.unityDeviceId;
#else
        this.d = SystemInfo.deviceUniqueIdentifier;
#endif
    }
}

[Serializable]
public class InfoRequest : RequestBase
{
    public string h;
}

[Serializable]
public class AuthLoginRequest : RequestBase
{
    public string auth_id;
    public string auth_platform;
}

[Serializable]
public class NickNameRequest : RequestBase
{
    public NickNameRequest(int uid) : base(uid) { }
    //Required
    public string nickname;
}

[Serializable]
public class UpdateWeeklyRankingRequest : RequestBase
{
    public UpdateWeeklyRankingRequest(int uid) : base(uid) { }
    //Required
    public string key;
    public int score;
    public int update;
    public RankInfo data;
}

[Serializable]
public class SearchWeeklyRankingRequest : RequestBase
{
    public SearchWeeklyRankingRequest(int uid) : base(uid) { }
    //Required
    public string key;
}

[Serializable]
public class SaveDataListRequest : RequestBase
{
    public SaveDataListRequest(int uid) : base(uid) { }

    public List<InfoBase> data_list;
}

[Serializable]
public class SaveUserRequest : InfoBase
{
    public SaveUserRequest(int uid) : base(uid) { }
    public string category;
    public UserData data;
}

[Serializable]
public class SaveTimeRequest : InfoBase
{
    public SaveTimeRequest(int uid) : base(uid) { }
    public string category;
    public TimeData data;
}

[Serializable]
public class SaveQuestRequest : InfoBase
{
    public SaveQuestRequest(int uid) : base(uid) { }
    public string category;
    public List<QuestData> data;
}

[Serializable]
public class SaveUpgradeRequest : InfoBase
{
    public SaveUpgradeRequest(int uid) : base(uid) { }
    public string category;
    public List<UpgradeData> data;
}

[Serializable]
public class SavePurchaseRequest : InfoBase
{
    public SavePurchaseRequest(int uid) : base(uid) { }
    public string category;
    public List<PurchaseProductData> data;
}

[Serializable]
public class PaymentRequest : RequestBase
{
    public PaymentRequest(int uid) : base(uid) { }
    public string product_id;
    public string order_id;
    public string currency_price;
    public string currency_code;
    public string receipt;
}


/////////////////////////////////////////////////////////////////////////////////////////////
//Response

[Serializable]
public class ResponseBase
{
    public int code;
    public string msg;
}

public enum ConfigType
{
    auto_ai_time,
    party_max_count,
    pvp_trophy_penalty,
    party_reward_default,
    party_reward_inc,
    party_value_inc,

}

[Serializable]
public class ServerConfig
{
    public List<ConfigData> config;
}

public class ConfigData
{
    public int id;
    public string key;
    public string value;
    public string filter;
    public string type;
}


[Serializable]
public class ConfigResponse : ResponseBase
{
    public ServerConfig data;
}

[Serializable]
public class VersionInfo
{
    public int status;
    public int spec_version;
    public int app_version;
}

[Serializable]
public class VersionInfoResponse : ResponseBase
{
    public VersionInfo data;
}

[Serializable]
public class AuthLoginInfo
{
    public int isNew;
    public int isNickname;
    public int uid;
    public int isBanned;
    public string token;
}

[Serializable]
public class AuthLoginResponse : ResponseBase
{
    public AuthLoginInfo data;
}

[Serializable]
public class TimeInfo
{
    public long serverTime;
}

[Serializable]
public class TimeInfoResponse : ResponseBase
{
    public TimeInfo data;
}

[Serializable]
public class LobbyInfo
{
    public UserServerData UserData;
    public string nickname;
}

[Serializable]
public class UserServerData
{
    public UserData user;
    public TimeData time;
    public List<QuestData> quests;
    public List<UpgradeData> upgrades;
    public List<PurchaseProductData> purchases;
}

[Serializable]
public class LobbyInfoResponse : ResponseBase
{
    public LobbyInfo data;
}

[Serializable]
public class WeeklyRankingResponse : ResponseBase
{
    public WeeklyRankingInfo data;
}

[Serializable]
public class WeekNumberResponse : ResponseBase
{
    public WeekNumber data;
}

[Serializable]
public class UserRankingInfo
{
    public int rank;
    public string nickName;
    public int score;
    public RankInfo data;
}

[Serializable]
public class WeeklyRankingInfo
{
    public UserRankingInfo myInfo;
    public int weekNumber;
    public int totalUserCount;
    public List<UserRankingInfo> rankings;
}

[Serializable]
public class WeekNumber
{
    public int weekNumber;
}

[Serializable]
public class NickNameResponse : ResponseBase
{

}

[Serializable]
public class EmptyResponse : ResponseBase
{

}