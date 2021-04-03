using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class CoroutineHelper : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////
    // public

    public static CoroutineHelper Instance { get => instance; }

    public int RunCount { get => runs.Count; }

    public Run Add(Run run)
    {
        if (run != null)
            this.runs.Add(run);
        return run;
    }

    /////////////////////////////////////////////////////////////////
    // protected

    protected void Awake()
    {
        instance = this;
    }

    protected void Update()
    {
        for (int i = this.runs.Count - 1; i >= 0; i--)
        {
            if (this.runs[i].IsDone)
                this.runs.RemoveAt(i);
        }
    }

    /////////////////////////////////////////////////////////////////
    // private
    private static CoroutineHelper instance;

    private List<Run> runs = new List<Run>();

}

public class Run
{
    public string calleeName;

    public bool IsDone { get => this.isDone; }

    public Coroutine WaitFor { get => CoroutineHelper.Instance.StartCoroutine(this.WaitUntilDone(null)); }

    public Run()
    {
    }

    public Run(string name)
    {
        this.calleeName = name;
    }

    public static Run EachFrame(UnityAction action)
    {
        var tmp = new Run();
        tmp.action = RunEachFrame(tmp, action);
        tmp.Start();
        return tmp;
    }

    public static Run Every(float aInitialDelay, float aDelay, UnityAction action)
    {
        var tmp = new Run();
        tmp.action = RunEvery(tmp, aInitialDelay, aDelay, 0, action);
        tmp.Start();
        return tmp;
    }

    public static Run Every(float aInitialDelay, float aDelay, int count, UnityAction action)
    {
        var tmp = new Run();
        tmp.action = RunEvery(tmp, aInitialDelay, aDelay, count, action);
        tmp.Start();
        return tmp;
    }

    /**
    WaitForSeconds 을 사용합니다.
    Update() 다음 호출 됩니다.
     */
    public static Run After(float aDelay, UnityAction action)
    {
        var tmp = new Run();
        tmp.action = RunAfter(tmp, aDelay, action);
        tmp.Start();
        return tmp;
    }

    /**
    WaitForSecondsRealtime 을 사용합니다.
    Update() 다음 호출 됩니다.
    */
    public static Run AfterRealtime(float aDelay, UnityAction action)
    {
        var tmp = new Run();
        tmp.action = RunAfterRealtime(tmp, aDelay, action);
        tmp.Start();
        return tmp;
    }

    /**
        WaitForSecondsRealtime 을 사용합니다.
        Update() 다음 호출 됩니다.
        */
    public static Run AfterRealtime(float aDelay, UnityAction action, string calleeName)
    {
        var tmp = new Run(calleeName);
        tmp.action = RunAfterRealtime(tmp, aDelay, action);
        tmp.Start();
        return tmp;
    }


    public static Run AfterCoroutine(IEnumerator coroutine, UnityAction action)
    {
        var tmp = new Run();
        tmp.action = RunAfterCoroutine(tmp, coroutine, action);
        tmp.Start();
        return tmp;
    }

    /**
    WaitForEndOfFrame 을 사용합니다.
    LateUpdate() 다음 호출 됩니다.
     */
    public static Run NextFrame(UnityAction action)
    {
        var tmp = new Run();
        tmp.action = RunNextFrame(tmp, action);
        tmp.Start();
        return tmp;
    }

    /**
    yield return null 을 사용합니다.
    Update() 다음 호출 됩니다.
    */
    public static Run NextUpdate(UnityAction action)
    {
        var tmp = new Run();
        tmp.action = RunNextFrame(tmp, action);
        tmp.Start();
        return tmp;
    }

    /**
    WaitForFixedUpdate 을 사용합니다.
    FixedUpdate() 다음 호출 됩니다.
    */
    public static Run NextFixedUpdate(UnityAction action)
    {
        var tmp = new Run();
        tmp.action = RunNextFixedUpdate(tmp, action);
        tmp.Start();
        return tmp;
    }


    public static Run Lerp(float duration, UnityAction<float> action)
    {
        var tmp = new Run();
        tmp.action = RunLerp(tmp, duration, action);
        tmp.Start();
        return tmp;
    }


    public static Run Wait(Func<bool> predict, UnityAction action)
    {
        var tmp = new Run();
        tmp.action = RunWait(tmp, predict, action);
        tmp.Start();
        return tmp;
    }

    public static Run Coroutine(IEnumerator coroutine)
    {
        var tmp = new Run();
        tmp.action = RunCoroutine(tmp, coroutine);
        tmp.Start();
        return tmp;
    }


    public Run ExecuteWhenDone(UnityAction action)
    {
        var tmp = new Run();
        tmp.action = this.WaitUntilDone(action);
        tmp.Start();
        return tmp;
    }

    public void Abort()
    {
        this.abort = true;
    }

    /////////////////////////////////////////////////////////////////
    // protected

    protected void Start()
    {
        if (this.action != null)
            CoroutineHelper.Instance.StartCoroutine(this.action);
    }


    /////////////////////////////////////////////////////////////////
    // private

    private bool isDone;
    private bool abort;

    private IEnumerator action;

    private IEnumerator WaitUntilDone(UnityAction onDone)
    {
        while (!this.isDone)
            yield return null;
        if (onDone != null)
            onDone();
    }

    private static IEnumerator RunEachFrame(Run run, UnityAction action)
    {
        run.isDone = false;
        while (true)
        {
            if (!run.abort && action != null)
                action();
            else
                break;
            yield return null;
        }
        run.isDone = true;
    }

    private static IEnumerator RunEvery(Run run, float aInitialDelay, float aSeconds, int count, UnityAction action)
    {
        run.isDone = false;
        if (aInitialDelay > 0f)
            yield return new WaitForSeconds(aInitialDelay);
        else
        {
            int FrameCount = Mathf.RoundToInt(-aInitialDelay);
            for (int i = 0; i < FrameCount; i++)
                yield return null;
        }

        int repeat = (count == 0) ? int.MaxValue : count;
        while (repeat > 0)
        {
            if (!run.abort && action != null)
                action();
            else
                break;
            if (aSeconds > 0)
                yield return new WaitForSeconds(aSeconds);
            else
            {
                int FrameCount = Mathf.Max(1, Mathf.RoundToInt(-aSeconds));
                for (int i = 0; i < FrameCount; i++)
                    yield return null;
            }
            repeat--;
        }
        run.isDone = true;
    }

    private static IEnumerator RunAfter(Run run, float aDelay, UnityAction action)
    {
        run.isDone = false;
        yield return new WaitForSeconds(aDelay);
        if (!run.abort && action != null)
            action();
        run.isDone = true;
    }

    private static IEnumerator RunAfterRealtime(Run run, float aDelay, UnityAction action)
    {
        run.isDone = false;
        yield return new WaitForSecondsRealtime(aDelay);
        if (!run.abort && action != null)
            action();
        run.isDone = true;
    }

    private static IEnumerator RunAfterCoroutine(Run run, IEnumerator coroutine, UnityAction action)
    {
        run.isDone = false;
        yield return CoroutineHelper.Instance.StartCoroutine(coroutine);
        if (!run.abort && action != null)
            action();
        run.isDone = true;
    }

    private static IEnumerator RunNextFrame(Run run, UnityAction action)
    {
        run.isDone = false;
        yield return new WaitForEndOfFrame();
        if (!run.abort && action != null)
            action();
        run.isDone = true;
    }

    private static IEnumerator RunNextFixedUpdate(Run run, UnityAction action)
    {
        run.isDone = false;
        yield return new WaitForFixedUpdate();
        if (!run.abort && action != null)
            action();
        run.isDone = true;
    }

    private static IEnumerator RunNextUpdate(Run run, UnityAction action)
    {
        run.isDone = false;
        yield return null;
        if (!run.abort && action != null)
            action();
        run.isDone = true;
    }

    private static IEnumerator RunLerp(Run run, float aDuration, UnityAction<float> action)
    {
        run.isDone = false;
        float t = 0f;
        while (t < 1.0f)
        {
            t = Mathf.Clamp01(t + Time.deltaTime / aDuration);
            if (!run.abort && action != null)
                action(t);
            yield return null;
        }
        run.isDone = true;
    }

    private static IEnumerator RunWait(Run run, Func<bool> predict, UnityAction action)
    {
        run.isDone = false;
        yield return new WaitUntil(predict);
        if (!run.abort && action != null)
            action();
        run.isDone = true;
    }

    private static IEnumerator RunCoroutine(Run run, IEnumerator coroutine)
    {
        run.isDone = false;
        yield return CoroutineHelper.Instance.StartCoroutine(coroutine);
        run.isDone = true;
    }

}