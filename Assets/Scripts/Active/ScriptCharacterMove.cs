using UnityEngine;
using System.Collections;

public class ScriptCharacterMove : MonoBehaviour {
	
	ScriptCharacterSheet scriptCharacterSheet;
	public Vector3 startMarker;
	public Vector3 endMarker;
	public float movementSpeed = 0.5F;
	public float journeyLength;
	public float startTime;
	public bool greenLight = false;
	//public bool redLight = false;
	public bool startLerp;
	public float fracJourney;
	public bool atDestination;
	
	
	//public float durationMultiplier = 1;
	
	// Use this for initialization
	void Start () {
		scriptCharacterSheet = GetComponent<ScriptCharacterSheet>();
		
		
		//GreenLight();
		//RedLight();
		
	
		
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
		
		if(greenLight){	
			//Debug.Log ("green");
			if(startLerp){
				//Debug.Log ("reset");
				startLerp = false;
			ResetLerp();	
			}
			
			if(!atDestination){
			if(journeyLength > 0.0F){
				if(atDestination){
				atDestination = false;
				}
				
				
			fracJourney = (Time.time - startTime) / journeyLength;
				if(fracJourney < 1.0F){
				//float distCovered = (Time.time - startTime) * movementSpeed;
				transform.position = Vector3.Lerp (startMarker, endMarker, fracJourney);
				} else {
					atDestination = true;
					//scriptGameMaster.SendMessage("SetToEngagementMode");
				}
				
				
			} else {
				atDestination = true;
					//scriptGameMaster.SendMessage("SetToEngagementMode");
				}
			
			}
		}
		
		
	
		
		
		
		
	}
	void ResetLerp(){
		//Debug.Log ("Reset");
		startTime = Time.time;
				startMarker = transform.position;
				endMarker = scriptCharacterSheet.positionObjective;
				journeyLength = Vector3.Distance(startMarker, endMarker);
		if(atDestination){
		if((scriptCharacterSheet.positionObjective - transform.position).magnitude > 0){
				atDestination = false;
			}
		}
		
		//Debug.Break ();
	}
	
	/*
	void GreenLight(){
		//Assign static variables
		startTime = Time.time;
		startMarker = transform.position;
		endMarker = scriptCharacterSheet.destination;
		journeyLength = Vector3.Distance(startMarker, endMarker);
		
		//Assign dynamic variables
		float distCovered = (Time.time - startTime) * movementSpeed;
		float fracJourney = (Time.time - startTime) / journeyLength;
		
		//Lerp
		
		
		
		//float duration = (endPoint - startPoint).magnitude * durationMultiplier	
	}
	
	void RedLight(){
		
	}
	*/
}
