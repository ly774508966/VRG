using UnityEngine;
using System.Collections;

public class ScriptModelArm : MonoBehaviour {
	
	
	public bool leftArmLerp = false;
	
	//Lerp constants
	public float lerpDuration;
	
	
	//Static lerp variables
	 Transform leftArmRaised;
	 Transform leftArmIdle;
	 float startTime;
	public float currentTime;
	
	//Dynamic lerp variables
	public float fractionComplete; //fracJourney
	
	
	// Use this for initialization
	void Start () {
	leftArmIdle = transform;
	leftArmRaised = transform.FindChild("leftArmRaisedTransform");
	
	InitializeLeftArmRaiseLerp();
	}
	
	// Update is called once per frame
	void Update () {
	if(leftArmLerp){
			currentTime = Time.time;
			fractionComplete = (Time.time - startTime) / lerpDuration;
		transform.rotation = Quaternion.Lerp(leftArmIdle.rotation, leftArmRaised.rotation, fractionComplete);
		
		}
	}
	
	void InitializeLeftArmRaiseLerp(){
		startTime = Time.time;
		
		
		leftArmLerp = true;
	}
}
