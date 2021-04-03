/*********************************************
 * NHN StarFish - Simple Tween
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class STweenRotate : STweenBase<Vector3> {

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// public

	public override void Restore ()
	{
		base.Restore ();
		this.transform.localRotation = Quaternion.Euler (this.start);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// overrides STweenBase 

	protected override void PlayTween ()
	{
		this.transform.localRotation = Quaternion.Euler (this.start);
		base.PlayTween ();

		base.tweenValue = this.tweener.CreateTween (this.start, this.end);
	}

	protected override void UpdateValue (Vector3 value)
	{
		base.UpdateValue (value);
		this.transform.localRotation = Quaternion.Euler (value);

//		Vector3 rotateValue = this.lastValue - value;
//		this.transform.Rotate(rotateValue);
//		this.lastValue = value;
	}

}
