/*********************************************
 * NHN StarFish - UI Extends
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UEContinuativeButton : Button {
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	// overrides MonoBehaviour 

	
	// Update is called once per frame
	void Update () {

		if(onPressed)
		{
			if(this.clickRate == this.curClickRate)
			{
				this.onClick.Invoke ();
				this.clickRate = Mathf.Max(this.clickRate - 2f, MIN_CLICK_RATE);
				this.curClickRate = 0f;
			}
			else
			{
				this.curClickRate += 1f;
			}
		}
	}

	protected override	void OnDisable()
	{
		this.onPressed = false;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	// public overrides Button

	public override void OnPointerDown (UnityEngine.EventSystems.PointerEventData eventData)
	{
		base.OnPointerDown (eventData);
		this.clickRate = MAX_CLICK_RATE;
		this.curClickRate = MAX_CLICK_RATE;
		this.onPressed = true;
	}

	public override void OnPointerUp (UnityEngine.EventSystems.PointerEventData eventData)
	{
		this.onPressed = false;
	}

	public override void OnPointerExit (UnityEngine.EventSystems.PointerEventData eventData)
	{
		this.onPressed = false;
	}

	public override void OnPointerClick (UnityEngine.EventSystems.PointerEventData eventData)
	{
		//Do nothing
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// private

	public static float MAX_CLICK_RATE = 12f;
	public static float MIN_CLICK_RATE = 1f;

	private bool onPressed = false;
	private float clickRate = MAX_CLICK_RATE;
	private float curClickRate = MAX_CLICK_RATE;
}
