using UnityEngine;
using System.Collections;

public class XYShooter : MonoBehaviour {
	
public GameObject bullet;
	
public Vector3 shotVector = Vector3.zero;
public float shotForce = 10.0F;
public bool isFiring = false;
public bool canFire = true;
public float shotDelay = 1.0F;
public float fireInput = 0.0F;
public Vector3 shotInput = Vector3.zero;
	
	// Use this for initialization
	void Start () {
	CharacterController characterController = GetComponent<CharacterController>();
	characterController.detectCollisions = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(fireInput >= 0.9F && canFire){
		canFire = false;
		GameObject newBullet;
		newBullet = Instantiate (bullet,new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.0f), transform.rotation) as GameObject;
		newBullet.GetComponent<Rigidbody>().AddForce(shotInput * shotForce);
				//(transform.TransformDirection(shotVector * shotForce));
		StartCoroutine("ShotTimer");
		}
		}

IEnumerator ShotTimer(){
		yield return new WaitForSeconds(shotDelay);
		canFire = true;
	}

}
