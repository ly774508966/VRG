using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptInterface : MonoBehaviour {
	
	public GameObject selectedCharacterInfoDisplay;
	public GameObject opposingCharacterInfoDisplay;
	public GameObject console;
	
	ScriptGameMaster scriptGameMaster;
	
	GUIStyle defaultStyle;
	
	//public bool testSwitch = true;
	
	
	// Use this for initialization
	void Start () {
		scriptGameMaster = GameObject.Find ("ControllerGame").GetComponent<ScriptGameMaster>();
	//console = transform.FindChild("Console").gameObject;	
	}
	
	// Update is called once per frame
	void Update () {
		
		selectedCharacterInfoDisplay.guiText.text = GetCharacterInfo(scriptGameMaster.selectedSheet);
		opposingCharacterInfoDisplay.guiText.text = GetCharacterInfo(scriptGameMaster.opposingSheet);

		
	}
	
	
	void AddNewLine(string textLine){
	
		console.SendMessage("AddNewLineConsole", textLine);
		
	}
	
	string GetCharacterInfo(ScriptCharacterSheet hotSheet){
		return hotSheet.stringID + 
			"\n HP " + hotSheet.meat.ToString() +
			"\n Priority " + hotSheet.readyPriority.ToString() +
			"\n Attack " + hotSheet.readyAttack.ToString() +
			//"\n Melee " + hotSheet.melee.ToString() +
			"\n Defense " + hotSheet.readyDefense.ToString() +
			"\n Muscle " + hotSheet.muscle.ToString() +
			"\n Range " + hotSheet.readyRange.ToString() +
			"\n Damage " + hotSheet.readyDamage.ToString() +
			//"\n Cooldown " + hotSheet.weaponCooldown.ToString() +
			"\n Hit % " + hotSheet.currentHitChance.ToString() +
			"\n Item: " + hotSheet.activeItem.fullName;
	}
	
	
}
