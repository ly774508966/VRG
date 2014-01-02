using UnityEngine;
using System.Collections;

public class HazardHitbox : MonoBehaviour {
	
	XYCharacterSheet xYCharacterSheet;
	
	// Use this for initialization
	void Start () {
	
	xYCharacterSheet = transform.parent.GetComponent<XYCharacterSheet>();	
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//Harm character
	void OnTriggerEnter(Collider other){
		if (other.tag == "Hitbox"){
			other.SendMessage("TakeDamage",1);
		}
		if(other.tag == "Projectile"){
		SendMessageUpwards("TakeDamage", 1);	
		}
	}
	
	void TakeDamage(int amount){
		xYCharacterSheet.hitPoints -= 1;
		if(xYCharacterSheet.hitPoints <= 0){
					Destroy(gameObject.transform.parent.gameObject);
		}
	}
}
