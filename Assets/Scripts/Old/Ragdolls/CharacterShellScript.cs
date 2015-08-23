using UnityEngine;
using System.Collections;

public class CharacterShellScript : MonoBehaviour {
	
	//Scripts
	public CharacterSheetScript characterSheetScript;
	
	//Stats
	public float currentHitPoints;
	public float maxHitPoints;
	public float nerves;
	
	//States
	public bool isAlive = true;
	
	//GUI
	public GameObject damageDisplay;
	
	
		void Start () {
	currentHitPoints = characterSheetScript.meat;
	maxHitPoints = characterSheetScript.meat;
	nerves = characterSheetScript.nerves;
	}				
	
void FixedUpdate () {
			
	}
void InitiateRagdoll (GameObject part) {
		foreach (Transform child in part.transform) {
			if(child.gameObject.GetComponent<Rigidbody>()) {
				InitiateBoxPhysics(child.gameObject.GetComponent<Rigidbody>());
			} else {
				InitiateRagdoll (child.gameObject);
				}
			}
		}
void InitiateBoxPhysics (Rigidbody box) {
		box.GetComponent<Rigidbody>().isKinematic = false;
		box.GetComponent<Rigidbody>().WakeUp();
}
	
void ReceiveDamage (int damageAmount) {
	GameObject currentDamageDisplay = Instantiate(damageDisplay, new Vector3(transform.position.x,transform.position.y,-1.0f), Quaternion.identity) as GameObject;
	currentDamageDisplay.GetComponentInChildren<TextMesh>().text = "" + damageAmount;
		currentHitPoints -= damageAmount;
	if(isAlive == true && currentHitPoints <= 0) {
	isAlive = false;
	Die();
	InitiateRagdoll(gameObject);
	//Debug.Break ();
	}
	}

void Die () {
	//Destroy (GetComponent<Rigidbody>());
	//Destroy (GetComponent<BoxCollider>());
	//GunScript gunScript = GetComponentInChildren<GunScript>();
	//gunScript.enabled = false;
	GetComponent<MoveCharacterScript>().enabled = false;
	//GetComponentInChildren<SightScript>().enabled = false;
	GetComponentInChildren<ArmRotateScript>().enabled = false;
	
	}
	
		

}