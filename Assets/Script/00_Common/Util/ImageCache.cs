using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public enum ImageType
{
    COMMON, TIP,
}

public class ImageCache
{
    public static void LoadSprite(ImageType type, string imageURL, UnityAction<Sprite, string> onLoadComplete)
    {
        Texture2D texture2D = LoadTexture2DFromFile(type, imageURL);
        if (texture2D != null)
        {
            onLoadComplete(Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f)), imageURL);
            return;
        }

        Run.Coroutine(LoadTexture2DFromWEB(imageURL, (result) =>
        {
            if (result != null)
            {
                SaveTexture(result, type, imageURL);
                onLoadComplete(Sprite.Create(result, new Rect(0, 0, result.width, result.height), new Vector2(0.5f, 0.5f)), imageURL);
            }
            else
            {
                onLoadComplete(null, imageURL);
            }
        }));
    }

    private static Texture2D LoadTexture2DFromFile(ImageType type, string imageURL)
    {
        string fileName = GetFileNameFromURL(imageURL);

        //디렉 존재 토리 체크 후 없으면 null 반환
        string directoryPath = GetDirectoryPathByType(type);
        if (!Directory.Exists(directoryPath))
        {
            Debug.LogError("Path is not exists " + directoryPath);
            return null;
        }

        //파일 존재 체크 후 없으면 null 반환
        string filePath = GetFileFullPath(directoryPath, fileName);
        if (!System.IO.File.Exists(filePath))
        {
            Debug.LogError("File is not exists " + filePath);
            return null;
        }

        byte[] imageData = LoadImageDataFromFile(filePath);
        if (imageData != null)
        {
            Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            texture2D.LoadImage(imageData);
            return texture2D;
        }
        return null;
    }

    private static void SaveTexture(Texture2D texture, ImageType type, string imageURL)
    {
        //디렉 존재 토리 체크 후 없으면 생성
        string directoryPath = GetDirectoryPathByType(type);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string filePath = GetFileFullPath(directoryPath, GetFileNameFromURL(imageURL));

        using (FileStream file = File.Open(filePath, FileMode.OpenOrCreate))
        {
            byte[] imageData = texture.EncodeToPNG();
            BinaryWriter writer = new BinaryWriter(file);
            writer.Write(imageData);
            writer.Close();
            file.Close();
        }
    }

    private static IEnumerator LoadTexture2DFromWEB(string imageURL, UnityAction<Texture2D> onLoadComplete)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageURL);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
            onLoadComplete(null);
        }
        else
        {
            onLoadComplete(((DownloadHandlerTexture)www.downloadHandler).texture);
        }
    }

    private static byte[] LoadImageDataFromFile(string filePath)
    {
        try
        {
            return File.ReadAllBytes(filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
            return null;
        }
    }

    private static string GetFileNameFromURL(string imageURL)
    {
        int sIndex = imageURL.LastIndexOf("/");
        return imageURL.Substring(sIndex + 1);
    }

    private static string GetDirectoryPathByType(ImageType type)
    {
        return Application.persistentDataPath + "/" + type.ToString();
    }

    private static string GetFileFullPath(string directory, string fileName)
    {
        return directory + "/" + fileName;
    }

}
