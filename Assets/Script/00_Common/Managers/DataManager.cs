using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using CodeStage.AntiCheat.ObscuredTypes;

[Flags]
public enum DataType
{
    All = 0,
    User = 1,
    Card = 2,
    Time = 4,
    Purchase = 8,
    Season = 16,
    Quest = 32,
    SpecialShop = 64,
    WinReward = 128,
    GuideMission = 256,
    Upgrade = 512,
}

public enum CurrencyLocation
{
    NONE,
    WATCH_AD_MUTLI_REWARD,
    REWARD_SEASON_WEEKLY,
    REWARD_SEASON,
    REWARD_WIN,
    REWARD_QUEST,
    REWARD_GUIDE_MISSION,
    COUPON,
    FREE_CHEST,
    CROWN_CHEST,
    SHOP_CHEST,
    UPGRADE_CARD,
    UPGRADE,
    NAME_CHANGE,
    GAME_RESULT,
    PACKAGE_LEGEND_RAPID,
    PACKAGE_REVENGE,
    PACKAGE_REVENGE_2,
    PACKAGE_DECK,
    SHOP_SPECIAL_RESET,
    SHOP,
    SHOP_DAILY,
    SHOP_PACKAGE,
    SHOP_STARTER,
    SHOP_CHEST_1 = 60201,
    SHOP_CHEST_2 = 60202,
    SHOP_CHEST_3 = 60203,
    ATTENDANCE,
}

public class DataManager : GameObjectSingleton<DataManager>
{

    /////////////////////////////////////////////////////////////
    // public
    public bool IsDataLoaded { get => this.isDataLoaded; }
    public UserData UserData { get => this.user; }
    public TimeData UserTimeData { get => this.time; }
    public List<PurchaseProductData> Purchases { get => this.purchases; }
    public List<QuestData> Quests { get => this.quests; }
    public List<UpgradeData> Upgrades { get => this.upgrades; }


    public long Coin { get => this.coin; }
    public long Jewel { get => this.jewel; }
    public int Crown { get => this.crown; }

    public void AddCoin(long amount, CurrencyLocation loc)
    {
        this.coin += amount;
        // FacebookManager.Instance.LogCurrencyEvent(CurrencyType.COIN, loc, (int)amount);
    }

    public void UseCoin(long amount, CurrencyLocation loc)
    {
        this.coin -= amount;
        // FacebookManager.Instance.LogCurrencyEvent(CurrencyType.COIN, loc, (int)amount);
    }

    public void AddJewel(long amount, CurrencyLocation loc)
    {
        this.jewel += amount;
        DataManager.Instance.user.accGetJewel += amount;
        // FacebookManager.Instance.LogCurrencyEvent(CurrencyType.JEWEL, loc, (int)amount);
    }

    public void UseJewel(long amount, CurrencyLocation loc)
    {
        this.jewel -= amount;
        DataManager.Instance.user.accUseJewel += amount;
        // FacebookManager.Instance.LogCurrencyEvent(CurrencyType.JEWEL, loc, (int)amount);
    }

    public void AddCrown(int amount)
    {
        this.crown += amount;
    }

    public void UseCrown(int amount)
    {
        this.crown -= amount;
    }

    public void AddPurchaseRecord(string productID, string receipt)
    {
        PurchaseProductData data = new PurchaseProductData();
        data.id = productID;
        data.time = NetworkManager.UtcNow.ToString();
        data.receipt = receipt;
        this.purchases.Add(data);
    }

    public void SetServerData(LobbyInfo data)
    {
        if (data.UserData == null || data.UserData.user == null)
        {
            this.CreateAllData();
            this.SaveData(DataType.All);
        }
        else
        {
            DataType type = DataType.User;
            this.SetUserData(data.UserData.user);

            if (this.SetTimeData(data.UserData.time))
            {
                type |= DataType.Time;
            }

            if (this.SetPurchaseData(data.UserData.purchases))
            {
                type |= DataType.Purchase;
            }

            if (this.SetQuestData(data.UserData.quests))
            {
                type |= DataType.Quest;
            }

            if (this.SetUpgradeData(data.UserData.upgrades))
            {
                type |= DataType.Upgrade;
            }

            this.SaveData(type);
        }

        this.LoadLocalData();
    }

    public void SaveData(DataType type)
    {

        List<InfoBase> requestList = new List<InfoBase>();

        if (type == DataType.All || type.HasFlag(DataType.User))
        {
            long lastJewel = this.user.jewel;
            if (Mathf.Abs(this.jewel - lastJewel) > 100000)
            {
                this.user.isCheater = true;
            }
            else
            {
                this.user.ver = NumberUtil.GetVersionAsNumber();
                this.user.coin = this.coin;
                this.user.jewel = this.jewel;
                this.user.crown = this.crown;
            }
            requestList.Add(NetworkManager.Instance.GetUserRequest(this.user));
        }

        if (type == DataType.All || type.HasFlag(DataType.Time))
        {
            requestList.Add(NetworkManager.Instance.GetTimeRequest(this.time));
        }
        if (type == DataType.All || type.HasFlag(DataType.Purchase))
        {
            requestList.Add(NetworkManager.Instance.GetPurchaseRequest(this.purchases));
        }
        if (type == DataType.All || type.HasFlag(DataType.Quest))
        {
            requestList.Add(NetworkManager.Instance.GetQuestRequest(this.quests));
        }
        if (type == DataType.All || type.HasFlag(DataType.Upgrade))
        {
            requestList.Add(NetworkManager.Instance.GetUpgradeRequest(this.upgrades));
        }

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            GameManager.Instance.ShowMoveToMain("네트워크 연결을 확인하세요.");
            return;
        }

        if (NetworkManager.Instance == null) return;

        NetworkManager.Instance.SaveDataAll(requestList, (state, result) =>
        {

        });
    }

    public void SetConfig(List<ConfigData> configList)
    {
        this.configList = configList;
    }

    public string GetConfig(ConfigType type)
    {
        ConfigData config = this.configList.Find(c => c.key.Equals(type.ToString()));
        if (config != null) return config.value;
        else return null;
    }

    public int GetIntConfig(ConfigType type)
    {
        ConfigData config = this.configList.Find(c => c.key.Equals(type.ToString()));
        if (config != null) return int.Parse(config.value);
        else return 0;
    }

    public float GetFloatConfig(ConfigType type)
    {
        ConfigData config = this.configList.Find(c => c.key.Equals(type.ToString()));
        if (config != null) return float.Parse(config.value);
        else return 0f;
    }

    public bool HasPurchaseRecord(PurchaseProductID id)
    {
        return this.purchases.Exists(r => r.id.Equals(id.ToString()));
    }

    public bool HasPurchaseRecord(string id)
    {
        return this.purchases.Exists(r => r.id.Equals(id.ToString()));
    }

    public bool HasAnyPurchaseRecord()
    {
        return this.purchases.Count > 0;
    }

    public int GetPurchaseCount(PurchaseProductID id)
    {
        return this.purchases.Count(r => r.id.Equals(id.ToString()));
    }

    public bool HasPurchaseRecordThisYear(string id)
    {
        PurchaseProductData record = this.purchases.FindLast(r => r.id.Equals(id));
        if (record == null)
        {
            return false;
        }
        else
        {
            DateTime result = NetworkManager.UtcNow;
            DateTime.TryParse(record.time, out result);
            return result.Year == NetworkManager.UtcNow.Year;
        }
    }

    public bool HasPurchaseRecordThisMonth(PurchaseProductID id)
    {
        PurchaseProductData record = this.purchases.FindLast(r => r.id.Equals(id.ToString()));
        if (record == null)
        {
            return false;
        }
        else
        {
            DateTime result = NetworkManager.UtcNow;
            DateTime.TryParse(record.time, out result);
            return result.Year == NetworkManager.UtcNow.Year && result.Month == NetworkManager.UtcNow.Month;
        }
    }

    public bool HasPurchaseRecordToday(PurchaseProductID id)
    {
        PurchaseProductData record = this.purchases.FindLast(r => r.id.Equals(id.ToString()));
        if (record == null)
        {
            return false;
        }
        else
        {
            DateTime result = NetworkManager.UtcNow;
            DateTime.TryParse(record.time, out result);
            return result.Year == NetworkManager.UtcNow.Year && result.DayOfYear == NetworkManager.UtcNow.DayOfYear;
        }
    }

    public void ResetQuests()
    {
        this.quests = this.CreateQuestData();
    }

    public float GetQuestRewardBonusRate()
    {
        UpgradeData data = this.upgrades.Find(u => u.id == (int)UpgradeID.QUEST_REWARD);
        return data.MetaData.value + (data.MetaData.inc_value) * data.Lv;
    }

    public float GetCrownBonusRate()
    {
        UpgradeData data = this.upgrades.Find(u => u.id == (int)UpgradeID.EARNING_CROWN);
        return data.MetaData.value + (data.MetaData.inc_value) * data.Lv;
    }

    public float GetTrophyBonusRate()
    {
        UpgradeData data = this.upgrades.Find(u => u.id == (int)UpgradeID.EARNING_TROPHY);
        return data.MetaData.value + (data.MetaData.inc_value) * data.Lv;
    }

    public float GetFreeChestTime()
    {
        UpgradeData data = this.upgrades.Find(u => u.id == (int)UpgradeID.FREE_CHEST_WAITING);
        return data.MetaData.value + (data.MetaData.inc_value) * data.Lv;
    }

    public float GetCardUpgradePriceRate()
    {
        UpgradeData data = this.upgrades.Find(u => u.id == (int)UpgradeID.CARD_UPGRADE_COST);
        return data.MetaData.value + (data.MetaData.inc_value) * data.Lv;
    }

    public float GetCardPowerRate()
    {
        UpgradeData data = this.upgrades.Find(u => u.id == (int)UpgradeID.POWER);
        return data.MetaData.value + (data.MetaData.inc_value) * data.Lv;
    }

    public float GetCriticalDamageRate()
    {
        UpgradeData data = this.upgrades.Find(u => u.id == (int)UpgradeID.CRITICAL_DAMAGE);
        return data.MetaData.value + (data.MetaData.inc_value) * data.Lv;
    }

    public float GetSpecialShopRate()
    {
        UpgradeData data = this.upgrades.Find(u => u.id == (int)UpgradeID.SPECIAL_OFFER_PRICE);
        return data.MetaData.value + (data.MetaData.inc_value) * data.Lv;
    }

    public int GetStartStar()
    {
        UpgradeData data = this.upgrades.Find(u => u.id == (int)UpgradeID.STARTING_STAR);
        return (int)(data.MetaData.value + (data.MetaData.inc_value) * data.Lv);
    }

    public void CheckUseJewelCheating(System.Action action)
    {
        long before = DataManager.Instance.Jewel;
        action();
        if (DataManager.Instance.Jewel > before)
        {
            DataManager.Instance.UserData.isCheater = true;
            DataManager.Instance.SaveData(DataType.User);
        }
    }

#if UNITY_EDITOR
    public void SetCoin(long amount)
    {
        if (Application.isEditor)
            this.coin = amount;
    }

    public void SetJewel(long amount)
    {
        if (Application.isEditor)
            this.jewel = amount;
    }
#endif

    /////////////////////////////////////////////////////////////
    // protected

    /////////////////////////////////////////////////////////////
    // private

    private bool isDataLoaded = false;
    private UserData user;
    private TimeData time;
    private List<PurchaseProductData> purchases;
    private List<QuestData> quests;
    private List<ConfigData> configList;
    private List<UpgradeData> upgrades;

    private ObscuredLong coin;
    private ObscuredLong jewel;
    private ObscuredInt crown;

    private List<UserRankingInfo> rankings;
    private UserRankingInfo myRanking;
    private long rankingUpdatedTime;

    private void CreateAllData()
    {
        Debug.LogColor("CreateAllData");
        this.user = this.CreateUserData();
        this.time = new TimeData();
        this.purchases = new List<PurchaseProductData>();
        this.quests = this.CreateQuestData();
        this.upgrades = this.CreateUpgradeData();
    }

    private void LoadLocalData()
    {
        this.isDataLoaded = true;
    }

    //서버로부터 데이터를 저장세팅할때 가장 마지막에 세팅한다.
    private void SetUserData(UserData data)
    {
        this.user = data;
        this.coin = this.user.coin;
        this.jewel = this.user.jewel;
        this.crown = this.user.crown;
        if (string.IsNullOrEmpty(this.user.lan))
        {
            this.user.lan = LanguageManager.Instance.GetRecommand().ToString();
        }
    }

    private bool SetTimeData(TimeData data)
    {
        this.time = data;
        if (this.time == null)
        {
            this.time = new TimeData();
            return true;
        }
        return false;
    }

    private bool SetPurchaseData(List<PurchaseProductData> data)
    {
        this.purchases = data;
        if (this.purchases == null)
        {
            this.purchases = new List<PurchaseProductData>();
            return true;
        }
        return false;
    }

    private bool SetQuestData(List<QuestData> quests)
    {
        this.quests = quests;
        if (this.quests == null)
        {
            this.quests = this.CreateQuestData();
            return true;
        }
        else
        {
            bool needSaveToServer = false;

            //서버로부터 받은 리스트에 스펙데이터를 세팅한다.
            foreach (QuestData data in this.quests)
            {
                data.UpdateSpecData();
            }

            //스펙데이터엔 있으나 리스트에 없는 항목은 신규임으로 생성하여 리스트에 추가한 후 서버에 저장한다.
            foreach (QuestMetaData metaData in SpecDataManager.Instance.Quests)
            {
                if (!this.quests.Exists(u => u.id == metaData.id))
                {
                    this.quests.Add(new QuestData(metaData));
                    needSaveToServer = true;
                }
            }
            return needSaveToServer;
        }
    }

    private bool SetUpgradeData(List<UpgradeData> upgrades)
    {
        this.upgrades = upgrades;
        if (this.upgrades == null)
        {
            this.upgrades = this.CreateUpgradeData();
            return true;
        }
        else
        {
            bool needSaveToServer = false;

            //서버로부터 받은 리스트에 스펙데이터를 세팅한다.
            foreach (UpgradeData data in this.upgrades)
            {
                data.UpdateSpecData();
            }

            //스펙데이터엔 있으나 리스트에 없는 항목은 신규임으로 생성하여 리스트에 추가한 후 서버에 저장한다.
            foreach (UpgradeMetaData metaData in SpecDataManager.Instance.Upgrades)
            {
                if (!this.upgrades.Exists(u => u.id == metaData.id))
                {
                    this.upgrades.Add(new UpgradeData(metaData));
                    needSaveToServer = true;
                }
            }
            return needSaveToServer;
        }
    }

    private UserData CreateUserData()
    {
        UserData userData = new UserData();
        userData.uid = NetworkManager.Instance.UID;
        userData.nickName = string.Empty;
        userData.crown = 0;
        userData.level = 1;
        userData.createdTime = NetworkManager.UtcNow.ToString();

        userData.lan = LanguageManager.Instance.GetRecommand().ToString();
        return userData;
    }

    private void LoadUserTimeData()
    {
        string jsonStr = Preference.LoadPreference(Pref.DATA_TIME, string.Empty);
        if (string.IsNullOrEmpty(jsonStr))
        {
            this.time = this.CreateUserTimeData();
        }
        else
        {
            this.time = JsonConvert.DeserializeObject<TimeData>(jsonStr);
        }
    }

    private TimeData CreateUserTimeData()
    {
        TimeData data = new TimeData();
        return data;
    }

    private List<QuestData> CreateQuestData()
    {
        List<QuestData> returnList = new List<QuestData>();

        foreach (QuestMetaData metaData in SpecDataManager.Instance.Quests)
        {
            returnList.Add(new QuestData(metaData));
        }
        return returnList;
    }

    private List<UpgradeData> CreateUpgradeData()
    {
        List<UpgradeData> returnList = new List<UpgradeData>();

        foreach (UpgradeMetaData metaData in SpecDataManager.Instance.Upgrades)
        {
            returnList.Add(new UpgradeData(metaData));
        }
        return returnList;
    }

    [ContextMenu("Setting Random Uid")]
    void SetRandomUid()
    {
        this.UserData.uid = UnityEngine.Random.Range(1, 9999);
    }
}