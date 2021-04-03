/*********************************************
 * NHN StarFish - UI Extends
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using System.Collections;

public class UEPopupBackground : MonoBehaviour {

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// overrides MonoBehaviour 

	void Update () {
		
		if(!this.alphaTweener.Completed)
		{
			this.alphaTweener.Update(Time.unscaledDeltaTime);
			this.canvasGroup.alpha = this.alphaTweenValue.Value;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	// public

	public void Show(float tweenDuration)
	{
		this.canvasGroup = this.GetComponent<CanvasGroup> ();
		this.canvasGroup.alpha = 0f;
		this.gameObject.SetActive (true);
		this.alphaTweener.Reset (tweenDuration / 2f, EasingObject.LinearEasing, this.OnCompleteShow);
		this.alphaTweenValue = this.alphaTweener.CreateTween (0f, this.alphaValue);
	}
	
	public void Hide(float tweenDuration)
	{
		this.alphaTweener.Reset (tweenDuration / 2f, EasingObject.LinearEasing, this.OnCompleteHide);
		this.alphaTweenValue = this.alphaTweener.CreateTween (this.alphaValue, 0f);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	// private
	
	[SerializeField] private float alphaValue = 0.8f;

	private SimpleTweenerEx alphaTweener = new SimpleTweenerEx();
	private TweenLerp<float> alphaTweenValue;
	private CanvasGroup canvasGroup;

	private void OnCompleteShow(object[] onCompleteParms)
	{
		this.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		this.GetComponent<CanvasGroup> ().interactable = true;
	}

	private void OnCompleteHide(object[] onCompleteParms)
	{
		this.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		this.GetComponent<CanvasGroup> ().interactable = false;
	}
}
