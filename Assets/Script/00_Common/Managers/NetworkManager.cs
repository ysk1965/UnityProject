using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Globalization;
using CodeStage.AntiCheat.Detectors;

public enum NetworkState
{
    NORMAL,
    ERROR,
    NEED_LOGIN,
}

public class NetworkManager : GameObjectSingleton<NetworkManager>
{
    /////////////////////////////////////////////////////////////
    // public

    public static DateTime UtcNow
    {
        get { return DateTime.UtcNow.AddSeconds(gapSecond); }
    }

    public static DateTime Tommorrow
    {
        get
        {
            DateTime now = UtcNow;
            return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(1);
        }
    }

    public static void SetServerTime(long serverTime)
    {
        long deviceTime = (long)(DateTime.UtcNow - Jan1st1970).TotalSeconds;
        gapSecond = serverTime - deviceTime;
    }

    //닷넷 기준 초
    public static double NowSeconds
    {
        get
        {
            const double ticks2seconds = 1 / (double)TimeSpan.TicksPerSecond;
            long ticks = NetworkManager.UtcNow.Ticks;
            double seconds = ((double)ticks) * ticks2seconds;
            return seconds;
        }
    }

    public static long NowDays
    {
        get
        {
            const double ticks2days = 1 / (double)TimeSpan.TicksPerDay;
            long ticks = NetworkManager.UtcNow.Ticks;
            long days = (long)(((double)ticks) * ticks2days);
            return days;
        }
    }

    public static double NowMilliseconds
    {
        get
        {
            const double ticks2seconds = 1 / (double)TimeSpan.TicksPerMillisecond;
            long ticks = NetworkManager.UtcNow.Ticks;
            double seconds = ((double)ticks) * ticks2seconds;
            return seconds;
        }
    }

    //서버 기준 초
    public static double ServerNowSeconds
    {
        get
        {
            return (NetworkManager.UtcNow - Jan1st1970).TotalSeconds;
        }
    }

    private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static long gapSecond = 0;

    /////////////////////////////////////////////////////////////
    // public

    public int UID { get => this.uid; }

    public string GetServerName()
    {
#if SERVER_REAL
        return "REAL";
#else
        return "DEV";
#endif
    }

    public void CheckVersion(string h, Action<NetworkState, VersionInfo> onResult)
    {
        InfoRequest param = new InfoRequest();
        param.h = h;

        this.StartCoroutine(RESTfulApi.Post(this.GetApiURL("version"), null, JsonConvert.SerializeObject(param), (success, result) =>
        {
            if (success)
            {
                VersionInfoResponse response = JsonConvert.DeserializeObject<VersionInfoResponse>(result);
                onResult(NetworkState.NORMAL, response.data);
            }
            else
            {
                onResult(NetworkState.ERROR, null);
            }
        }));
    }

    public void GetConfig(Action<NetworkState, ServerConfig> onResult)
    {
        RequestBase param = new RequestBase();

        this.StartCoroutine(RESTfulApi.Post(this.GetApiURL("config"), null, JsonConvert.SerializeObject(param), (success, result) =>
        {
            if (success)
            {
                ConfigResponse response = JsonConvert.DeserializeObject<ConfigResponse>(result);
                onResult(NetworkState.NORMAL, response.data);
            }
            else
            {
                onResult(NetworkState.ERROR, null);
            }
        }));
    }

    public void GetSpecData(int verison, Action<NetworkState, string> onResult)
    {
        this.StartCoroutine(RESTfulApi.Get(string.Format(this.GetSpecDataUrl(), verison), null, (success, result) =>
        {
            if (success)
            {
                onResult(NetworkState.NORMAL, result);
            }
            else
            {
                onResult(NetworkState.ERROR, null);
            }
        }));
    }


    public void UpdateServerTime(Action onComplete = null)
    {
        this.isTimeRefreshing = true;
        this.StartCoroutine(RESTfulApi.Post(this.GetApiURL("time"), null, "{}", (success, result) =>
        {
            if (success)
            {
                this.isTimeRefreshing = false;
                TimeInfoResponse response = JsonConvert.DeserializeObject<TimeInfoResponse>(result);
                NetworkManager.SetServerTime(response.data.serverTime);

                if (onComplete != null) onComplete();
            }
            else
            {
                Run.After(1f, () =>
                {
                    this.UpdateServerTime(onComplete);
                });
            }
        }));
    }

    public void AuthLogin(string authID, string authPlatform, Action<NetworkState, AuthLoginInfo> onResult)
    {
        AuthLoginRequest param = new AuthLoginRequest();
        param.auth_id = authID;
        param.auth_platform = authPlatform;

        this.StartCoroutine(RESTfulApi.Post(this.GetApiURL("auth/login"), null, JsonConvert.SerializeObject(param), (success, result) =>
        {
            if (success)
            {
                AuthLoginResponse response = JsonConvert.DeserializeObject<AuthLoginResponse>(result);
                this.uid = response.data.uid;
                this.token = response.data.token;
                onResult(NetworkState.NORMAL, response.data);
            }
            else
            {
                onResult(NetworkState.ERROR, null);
            }
        }));
    }

    public void LobbyInfo(Action<NetworkState, LobbyInfo> onResult)
    {
        RequestBase param = new RequestBase(this.uid);

        this.StartCoroutine(RESTfulApi.Post(this.GetApiURL("users/lobby"), this.token, JsonConvert.SerializeObject(param), (success, result) =>
        {
            if (success)
            {
                LobbyInfoResponse response = JsonConvert.DeserializeObject<LobbyInfoResponse>(result);
                onResult(NetworkState.NORMAL, response.data);
            }
            else
            {
                onResult(NetworkState.ERROR, null);
            }
        }));
    }

    public void SaveDataAll(List<InfoBase> dataList, Action<NetworkState, int> onResult)
    {
        SaveDataListRequest param = new SaveDataListRequest(this.uid);
        param.data_list = dataList;
        string reqeustJson = JsonConvert.SerializeObject(param);

        this.StartCoroutine(RESTfulApi.Post(this.GetApiURL("users/data/save"), this.token, reqeustJson, (success, result) =>
        {
            if (success)
            {
                EmptyResponse response = JsonConvert.DeserializeObject<EmptyResponse>(result);
                onResult(NetworkState.NORMAL, response.code);
            }
            else
            {
                int errorCode = 0;
                int.TryParse(result, out errorCode);
                onResult(NetworkState.ERROR, errorCode);
            }
        }));
    }

    public void ChangeNickName(string nickname, Action<NetworkState, bool> onResult)
    {
        NickNameRequest param = new NickNameRequest(this.uid);
        param.nickname = nickname;

        this.StartCoroutine(RESTfulApi.Post(this.GetApiURL("users/nickname/save"), this.token, JsonConvert.SerializeObject(param), (success, result) =>
        {
            if (success)
            {
                NickNameResponse response = JsonConvert.DeserializeObject<NickNameResponse>(result);
                if (response.code == 200)
                    onResult(NetworkState.NORMAL, true);
                else
                    onResult(NetworkState.NORMAL, false);
            }
            else
            {
                onResult(NetworkState.ERROR, false);
            }
        }));
    }

    public void UpdateWeeklyRanking(string key, int score, RankInfo info, Action<NetworkState, WeeklyRankingInfo> onResult)
    {
        UpdateWeeklyRankingRequest param = new UpdateWeeklyRankingRequest(this.uid);
        param.key = key;
        param.score = score;
        param.update = (info == null) ? 0 : 1;
        param.data = info;

        this.StartCoroutine(RESTfulApi.Post(this.GetApiURL("rankings/weekly"), this.token, JsonConvert.SerializeObject(param), (success, result) =>
        {
            if (success)
            {
                WeeklyRankingResponse response = JsonConvert.DeserializeObject<WeeklyRankingResponse>(result);
                if (response.code == 200)
                    onResult(NetworkState.NORMAL, response.data);
                else
                    onResult(NetworkState.ERROR, null);
            }
            else
            {
                onResult(NetworkState.ERROR, null);
            }
        }));
    }

    public void GetWeeklyRanking(string key, Action<NetworkState, WeeklyRankingInfo> onResult)
    {
        SearchWeeklyRankingRequest param = new SearchWeeklyRankingRequest(this.uid);
        param.key = key;

        this.StartCoroutine(RESTfulApi.Post(this.GetApiURL("rankings/weekly/search"), this.token, JsonConvert.SerializeObject(param), (success, result) =>
        {
            if (success)
            {
                WeeklyRankingResponse response = JsonConvert.DeserializeObject<WeeklyRankingResponse>(result);
                if (response.code == 200)
                    onResult(NetworkState.NORMAL, response.data);
                else
                    onResult(NetworkState.ERROR, null);
            }
            else
            {
                onResult(NetworkState.ERROR, null);
            }
        }));
    }

    public void GetWeeklyNumber(Action<NetworkState, int> onResult)
    {
        RequestBase param = new RequestBase();

        this.StartCoroutine(RESTfulApi.Post(this.GetApiURL("rankings/weekly/number"), this.token, JsonConvert.SerializeObject(param), (success, result) =>
        {
            if (success)
            {
                WeekNumberResponse response = JsonConvert.DeserializeObject<WeekNumberResponse>(result);
                if (response.code == 200)
                    onResult(NetworkState.NORMAL, response.data.weekNumber);
                else
                    onResult(NetworkState.ERROR, 0);
            }
            else
            {
                onResult(NetworkState.ERROR, 0);
            }
        }));
    }

    public void ReportPayment(string product_id, string order_id, string currency_price, string currency_code, string receipt)
    {
        PaymentRequest param = new PaymentRequest(this.uid);
        param.product_id = product_id;
        param.order_id = order_id;
        param.currency_price = currency_price;
        param.currency_code = currency_code;
        param.receipt = receipt;

        this.StartCoroutine(RESTfulApi.Post(this.GetApiURL("payment"), this.token, JsonConvert.SerializeObject(param), (success, result) =>
        { }));
    }

    // public void BattleLog(BattleLogData data)
    // {
    //     BattleLogRequest param = new BattleLogRequest(this.uid);
    //     param.data = data;

    //     this.StartCoroutine(RESTfulApi.Post(this.GetApiURL("logs/battle"), this.token, JsonConvert.SerializeObject(param), (success, result) =>
    //     { }));
    // }

    public SaveUserRequest GetUserRequest(UserData data)
    {
        SaveUserRequest param = new SaveUserRequest(this.uid);
        param.category = "user";
        param.data = data;
        return param;
    }

    public SaveTimeRequest GetTimeRequest(TimeData data)
    {
        SaveTimeRequest param = new SaveTimeRequest(this.uid);
        param.category = "time";
        param.data = data;
        return param;
    }

    public SavePurchaseRequest GetPurchaseRequest(List<PurchaseProductData> data)
    {
        SavePurchaseRequest param = new SavePurchaseRequest(this.uid);
        param.category = "purchases";
        param.data = data;
        return param;
    }

    public SaveQuestRequest GetQuestRequest(List<QuestData> data)
    {
        SaveQuestRequest param = new SaveQuestRequest(this.uid);
        param.category = "quests";
        param.data = data;
        return param;
    }

    public SaveUpgradeRequest GetUpgradeRequest(List<UpgradeData> data)
    {
        SaveUpgradeRequest param = new SaveUpgradeRequest(this.uid);
        param.category = "upgrades";
        param.data = data;
        return param;
    }

    // 
    // public SaveSpeicalShopRequest GetSpecialShopRequest(SpecialShopData data)
    // {
    //     SaveSpeicalShopRequest param = new SaveSpeicalShopRequest(this.uid);
    //     param.category = "specialShop";
    //     param.data = data;
    //     return param;
    // }

    // public SaveWinRewardRequest GetWinRewardList(List<WinRewardData> data)
    // {
    //     SaveWinRewardRequest param = new SaveWinRewardRequest(this.uid);
    //     param.category = "winRewards";
    //     param.data = data;
    //     return param;
    // }

    // public GuideMissionRequest GetGuideMission(GuideMissionData data)
    // {
    //     GuideMissionRequest param = new GuideMissionRequest(this.uid);
    //     param.category = "guideMission";
    //     param.data = data;
    //     return param;
    // }

    public void CheckServer()
    {
        this.CheckServerVersion();
    }

    /////////////////////////////////////////////////////////////
    // protected

    protected void Start()
    {
        ObscuredCheatingDetector.StartDetection(this.OnCheaterDetected);
    }

    protected void OnCheaterDetected()
    {
        DataManager.Instance.UserData.isCheater = true;
        DataManager.Instance.SaveData(DataType.User);

        // string msg = LanguageManager.Instance.GetNeedLoginText();
        // string confirm = LanguageManager.Instance.GetConfirmText();
        // MessagePop.ShowPop(msg, confirm, () =>
        // {
        //     GameObject.Destroy(DontDestroyObject.Instance.gameObject);
        //     UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        // }, canClose: false);
    }

    protected void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            this.UpdateNetwork();
        }
        else
        {
            this.isTimeRefreshing = true;
            this.needCheckVersion = true;
            this.Save();
        }
    }

    protected void OnApplicationFocus(bool focusStatus)
    {
        if (Application.isEditor)
        {
            if (focusStatus)
            {
                this.UpdateNetwork();
            }
            else
            {
                this.isTimeRefreshing = true;
                this.needCheckVersion = true;
                this.Save();
            }
        }
    }

    /////////////////////////////////////////////////////////////
    // private

    private readonly string server_DEV = "https://ss.pg.cookapps.com/fantasy_idle/";
    private readonly string server_REAL = "https://ss.cookappsgames.com/fantasy_idle/";

    private readonly string cdn_DEV = "https://d19otl8pcfrvqp.cloudfront.net/fantasy-idle/dev/fantasy-idle-spec-{0}.json";
    private readonly string cdn_REAL = "https://d19otl8pcfrvqp.cloudfront.net/fantasy-idle/prod/fantasy-idle-spec-{0}.json";


    private string GetApiURL(string api)
    {
#if SERVER_REAL
        return server_REAL + api;
#else
        return server_DEV + api;
#endif
    }

    private string GetSpecDataUrl()
    {
#if SERVER_REAL
        return cdn_REAL;
#else
        return cdn_DEV;
#endif
    }

    private bool needCheckVersion = false;
    private bool isTimeRefreshing = false;

    private int retryCount = 0;

    private int uid;
    private string token;

    private void UpdateNetwork()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            GameManager.Instance.ShowMoveToMain("네트워크 연결을 확인하세요.");
        }
        else
        {
            this.UpdateServerTime(() =>
            {
                if (this.needCheckVersion)
                {
                    this.CheckServerVersion();
                }
            });
        }
    }

    private void CheckServerVersion()
    {
        NetworkManager.Instance.CheckVersion(null, (state, result) =>
         {
             if (state == NetworkState.NORMAL)
             {
                 int savedVersion = Preference.LoadPreference(Pref.SPEC_VERSION, 0);
                 if (result.status == 1)
                 {
                     GameManager.Instance.ShowMoveToMarket("새로운 버전으로 업데이트 하세요.");
                 }
                 else if (result.status == 5)
                 {
                     GameManager.Instance.ShowMoveToMain("서비스를 이용할 수 없습니다.");
                 }
                 else if (result.status == 3)
                 {
                     GameManager.Instance.ShowQuit("서버 점검 중 입니다.");
                 }
                 else if (savedVersion != result.spec_version)
                 {
                     this.Save();
                     GameManager.Instance.ShowMoveToMain("새로운 데이터를 적용하기위해 다시 시작합니다.");
                 }
             }
         });
    }

    private void Save()
    {
        if (DataManager.Instance.IsDataLoaded)
        {
            if (Application.isEditor)
            {
                // foreach (CardData data in DataManager.Instance.CardDatas)
                // {
                //     data.lv = 1;
                // }
                // DataManager.Instance.SetJewel(9999999);
                // DataManager.Instance.AddJewel(5000, CurrencyLocation.NONE);
                // DataManager.Instance.AddCoin(100000, CurrencyLocation.NONE);
                // DataManager.Instance.UserData.bTrophy = 5000;
                // DataManager.Instance.UserData.victory += 50;
                // if (LobbyUI.Instance != null) LobbyUI.Instance.Refresh();

                //     DataManager.Instance.SetJewel(100000);

                //     foreach (CardData data in DataManager.Instance.CardDatas)
                //     {
                //         if (data.MetaData.grade == 4)
                //             data.lv = 3;
                //         else if (data.MetaData.grade == 3)
                //             data.lv = 8;
                //         else if (data.MetaData.grade == 2)
                //             data.lv = 8;
                //         else if (data.MetaData.grade == 1)
                //             data.lv = 8;
                //     }
            }
            DataManager.Instance.SaveData(DataType.All);
            // if (GameUI.Instance != null) GameUI.Instance.Refresh();
        }
    }
}
