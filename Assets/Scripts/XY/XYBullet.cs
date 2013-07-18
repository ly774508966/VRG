using UnityEngine;
using System.Collections;

public class XYBullet : MonoBehaviour {
	
	public bool isActive = false;
	public float inactiveTime = 0.1F;
	
	// Use this for initialization
	void Start () {
		StartCoroutine("BulletDelay");
		StartCoroutine("BulletLifespan");
		
	}
	
	// Update is called once per frame
	void Update () {
	//gameObject.rigidbody.WakeUp();
	}
	
	void OnCollisionEnter(Collision hit){
		Debug.Log(hit.gameObject);
		if (hit.gameObject.tag == "Hitbox"){
			
			hit.gameObject.SendMessage("TakeDamage",1);
		}
	}
	
	IEnumerator BulletDelay() {
	yield return new WaitForSeconds(inactiveTime);
	isActive = true;
	}
	
	IEnumerator BulletLifespan() {
		yield return new WaitForSeconds(2);
		Destroy(gameObject);
		
	}
	
}
