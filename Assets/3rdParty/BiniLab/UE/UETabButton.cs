/*********************************************
 * NHN StarFish - UI Extends
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[System.Serializable]
public class UETabEvent : UnityEvent<int> {}

public class UETabButton : MonoBehaviour, IPointerClickHandler
{

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// public

	//IPointerClickHandler
	public void OnPointerClick( PointerEventData eventData )
	{
		onTab.Invoke(this.index);
	}

	public void SetSelected(bool isSelected)
	{
		this.isSelected = isSelected;
		this.UpdateUI ();
	}

	public int Index { get { return this.index; } }
	public bool IsSelected { get { return this.isSelected; } }

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// Life Cycle

	void Start()
	{
		this.UpdateUI ();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// private

	[SerializeField] private GameObject enabledObj;
	[SerializeField] private GameObject disabledObj;

	[SerializeField] private UETabEvent onTab;

	[SerializeField] private int index;

	private bool isSelected = false;

	private void UpdateUI()
	{
		if(this.enabledObj) this.enabledObj.SetActive (this.isSelected);
		if(this.disabledObj) this.disabledObj.SetActive (!this.isSelected);
	}

}
