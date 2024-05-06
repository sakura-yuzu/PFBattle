
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class ToggleGroupInherit : ToggleGroup
{
	public List<Toggle> toggles;
	public void SetAllTogglesEnable(bool enable)
	{
		toggles.ForEach((Toggle toggle)=>{ toggle.enabled = enable; });
	}

	public string getValue()
	{
		IEnumerable<Toggle> ggles = this.ActiveToggles();
		Toggle ggle = ggles.FirstOrDefault();
		if(ggle){
			return ggle.GetComponent<BaseButton>().value;
		}
		return "";
	}
}