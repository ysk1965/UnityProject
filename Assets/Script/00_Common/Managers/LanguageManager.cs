using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum Language
{
    NONE = 0,
    EN = 1,
    KR = 2,
    TC = 3,
    SC = 4,
    JP = 5,
}

public class LanguageManager : GameObjectSingleton<LanguageManager>
{

    public Language Language { get => this.lan; }

    public string GetAIName()
    {
        return BiniLab.Utils.RandomPick(this.names);
    }

    public bool CanUseNick(string nick)
    {
        return !this.badNames.Exists(n => !string.IsNullOrEmpty(n) && (n.IndexOf(nick) >= 0 || nick.IndexOf(n) >= 0));
    }

    public Font GetFont()
    {
        switch (this.lan)
        {
            case Language.EN: return this.fontEN;
            case Language.KR: return this.fontKR;
            case Language.JP: return this.fontJP;
            case Language.TC: return this.fontJP;
            case Language.SC: return this.fontCN_SC;
        }
        return this.fontKR;
    }

    public string GetLanguageNameText(Language lan)
    {
        switch (this.lan)
        {
            case Language.EN: return "ENGLISH";
            case Language.KR: return "한국어";
            case Language.JP: return "日本語";
            case Language.TC: return "繁體中文";
            case Language.SC: return "简体中文";
        }

        return string.Empty;
    }

    public string GetText(LanguageID id)
    {
        try
        {
            LanguageMetaData data = SpecDataManager.Instance.GetLanguageData((int)id);
            switch (this.lan)
            {
                case Language.EN: return data.en;
                case Language.KR: return data.kr;
                case Language.JP: return data.jp;
                case Language.TC: return data.tc;
                case Language.SC: return data.sc;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e);
        }
        return string.Empty;
    }

    public void SetText(UnityEngine.UI.Text text, LanguageID id)
    {
        if (text.font != null)
        {
            if (text.font.name != this.GetFont().name)
                text.font = this.GetFont();
        }
        else
        {
            text.font = this.GetFont();
        }

        text.text = this.GetText(id);
    }

    public void SetText(UnityEngine.UI.Text uiText, string text)
    {
        try
        {
            if (uiText == null)
            {
                Debug.LogError("WARNINIG!!!!!!! uiText is null");
                return;
            }
            if (uiText.font != null)
            {
                if (uiText.font.name != this.GetFont().name)
                    uiText.font = this.GetFont();
            }
            else
            {
                uiText.font = this.GetFont();
            }

            uiText.text = text;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public string GetLocalizedMetaDataName(LocalizedMetaData data)
    {
        switch (this.lan)
        {
            case Language.EN: return data.name_en;
            case Language.KR: return data.name_kr;
            case Language.JP: return data.name_jp;
            case Language.TC: return data.name_tc;
            case Language.SC: return data.name_sc;
        }
        return string.Empty;
    }

    public string GetLocalizedMetaDataDesc(LocalizedMetaData data)
    {
        switch (this.lan)
        {
            case Language.EN: return data.desc_en;
            case Language.KR: return data.desc_kr;
            case Language.JP: return data.desc_jp;
            case Language.TC: return data.desc_tc;
            case Language.SC: return data.desc_sc;
        }
        return string.Empty;
    }

    public string GetSecText()
    {
        switch (this.lan)
        {
            case Language.EN: return "sec";
            case Language.KR: return "초";
            case Language.JP: return "秒";
            case Language.TC: return "秒";
            case Language.SC: return "秒";
        }
        return string.Empty;
    }

    public string GetConfirmText()
    {
        switch (this.lan)
        {
            case Language.EN: return "OKAY";
            case Language.KR: return "확인";
            case Language.JP: return "確認";
            case Language.TC: return "檢查一下";
            case Language.SC: return "检查一下";
        }
        return string.Empty;
    }

    public string GetContactUSText()
    {
        switch (this.lan)
        {
            case Language.EN: return "Contact us";
            case Language.KR: return "문의 하기";
            case Language.JP: return "お問い合わせ";
            case Language.TC: return "聯繫我們";
            case Language.SC: return "联系我们";
        }
        return string.Empty;
    }

    public string GetNeedUpdateText()
    {
        switch (this.lan)
        {
            case Language.EN: return "A new version has been released. Please update.";
            case Language.KR: return "새로운 버전이 출시되었습니다. 업데이트 해주세요.";
            case Language.JP: return "新しいバージョンがリリースされました。アップデートしてください。";
            case Language.TC: return "新版本已經發布。請更新。";
            case Language.SC: return "新版本已经发布。请更新。";
        }
        return string.Empty;
    }

    public string GetNeedNewVersionText()
    {
        switch (this.lan)
        {
            case Language.EN: return "Restart to apply the new data version.";
            case Language.KR: return "신규 데이터 패치를 위해 재시작 합니다.";
            case Language.JP: return "新しいバージョンのデータを適用するため、再起動します";
            case Language.TC: return "將重新開始以套用新版本的資料。";
            case Language.SC: return "为加载新版本的数据，即将重启游戏。";
        }
        return string.Empty;
    }

    public string GetNeedLoginText()
    {
        switch (this.lan)
        {
            case Language.EN: return "Reconnection is required.";
            case Language.KR: return "재접속이 필요합니다.";
            case Language.JP: return "再接続が必要です。";
            case Language.TC: return "需要重新連接。";
            case Language.SC: return "需要重新连接。";
        }
        return string.Empty;
    }

    public string GetAccountLockedText()
    {
        switch (this.lan)
        {
            case Language.EN: return "Your account has been suspended.";
            case Language.KR: return "계정이 정지되었습니다.";
            case Language.JP: return "アカウントが停止されました。";
            case Language.TC: return "您的帳戶已被暫停。";
            case Language.SC: return "您的帐户已被暂停。";
        }
        return string.Empty;
    }

    public string GetServerCheckText()
    {
        switch (this.lan)
        {
            case Language.EN: return "Sorry. The server is currently being checked.";
            case Language.KR: return "죄송합니다. 현재 서버 점검 중입니다.";
            case Language.JP: return "申し訳ありません。現在のサーバーメンテナンス中です。";
            case Language.TC: return "不好意思 當前正在檢查服務器。";
            case Language.SC: return "不好意思 当前正在检查服务器。";
        }
        return string.Empty;
    }

    public string GetRootingDetectText()
    {
        switch (this.lan)
        {
            case Language.EN: return "You cannot play on rooted devices.";
            case Language.KR: return "루팅된 기기에서는 플레이 할 수 없습니다.";
            case Language.JP: return "ルーティングされた機器では再生できません。";
            case Language.TC: return "您不能在有根設備上播放。";
            case Language.SC: return "您不能在有根设备上播放。";
        }
        return string.Empty;
    }

    public string GetCheatingDetectText()
    {
        switch (this.lan)
        {
            case Language.EN: return "Hacking attempt is detected.";
            case Language.KR: return "해킹 시도가 감지되었습니다.";
            case Language.JP: return "ハッキングの試みが検出されました。";
            case Language.TC: return "檢測到黑客嘗試。";
            case Language.SC: return "检测到黑客尝试。";
        }
        return string.Empty;
    }

    public string GetTimeCheatingDetectText()
    {
        switch (this.lan)
        {
            case Language.EN: return "Changing device time cheating attempt is detected.";
            case Language.KR: return "기기 시간 변경 시도가 감지되었습니다.";
            case Language.JP: return "機器の時間変化が検知されました。";
            case Language.TC: return "已檢測到嘗試更改設備時間的嘗試。";
            case Language.SC: return "检测到尝试更改设备时间。";
        }
        return string.Empty;
    }

    public string GetCheckingDataText()
    {
        switch (this.lan)
        {
            case Language.EN: return "Checking Data..";
            case Language.KR: return "데이터 체크중..";
            case Language.JP: return "データをチェックしています..";
            case Language.TC: return "正在檢查數據..";
            case Language.SC: return "正在检查数据..";
        }
        return string.Empty;
    }

    public string GetDownloadDataText()
    {
        switch (this.lan)
        {
            case Language.EN: return "Download Data...";
            case Language.KR: return "데이터 다운로드 중...";
            case Language.JP: return "データをダウンロード...";
            case Language.TC: return "下載數據...";
            case Language.SC: return "下载数据...";
        }
        return string.Empty;
    }

    public string GetLoginToServerText()
    {
        switch (this.lan)
        {
            case Language.EN: return "Login to Server....";
            case Language.KR: return "서버에 로그인 중....";
            case Language.JP: return "サーバーにログイン....";
            case Language.TC: return "登錄到服務器....";
            case Language.SC: return "登录到服务器....";
        }
        return string.Empty;
    }
    public string GetWelcomeText()
    {
        switch (this.lan)
        {
            case Language.EN: return "Welcome to Random Royale.";
            case Language.KR: return "랜덤로얄에 오신걸 환영합니다.";
            case Language.JP: return "ランダムロイヤルへようこそ。";
            case Language.TC: return "歡迎來到隨機皇家。";
            case Language.SC: return "欢迎来到随机皇家。";
        }
        return string.Empty;
    }

    public string GetTryAgainText()
    {
        switch (this.lan)
        {
            case Language.EN: return "Please try again.";
            case Language.KR: return "다시 시도해 주세요.";
            case Language.JP: return "もう一度やり直してください。";
            case Language.TC: return "請再試一遍。";
            case Language.SC: return "请再试一遍。";
        }
        return string.Empty;
    }

    public string GetGoogleLoginText()
    {
        switch (this.lan)
        {
            case Language.EN: return "Sign in with Google Play";
            case Language.KR: return "구글 로그인";
            case Language.JP: return "Google Playのアカウントでサインイン";
            case Language.TC: return "在與谷歌播放";
            case Language.SC: return "在与谷歌播放";
        }
        return string.Empty;
    }

    public string GetAppleLoginText()
    {
        switch (this.lan)
        {
            case Language.EN: return "Sign in with Apple";
            case Language.KR: return "애플 로그인";
            case Language.JP: return "アップルでサインイン";
            case Language.TC: return "登錄與蘋果";
            case Language.SC: return "登录与苹果";
        }
        return string.Empty;
    }

    public string GetGuestLoginText()
    {
        switch (this.lan)
        {
            case Language.EN: return "Play as Guest";
            case Language.KR: return "게스트 플레이";
            case Language.JP: return "ゲストとして遊ぶ";
            case Language.TC: return "扮演客戶";
            case Language.SC: return "扮演客户";
        }
        return string.Empty;
    }

    public string GetStartText()
    {
        switch (this.lan)
        {
            case Language.EN: return "START";
            case Language.KR: return "시작 하기";
            case Language.JP: return "開始";
            case Language.TC: return "開始";
            case Language.SC: return "开始";
        }
        return string.Empty;
    }

    public string GetGradeString(int grade)
    {
        switch (grade)
        {
            case 0: return this.GetText(LanguageID.COMMON_RANDOM);
            case 1: return this.GetText(LanguageID.GRADE_NORMAL);
            case 2: return this.GetText(LanguageID.GRADE_RARE);
            case 3: return this.GetText(LanguageID.GRADE_UNIQUE);
            case 4: return this.GetText(LanguageID.GRADE_LEGENDARY);
            default: return "NONE";
        }
    }

    public string GetRankingText(int ranking)
    {
        switch (this.lan)
        {
            case Language.EN:
                if (ranking == 1)
                    return "1st";
                else if (ranking == 2)
                    return "2nd";
                else if (ranking == 3)
                    return "3rd";
                else
                    return ranking + "th";

            case Language.KR: return ranking + "위";
            case Language.JP: return ranking + "位";
            case Language.TC: return "第" + ranking + "名";
            case Language.SC: return "第" + ranking + "名";
        }
        return string.Empty;
    }

    public Language GetRecommand()
    {
        if (Application.systemLanguage == SystemLanguage.Korean)
            return Language.KR;
        if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
            return Language.TC;
        if (Application.systemLanguage == SystemLanguage.ChineseSimplified)
            return Language.SC;
        if (Application.systemLanguage == SystemLanguage.Japanese)
            return Language.JP;

        return Language.EN;
    }

    protected override void Awake()
    {
        base.Awake();
        this.lan = (Language)Preference.LoadPreference(Pref.LANGUAGE, (int)this.GetRecommand());
        this.LoadNames();
        this.LoadBadNames();
    }

    protected IEnumerator Start()
    {
        yield return new WaitUntil(() => DataManager.Instance.IsDataLoaded);
        this.lan = (Language)Enum.Parse(typeof(Language), DataManager.Instance.UserData.lan);
        Preference.SavePreference(Pref.LANGUAGE, (int)this.lan);
    }

    [SerializeField] private Font fontKR;
    [SerializeField] private Font fontEN;
    [SerializeField] private Font fontJP;
    [SerializeField] private Font fontCN_SC;

    [SerializeField] private TextAsset nameEnAsset;
    [SerializeField] private TextAsset badEnAsset;
    private List<string> names;
    private List<string> badNames;

    private Language lan = Language.EN;

    private void LoadNames()
    {
        // StringReader sr = new StringReader(this.nameEnAsset.text);

        // this.names = new List<string>();
        // string line = sr.ReadLine();
        // while (!string.IsNullOrEmpty(line))
        // {
        //     line = sr.ReadLine();
        //     this.names.Add(line);
        // }
    }

    private void LoadBadNames()
    {
        // if (this.badNames == null)
        // {
        //     StringReader sr = new StringReader(this.badEnAsset.text);

        //     this.badNames = new List<string>();
        //     string line = sr.ReadLine();
        //     while (!string.IsNullOrEmpty(line))
        //     {
        //         line = sr.ReadLine();
        //         this.badNames.Add(line);
        //     }
        // }
    }
}
