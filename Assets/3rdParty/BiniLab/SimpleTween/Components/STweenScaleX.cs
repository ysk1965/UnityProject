/*********************************************
 * NHN StarFish - Simple Tween
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class STweenScaleX : STweenBase<float> {

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// public

	public override void Restore ()
	{
		base.Restore ();
		Vector3 cur = this.transform.localScale;
		this.transform.localScale = new Vector3(this.start, cur.y, cur.z);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// overrides STweenBase 

	protected override void PlayTween ()
	{
		base.PlayTween ();

		base.tweenValue = this.tweener.CreateTween (this.start, this.end);
	}

	protected override void UpdateValue (float value)
	{
		Vector3 cur = this.transform.localScale;
		this.transform.localScale = new Vector3(value, cur.y, cur.z);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	// public 

	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	// private 

}
