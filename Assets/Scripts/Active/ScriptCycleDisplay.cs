using UnityEngine;
using System.Collections;

public class ScriptCycleDisplay : MonoBehaviour {
	private GUIText _cycleText = null;
	GUIText CycleText {
		get {
			if (_cycleText == null) {
				_cycleText = transform.FindChild ("GUICycleDisplay").GetComponent<GUIText> ();
			}
			return _cycleText;
		}
	}
	//int cycleNumber;
	//ScriptGameMaster scriptGameMaster;
	
	// Use this for initialization
	void Start () {
		_cycleText = transform.FindChild("GUICycleDisplay").GetComponent<GUIText>();
		//scriptGameMaster = GameObject.Find ("ControllerGame").GetComponent<ScriptGameMaster>();
	}
	
	// Update is called once per frame
	void Update () {
		//if(scriptGameMaster.selectedSheet && scriptGameMaster.charactersInPlay.Count > 0){
		//	selectedCharacterInfoDisplay.guiText.text = GetCharacterInfo(scriptGameMaster.selectedSheet);
		//}
	//}
	}
	
	void UpdateCycle(int currentCycle){
		//cycleNumber = currentCycle;
//		print ("gui component: " + guiCycleDisplay.GetComponent<GUIText>().text);
//		print ("current cycle: " + currentCycle.ToString());

		CycleText.GetComponent<GUIText>().text = currentCycle.ToString();
	}
}