using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureBuildingToFile : MonoBehaviour
{
    public IEnumerator capture()
    {

        yield return new WaitForEndOfFrame();
        //After Unity4,you have to do this function after WaitForEndOfFrame in Coroutine
        //Or you will get the error:"ReadPixels was called to read pixels from system frame buffer, while not inside drawing frame"
        for (int index = 0; index < this.transform.childCount; index++)
            this.transform.GetChild(index).gameObject.SetActive(false);

        for (int ry = 45; ry < 360; ry += 90)
        {
            this.transform.rotation = Quaternion.Euler(0.0f, (float)ry, 0.0f);

            for (int index = 0; index < this.transform.childCount; index++)
            {
                Transform xform;

                xform = this.transform.GetChild(index);
                xform.gameObject.SetActive(true);
                yield return new WaitForEndOfFrame();
                zzTransparencyCapture.captureScreenshot(Application.dataPath + "/../" + xform.name + "_" + ry.ToString() + ".png");
                xform.gameObject.SetActive(false);
            }
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            StartCoroutine(capture());
    }
}
