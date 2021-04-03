/*********************************************
 * NHN StarFish - UI Extends
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class UEIndicator : MonoBehaviour {
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	// public

	public GameObject indicatorItemPrefab;

	public void Init(UECenterOnChild centerOnChild)
	{
		centerOnChild.onChange = this.OnChange;
		
		int pageCount = centerOnChild.GetItemCount ();
		this.items = new List<UEIndicatorItem> ();
		for(int i = 0; i < pageCount; i++)
		{
			GameObject go = GameObject.Instantiate(indicatorItemPrefab) as GameObject;
			go.GetComponent<RectTransform>().SetParent(this.transform);
			go.GetComponent<RectTransform>().localScale = Vector3.one;
			UEIndicatorItem item = go.GetComponent<UEIndicatorItem>();
			item.OnOff(i == 0);
			this.items.Add(item);
		}

	}

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// private

	private List<UEIndicatorItem> items;

	private void OnChange(int cur, int last)
	{
		this.items [cur].OnOff (true);
		this.items [last].OnOff (false);
	}
}
