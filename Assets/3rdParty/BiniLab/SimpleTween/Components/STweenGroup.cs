/*********************************************
 * NHN StarFish - Simple Tween
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using System.Collections;

public class STweenGroup : STween {

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// public

	public override void Begin ()
	{
		base.Begin ();
		foreach (var stween in stweens)
			stween.Begin ();
	}

	public override void Restore ()
	{
		base.Restore ();
		foreach (var stween in stweens)
			stween.Restore ();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// private

	[SerializeField] private STween[] stweens;
}
