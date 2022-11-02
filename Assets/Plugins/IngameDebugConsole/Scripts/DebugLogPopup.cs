using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
#if UNITY_EDITOR && UNITY_2021_1_OR_NEWER
using Screen = UnityEngine.Device.Screen; // To support Device Simulator on Unity 2021.1+
#endif

// Manager class for the debug popup
namespace IngameDebugConsole
{
	public class DebugLogPopup : MonoBehaviour
	{




#pragma warning disable 0649

		public DebugLogManager debugManager;


#pragma warning restore 0649






		// Hides the log window and shows the popup
		public void Show()
		{
			debugManager.ShowLogWindow();

		}
	}

}