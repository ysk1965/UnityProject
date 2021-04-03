using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPop : UEPopup {

	public static void ShowPopup()
	{
		if(instance == null)
		{
			instance = UEPopup.GetInstantiateComponent<LoadingPop>("Prefabs/UI/Popups/LoadingPop");
			instance.Show();
		}
	}

	public static void HidePopup()
	{
		if(instance != null)
		{
			instance.Hide();
			instance = null;
		}
	}

	public void Show()
	{
		base.Show(0.2f, false);
	}

	public void Hide()
	{
		base.Hide(0.2f, false);
	}

	private static new LoadingPop instance;
}
