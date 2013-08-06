using UnityEngine;
using System.Collections;

public class ScriptButton : MonoBehaviour {
	
	public Rect buttonRect = new Rect(150F, 1500F, 300F, 100F);
	public Vector2 scrollPosition = Vector2.zero;
	
	public ScriptGameMaster scriptGameMaster;
	
	void Start(){
	scriptGameMaster = GameObject.Find ("ControllerGame").GetComponent<ScriptGameMaster>();	
		
	}
	
	void OnGUI(){
		
		GUI.BeginGroup(new Rect(0.0F, Screen.height / 2, Screen.width, Screen.height / 2));
		
		//During Command Phase, show buttons and take input
		//if(scriptGameMaster.playerPrompt){
		GUI.Box (new Rect(10,0,100,90), "Attack");
			//Make the following toggle (boolean image) buttons
			if(GUI.Button (new Rect(20, 25, 80, 20), "Melee")){
				scriptGameMaster.inputButtonName = "Melee";
			}
			if(GUI.Button (new Rect(20, 50, 80, 20), "Ranged")){
				scriptGameMaster.inputButtonName = "Ranged";
			}
		
			//Next Step button
			if(GUI.Button (new Rect(20, 100, 80, 20), "Next")){
				scriptGameMaster.inputButtonName = "Next";
			}
		//}
		
		GUI.EndGroup();
		
		//Bottom-righthand corner 
		//GUI.BeginGroup(new Rect(Screen.width/2, Screen.height/2, Screen.width/2, Screen.height / 2));
		//GUI.EndGroup();
		
		/* Scrollbar
		GUI.BeginGroup(new Rect(0, 0, Screen.width / 2, Screen.height/2));
		GUILayout.BeginArea(new Rect(0, 0, 100, Screen.height / 2));
		scrollPosition = GUI.BeginScrollView (
                      new Rect (0,0,100,100),     // screen position
                      scrollPosition,            // current scroll position
                      new Rect (0, 0, 100, 200)      // content area
                 );
		GUILayout.Label("String 0");
		//GUILayout.Box ("String 1"); //Writes "String 1" on first line of body.
		GUI.Button (new Rect (0,0,100,20), "Top-left");
		GUI.Button (new Rect (120,0,100,20), "Top-right");
		GUI.Button (new Rect (0,180,100,20), "Bottom-left");
		GUI.Button (new Rect (120,180,100,20), "Bottom-right");
		GUI.EndScrollView();
		GUILayout.EndArea();
		GUI.EndGroup();
	*/
	}
}
