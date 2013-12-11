using UnityEngine;
using System.Collections;

public class BlockScript : MonoBehaviour {
	
	public bool isBroken = false;
	public int currentHitPoints = 1;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void ReceiveDamage (int damageAmount) {
	currentHitPoints -= damageAmount;
	if(isBroken == false && currentHitPoints <= 0) {
	isBroken = true;
	Break();
	}	
	}
	
	void Break () {
		rigidbody.isKinematic = false;
		rigidbody.WakeUp();
	}
	
}
