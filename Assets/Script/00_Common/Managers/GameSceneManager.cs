using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene
{
    Splash,
    Main,
    Lobby,
    InGame,
}


public class GameSceneManager : GameObjectSingleton<GameSceneManager>
{
    /////////////////////////////////////////////////////////////
    // public

    public static string CurrentScene_String { get => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; }
    public static Scene CurrentScene;

    public static void Restart()
    {
        GameObject.Destroy(DontDestroyObject.Instance.gameObject);
        Transition.LoadLevel(Scene.Splash.ToString(), 0.2f, Color.black);
    }

    public static void MoveScene(Scene scene, bool useTransition = true)
    {
        CurrentScene = scene;
        if (useTransition)
            Transition.LoadLevel(scene.ToString(), 0.2f, Color.black);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene.ToString());
    }

    public static void MoveScene(string sceneName, bool useTransition = true)
    {
        if (useTransition)
            Transition.LoadLevel(sceneName, 0.2f, Color.black);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

}

