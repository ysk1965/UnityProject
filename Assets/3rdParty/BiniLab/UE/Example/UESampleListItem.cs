using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UESampleListItem : UEListComponent {

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// public

	public string ItemName
	{
		set
		{
			this.itemName = value;
			this.SetData();
		}

		get
		{
			return this.itemName;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// private

	private string itemName;

	private void SetData()
	{
		this.GetComponentInChildren<Text> ().text = this.itemName;
	}

}
