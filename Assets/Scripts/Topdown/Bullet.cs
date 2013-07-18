using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	
	public bool isActive = false;
	public float inactiveTime = 0.1F;
	
	// Use this for initialization
	void Start () {
		StartCoroutine("BulletDelay");
	}
	
	// Update is called once per frame
	void Update () {
	gameObject.rigidbody.WakeUp();
	}
	
	void OnTriggerEnter(Collider other){
		if (other.tag == "Hitbox"){
			other.SendMessage("TakeDamage",1);
		}
	}
	
	
	IEnumerator BulletDelay() {
	yield return new WaitForSeconds(inactiveTime);
	isActive = true;
	}
	
}
