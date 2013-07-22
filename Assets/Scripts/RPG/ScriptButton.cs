using UnityEngine;
using System.Collections;

public class ScriptButton : MonoBehaviour {
	
	public Rect buttonRect = new Rect(150F, 1500F, 300F, 100F);
	
	public ScriptGameMaster scriptGameMaster = null;
	
	void OnGUI(){
		
		GUI.BeginGroup(new Rect(0.0F, Screen.height / 2, Screen.width, Screen.height / 2));
		
		//During Command Phase, show buttons and take input
		if(scriptGameMaster.playerPrompt){
			//Fight button
			if(GUI.Button (new Rect(15, 15, 100, 100), "Fight")){
				scriptGameMaster.inputButtonName = "Fight";
			}
			//Flight button
			if(GUI.Button (new Rect(150, 15, 100, 100), "Flight")){
				scriptGameMaster.inputButtonName = "Flight";
			}
		}
		
		GUI.EndGroup();
		
	}
}
