using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenShot : MonoBehaviour {

//	// Use this for initialization
//	void Start () {
//		RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 32); 
//		Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
//
//	void OnGUI()
//	{
//		if (GUI.Button (new Rect (20, 20, 100, 100), "SCREEN SHOT"))
//			this.TakeScreenShot ();
//	}

	public void LookAt(Transform tf)
	{
		Camera.main.transform.LookAt (tf);
	}

	public IEnumerator TakeScreenShot()
	{
		string dirPath = Application.dataPath + "/../capture/";
		if(!Directory.Exists(dirPath))
			Directory.CreateDirectory(dirPath);
		yield return new WaitForEndOfFrame();
		zzTransparencyCapture.captureScreenshot (dirPath + Time.realtimeSinceStartup.ToString() + ".png");
		yield return new WaitForEndOfFrame();
	}

	public IEnumerator capture()
	{
		string dirPath = Application.dataPath + "/../capture/";
		if(!Directory.Exists(dirPath))
			Directory.CreateDirectory(dirPath);

		int childCount = this.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			this.transform.GetChild (i).gameObject.SetActive (false);
		}

		for (int i = 0; i < childCount; i++)
		{
			this.transform.GetChild (i).gameObject.SetActive (true);
			yield return new WaitForEndOfFrame();

			zzTransparencyCapture.captureScreenshot (dirPath + this.transform.GetChild(i).name + ".png");
				
			yield return new WaitForEndOfFrame();
			this.transform.GetChild (i).gameObject.SetActive (false);

			Debug.Log ("Capture Done : " + dirPath + i);
		}
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
			StartCoroutine(capture());
		if (Input.GetKeyDown (KeyCode.S))
			StartCoroutine(TakeScreenShot ());
	}
}
