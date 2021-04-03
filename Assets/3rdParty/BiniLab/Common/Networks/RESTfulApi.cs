using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using UnityEngine.Networking;

public class RESTfulApi
{
    public static IEnumerator Get(string url, string token, UnityAction<bool, string> callback)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            callback(false, null);
            yield break;
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            if (token != null) webRequest.SetRequestHeader("Authorization", "Bearer " + token);

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Error url : " + url + " error : " + webRequest.error + " code : " + webRequest.responseCode);
                callback(false, webRequest.responseCode.ToString());
            }
            else
            {
                Debug.Log("Get Response: <color=yellow>" + webRequest.downloadHandler.text + "</color>");
                callback(true, webRequest.downloadHandler.text);
            }
        }
    }

    public static IEnumerator Post(string url, string token, string paramsJson, UnityAction<bool, string> callback)
    {
        Debug.Log("<color=yellow>Post: " + url + "</color> params: " + paramsJson);
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            callback(false, null);
            yield break;
        }

        using (UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
        {
            webRequest.url = url;

            byte[] myData = System.Text.Encoding.UTF8.GetBytes(paramsJson);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(myData);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            if (token != null)
                webRequest.SetRequestHeader("Authorization", "Bearer " + token);
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Error: " + webRequest.error + " " + webRequest.responseCode + " " + webRequest.url);
                if (callback != null)
                    callback(false, webRequest.responseCode.ToString());
            }
            else
            {
                Debug.Log(":Received: " + webRequest.downloadHandler.text);
                if (callback != null)
                    callback(true, webRequest.downloadHandler.text);
            }
        }
    }

    public static IEnumerator Put(string url, string token, string paramsJson, UnityAction<bool, string> callback)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            callback(false, null);
            yield break;
        }

        using (UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPUT))
        {
            byte[] myData = System.Text.Encoding.UTF8.GetBytes(paramsJson);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(myData);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            if (token != null)
                webRequest.SetRequestHeader("Authorization", "Bearer " + token);
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Error: " + webRequest.error + " " + webRequest.responseCode);
                if (callback != null)
                    callback(false, webRequest.responseCode.ToString());
            }
            else
            {
                Debug.Log(":Received: " + webRequest.downloadHandler.text);
                if (callback != null)
                    callback(true, webRequest.downloadHandler.text);
            }
        }
    }

    public static IEnumerator Delete(string url, string token, string paramsJson, UnityAction<bool, string> callback)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            callback(false, null);
            yield break;
        }

        using (UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbDELETE))
        {
            byte[] myData = System.Text.Encoding.UTF8.GetBytes(paramsJson);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(myData);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            if (token != null)
                webRequest.SetRequestHeader("Authorization", "Bearer " + token);
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Error: " + webRequest.error + " " + webRequest.responseCode);
                if (callback != null)
                    callback(false, webRequest.responseCode.ToString());
            }
            else
            {
                Debug.Log(":Received: " + webRequest.downloadHandler.text);
                if (callback != null)
                    callback(true, webRequest.downloadHandler.text);
            }
        }
    }
}