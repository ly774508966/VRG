using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour {
	
	//Scripts
public CommandScript commandScript;
public CharacterShellScript characterShellScript;
public CharacterSheetScript characterSheetScript;
	
	//Prefabs
public Rigidbody bullet;
	
	//Target
public GameObject gunSightObject;

	//Timing
public bool canShoot = true;
public float shotTimer = 0.0f;
	
	//Inputs
public bool inputFireWeapon = false;
	
	//Weapon Specs
public float weaponDamage;
public float weaponForce = 100.0f;
public float weaponRange;
public float weaponCooldown;


void Start () {	
		weaponDamage = characterSheetScript.weaponDamage;
		weaponRange = characterSheetScript.weaponRange;
		weaponCooldown = characterSheetScript.weaponCooldown;
	
		}
void FixedUpdate () {
		
		//Detect character in sights, cease aiming, and fire
	RaycastHit hit;
	if(Physics.Raycast(transform.position,transform.right,out hit,Mathf.Infinity)) {
			Debug.DrawLine (transform.position,hit.point,Color.red);
		gunSightObject = hit.collider.gameObject;
		} else {
			gunSightObject = null;
		}
						
		//Fire weapon
	if(inputFireWeapon && canShoot) {
			Debug.Log (Time.frameCount + "FireBeam" + transform.root.gameObject);
			StartCoroutine(FireBeam());
			
		}
			
			
			//Start weapon delay timer	
	if (!canShoot) {
	shotTimer += Time.deltaTime;
		}
			
			//Ready weapon and reset delay timer
	if(shotTimer >= weaponCooldown) {
			canShoot = true;
			shotTimer = 0f;
			}
			
		
	}
	
	
	IEnumerator FireBeam () {
		canShoot = false;
		RaycastHit hit;
		//Raycast starting position, direction, hit variable, distance
	if(Physics.Raycast(transform.position,transform.right,out hit,weaponRange)) {
		GameObject _box = hit.collider.gameObject;
		//	for(int _i=1; _i <= 1; _i++){
		//yield return null;
		//	}
		yield return null;	
		Debug.Log(Time.frameCount + "ApplyDamage" + transform.root.gameObject);
		audio.Play ();
		ApplyDamage (_box);
		ApplyForce (_box);
		
			}
		}
	
	void ApplyDamage (GameObject box) {
		box.SendMessageUpwards("ReceiveDamage",weaponDamage);
	}
	
	void ApplyForce (GameObject box) {
		if(!box.rigidbody.isKinematic){
			Vector3 forceDirection = transform.TransformDirection(Vector3.right);
			box.rigidbody.AddForce(forceDirection * weaponForce);
	}
	}
	
	/*void ApplyDamage (GameObject box) {
		GameObject character = box.transform.root.gameObject;
		character.SendMessage("ReceiveDamage", weaponDamage);
		CharacterShellScript characterShellScript = character.GetComponent<CharacterShellScript>();
		if(characterShellScript.currentHitPoints <= 0) {
		characterShellScript.inRagdoll = true;
		if(characterShellScript.inRagdoll == true) {
		character.SendMessage ("InitiateRagdoll", character);
		Vector3 forceDirection = transform.TransformDirection(Vector3.right);
		box.rigidbody.AddForce(forceDirection * weaponForce);	
		}
		}
	}*/

	
	
	
			
}
