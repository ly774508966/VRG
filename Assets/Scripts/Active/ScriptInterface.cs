using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptInterface : MonoBehaviour {
	
	public GameObject selectedCharacterComparisonDisplay;
	public GameObject selectedCharacterSheetDisplay;
	public GameObject opposingCharacterSheetDisplay;
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
		
		selectedCharacterComparisonDisplay.guiText.text = GetCharacterComparison(scriptGameMaster.selectedSheet, scriptGameMaster.opposingSheet);
		//opposingCharacterComparisonDisplay.guiText.text = GetMirroredCharacterComparison(scriptGameMaster.opposingSheet);
		selectedCharacterSheetDisplay.guiText.text = GetCharacterSheet(scriptGameMaster.selectedSheet);
		opposingCharacterSheetDisplay.guiText.text = GetCharacterSheet(scriptGameMaster.opposingSheet);
		
	}
	
	
	void AddNewLine(string textLine){
	
		console.SendMessage("AddNewLineConsole", textLine);
		
	}
	/*
	string GetStandardCharacterComparison(ScriptCharacterSheet hotSheet){
		return 
			//hotSheet.stringID + 
			//"\n HP " + hotSheet.baseToughness.ToString() +
			"\n Priority " + hotSheet.readyPriority.ToString() +
			"\n Attack " + hotSheet.readyAttack.ToString() +
			//"\n Melee " + hotSheet.melee.ToString() +
			"\n Defense " + hotSheet.readyDefense.ToString() +
			"\n Muscle " + hotSheet.baseMuscle.ToString() +
			"\n Range " + hotSheet.readyRange.ToString() +
			"\n Damage " + hotSheet.readyDamage.ToString() +
			//"\n Cooldown " + hotSheet.weaponCooldown.ToString() +
			"\n Hit % " + hotSheet.currentHitChance.ToString();
			//"\n Item: " + hotSheet.activeItem.fullName;
	}
	
		string GetMirroredCharacterComparison(ScriptCharacterSheet hotSheet){
		return 
			//hotSheet.stringID + 
			//"\n" + hotSheet.baseToughness.ToString() + " HP" +
			"\n" + hotSheet.readyPriority.ToString() + " Priority" +
			"\n" + hotSheet.readyDefense.ToString() +" Defense" + 
			"\n" + hotSheet.readyAttack.ToString() + " Attack" + 
			"\n" + hotSheet.baseMuscle.ToString() + " Muscle" + 
			"\n" + hotSheet.readyRange.ToString() +  " Range"  +
			"\n" + hotSheet.readyDamage.ToString() + " Damage" + 
			"\n" + hotSheet.currentHitChance.ToString() + " Hit %";
	}
	*/

	string GetCharacterComparison(ScriptCharacterSheet selectedCharacter, ScriptCharacterSheet opposingCharacter)
	{
		return string.Format("{0}     {1}"
		                     + "\n Range {2:00}     {3:00} Range"
		                     + "\n Attack {4:00}     {5:00} Defense" 
		                     + "\n Defense {6:00}     {7:00} Attack" 
		                     + "\n Priority {8:00}     {9:00} Priority", 
		                     new object[] {selectedCharacter.stringID, opposingCharacter.stringID,
			selectedCharacter.readyRange, opposingCharacter.readyRange, selectedCharacter.readyAttack, 
			opposingCharacter.readyDefense,	selectedCharacter.readyDefense, opposingCharacter.readyAttack, 
			selectedCharacter.readyPriority, opposingCharacter.readyPriority});
	}

	string GetCharacterSheet(ScriptCharacterSheet hotSheet)
	{
		return hotSheet.stringID +
			"\n Toughness: " + hotSheet.toughness +
			"\n Head: " + hotSheet.currentHitProfile.head.ToString() + "/" + hotSheet.maxHitProfile.head.ToString() +
				"\n Body: " + hotSheet.currentHitProfile.body.ToString() + "/" + hotSheet.maxHitProfile.body.ToString() +
				"\n Left Arm: " + hotSheet.currentHitProfile.leftArm.ToString() + "/" + hotSheet.maxHitProfile.leftArm.ToString() +
				"\n Right Arm: " + hotSheet.currentHitProfile.rightArm.ToString() + "/" + hotSheet.maxHitProfile.rightArm.ToString() +
				"\n Left Leg: " + hotSheet.currentHitProfile.leftLeg.ToString() + "/" + hotSheet.maxHitProfile.leftLeg.ToString() +
				"\n Right Leg: " + hotSheet.currentHitProfile.rightLeg.ToString() + "/" + hotSheet.maxHitProfile.rightLeg.ToString() +
			"\n Item: " + hotSheet.activeItem.fullName;
	}
	
}
