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
public class UESwitchEvent : UnityEvent<bool> {}

public class UESwitchButton : MonoBehaviour, IPointerClickHandler
{

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// public

	//IPointerClickHandler
	public void OnPointerClick( PointerEventData eventData )
	{
		this.isOn = !this.isOn;
		onSwitch.Invoke(this.isOn);
		this.UpdateUI ();
	}

	public void SetOnOff(bool onOff)
	{
		this.isOn = onOff;
		this.UpdateUI ();
	}

	public bool IsOn { get { return this.isOn; } }

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

	[SerializeField] private UESwitchEvent onSwitch;

	[SerializeField] private bool isOn = false;

	private void UpdateUI()
	{
		this.enabledObj.SetActive (this.isOn);
		this.disabledObj.SetActive (!this.isOn);
	}

}
