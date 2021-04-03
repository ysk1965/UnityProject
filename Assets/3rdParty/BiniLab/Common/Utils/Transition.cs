using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// This class is responsible for managing the transitions between scenes that are performed
// in the demo via a classic fade.
public class Transition : MonoBehaviour
{

    ////////////////////////////////////////////////////////////////////////////
    // public

    public static void LoadLevel(string level, float duration, Color color)
    {
        var fade = new GameObject("Transition");
        fade.AddComponent<Transition>();
        fade.GetComponent<Transition>().StartFade(level, duration, color);
        fade.transform.SetParent(canvas.transform, false);
        fade.transform.SetAsLastSibling();
    }


    ////////////////////////////////////////////////////////////////////////////
    // protected

    protected void Awake()
    {
        // Create a new, ad-hoc canvas that is not destroyed after loading the new scene
        // to more easily handle the fading code.
        Transition.canvas = new GameObject("TransitionCanvas");
        var canvas = Transition.canvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        DontDestroyOnLoad(Transition.canvas);
    }

    ////////////////////////////////////////////////////////////////////////////
    // private

    private static GameObject canvas;

    private GameObject overlay;

    private void StartFade(string level, float duration, Color fadeColor)
    {
        StartCoroutine(RunFade(level, duration, fadeColor));
    }

    // This coroutine performs the core work of fading out of the current scene
    // and into the new scene.
    private IEnumerator RunFade(string level, float duration, Color fadeColor)
    {
        var bgTex = new Texture2D(1, 1);
        bgTex.SetPixel(0, 0, fadeColor);
        bgTex.Apply();

        overlay = new GameObject();
        var image = overlay.AddComponent<Image>();
        var rect = new Rect(0, 0, bgTex.width, bgTex.height);
        var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
        image.material.mainTexture = bgTex;
        image.sprite = sprite;
        var newColor = image.color;
        image.color = newColor;
        image.canvasRenderer.SetAlpha(0.0f);

        overlay.transform.localScale = new Vector3(1, 1, 1);
        overlay.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
        overlay.transform.SetParent(canvas.transform, false);
        overlay.transform.SetAsFirstSibling();

        var time = 0.0f;
        var halfDuration = duration / 2.0f;
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            image.canvasRenderer.SetAlpha(Mathf.InverseLerp(0, 1, time / halfDuration));
            yield return new WaitForEndOfFrame();
        }

        image.canvasRenderer.SetAlpha(1.0f);
        yield return new WaitForEndOfFrame();

        UnityEngine.SceneManagement.SceneManager.LoadScene(level);

        time = 0.0f;
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            image.canvasRenderer.SetAlpha(Mathf.InverseLerp(1, 0, time / halfDuration));
            yield return new WaitForEndOfFrame();
        }

        image.canvasRenderer.SetAlpha(0.0f);
        yield return new WaitForEndOfFrame();

        Destroy(canvas);
    }
}