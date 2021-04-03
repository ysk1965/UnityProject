/*********************************************
 * NHN StarFish - UI Extends
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using System.Collections;

public class UEIndicatorItem : MonoBehaviour {

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// public 

	public void OnOff(bool on)
	{
		this.onImage.Begin (on ? 0f : 1f, on ? 1f : 0f);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// private 

	[SerializeField]
	private STweenAlpha onImage;
}
