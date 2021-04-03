/*********************************************
 * NHN StarFish - UI Extends
 * CHOI YOONBIN
 * 
 *********************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// The direction we are snapping in
public enum SnapDirection
{
	Horizontal,
	Vertical,
}

public class UECenterOnChild : UIBehaviour, IEndDragHandler, IBeginDragHandler
{

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// public

	public delegate void OnChange(int current, int last);

	public ScrollRect scrollRect; // the scroll rect to scroll
	public SnapDirection direction; // the direction we are scrolling

	public Transform itemContainer;
	public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f); // a curve for transitioning in order to give it a little bit of extra polish
	public float speed; // the speed in which we snap ( normalized position per second? )
	public OnChange onChange;
	public UEIndicator indicator;

	public void Init()
	{
		this.minDragDistance = Screen.width / 4f;

		if (scrollRect == null) // if we are resetting or attaching our script, try and find a scroll rect for convenience 
			scrollRect = GetComponent<ScrollRect>();
		
		if (this.itemContainer != null)
			this.itemCount = this.GetItemCount ();

		if (this.indicator != null)
			this.indicator.Init (this);
	}
	
	public void OnBeginDrag(PointerEventData eventData)
	{
		StopCoroutine(SnapRect()); // if we are snapping, stop for the next input

		this.dragStartPoint = this.direction == SnapDirection.Horizontal ? eventData.position.x : eventData.position.y;
	}
	
	public void OnEndDrag(PointerEventData eventData)
	{
//		StartCoroutine(SnapRect()); // simply start our coroutine ( better than using update )
//

		float dragEndPoint = this.direction == SnapDirection.Horizontal ? eventData.position.x : eventData.position.y;
		float dragDistance = this.dragStartPoint - dragEndPoint;
		if(Mathf.Abs(dragDistance) >= this.minDragDistance)
		{
			if(dragDistance > 0) this.MoveNext();
			else this.MovePrevious();
		}
		else
		{
			StartCoroutine(SnapRect());
		}

	}

	public void MoveNext()
	{
		if (this.currentTarget >= (this.itemCount-1))
			return;

		StartCoroutine(SnapRect(this.currentTarget + 1));
	}

	public void MovePrevious()
	{
		if (this.currentTarget == 0)
			return;

		StartCoroutine(SnapRect(this.currentTarget - 1 ));
	}

	public int GetItemCount()
	{
		return this.itemContainer.childCount;
	}


	////////////////////////////////////////////////////////////////////////////////////////////////////
	// private

	private int currentTarget = 0;
	private int lastTarget = 0;
	private float dragStartPoint = 0f;
	public float minDragDistance = 100f; // the minimum distance for scroll to next
	private int itemCount; // how many items we have in our scroll rect

	private IEnumerator SnapRect(int target = -1)
	{
		if (scrollRect == null)
			throw new System.Exception("Scroll Rect can not be null");
		if (itemCount == 0)
			throw new System.Exception("Item count can not be zero");
		
		float startNormal = direction == SnapDirection.Horizontal ? scrollRect.horizontalNormalizedPosition : scrollRect.verticalNormalizedPosition; // find our start position
		float delta = 1f / (float)(itemCount - 1); // percentage each item takes
		if(target < 0) target = Mathf.Max(0, Mathf.RoundToInt(startNormal / delta)); // this finds us the closest target based on our starting point
		if(target != this.currentTarget)
		{
			this.lastTarget = this.currentTarget;
			this.currentTarget = target;
			if(this.onChange != null) this.onChange(this.currentTarget, this.lastTarget);
			Debug.Log("this.currentTarget " + this.currentTarget );
		}

		float endNormal = delta * target; // this finds the normalized value of our target
		float duration = Mathf.Abs((endNormal - startNormal) / speed); // this calculates the time it takes based on our speed to get to our target
		
		float timer = 0f; // timer value of course
		while (timer < 1f) // loop until we are done
		{
			timer = Mathf.Min(1f, timer + Time.deltaTime / duration); // calculate our timer based on our speed
			float value = Mathf.Lerp(startNormal, endNormal, curve.Evaluate(timer)); // our value based on our animation curve, cause linear is lame
			
			if (direction == SnapDirection.Horizontal) // depending on direction we set our horizontal or vertical position
				scrollRect.horizontalNormalizedPosition = value;
			else
				scrollRect.verticalNormalizedPosition = value;
			
			yield return new WaitForEndOfFrame(); // wait until next frame
		}
        yield break;
	}
}

