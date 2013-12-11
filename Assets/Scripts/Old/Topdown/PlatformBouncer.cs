using UnityEngine;
using System.Collections;

public class PlatformBouncer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter (Collision hit){
	GameObject reflectedProjectile = hit.gameObject;
		if(reflectedProjectile.tag == "Projectile"){
		Rigidbody reflectedRigidbody = reflectedProjectile.GetComponent<Rigidbody>();
	reflectedRigidbody.velocity = reflectedRigidbody.velocity * -10;
		}
	}
}
