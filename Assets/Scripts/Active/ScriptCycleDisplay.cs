using UnityEngine;
using System.Collections;

public class ScriptCycleDisplay : MonoBehaviour {
	
	GameObject guiCycleDisplay;
	//int cycleNumber;
	//ScriptGameMaster scriptGameMaster;
	
	// Use this for initialization
	void Start () {
		guiCycleDisplay = transform.FindChild("GUICycleDisplay").gameObject;
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
			guiCycleDisplay.guiText.text = currentCycle.ToString();
	}
}