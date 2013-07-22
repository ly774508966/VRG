using UnityEngine;
using System.Collections;

public class ScriptButton : MonoBehaviour {
	
	public Rect buttonRect = new Rect(150F, 1500F, 300F, 100F);
	
	public ScriptGameMaster scriptGameMaster = null;
	
	void OnGUI(){
		
		GUI.BeginGroup(new Rect(0.0F, Screen.height / 2, Screen.width, Screen.height / 2));
		
		//During Command Phase, show buttons and take input
		//if(scriptGameMaster.playerPrompt){
		GUI.Box (new Rect(10,0,100,90), "Attack");
			//Make the following toggle (boolean image) buttons
			if(GUI.Button (new Rect(20, 30, 80, 20), "Melee")){
				scriptGameMaster.inputButtonName = "Melee";
			}
			if(GUI.Button (new Rect(20, 60, 80, 20), "Ranged")){
				scriptGameMaster.inputButtonName = "Ranged";
			}
		
			//Next Step button
	
		//}
		
		GUI.EndGroup();
		
		GUI.BeginGroup(new Rect(Screen.width/2, Screen.height/2, Screen.width/2, Screen.height / 2));
			if(GUI.Button (new Rect(20, 60, 80, 20), "Next")){
				scriptGameMaster.inputButtonName = "Next";
			}
		GUI.EndGroup();
		
		
		
	}
}
