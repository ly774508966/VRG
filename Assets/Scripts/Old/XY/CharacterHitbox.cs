using UnityEngine;
using System.Collections;

public class CharacterHitbox : MonoBehaviour {
	
	XYCharacterSheet xYCharacterSheet;
	public bool isInvulnerable = false;
	public Material invulnerabilityMaterial;
	public Material defaultMaterial;
	
	// Use this for initialization
	void Start () {
	
	xYCharacterSheet = transform.parent.GetComponent<XYCharacterSheet>();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	

	
	/*void OnTriggerEnter(Collider other){
		if(other.tag == "Projectile"){
			bool bulletIsActive	= other.GetComponent<Bullet>().isActive;
			if(!isInvulnerable && bulletIsActive){
				xYCharacterSheet.hitPoints -= 1;
				if(xYCharacterSheet.hitPoints <= 0){
					Destroy(gameObject.transform.parent.gameObject);
				} else {
					StartCoroutine("Invulnerability");
				}
			}
		}
	}*/
	
IEnumerator Invulnerability (){
	GetComponent<Renderer>().material = invulnerabilityMaterial;
	isInvulnerable = true;
	yield return new WaitForSeconds(1.0F);
	GetComponent<Renderer>().material = defaultMaterial;
	isInvulnerable = false;
	
	}
	
	void TakeDamage(int amount){
		xYCharacterSheet.hitPoints -= 1;
		if(xYCharacterSheet.hitPoints <= 0){
					Destroy(gameObject.transform.parent.gameObject);
		}
	}
	
}
