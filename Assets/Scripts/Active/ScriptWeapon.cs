using UnityEngine;
using System.Collections;

public class ScriptWeapon : MonoBehaviour {
	


	//Item Specs

//Effects

	//Animation
	public GameObject muzzleFlashPrefab;
	public float muzzleFlashDuration = 0.25F;
	public GameObject hotFlash;

	//Projectile
	public GameObject bulletPrefab;
	public Transform projectileSpawn;
	public float projectileSpeed;

	//Sound
	public AudioSource gunSound;
	

//Containers
	public GameObject junkContainer;

	
	// Use this for initialization
	void Start () {
	//MuzzleFlash();
		
		gunSound = GetComponent<AudioSource>();

		junkContainer = GameObject.Find("ContainerJunk");



	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void GunshotEffect(GameObject breakBox){

		//Shoot projectile
		GameObject hotProj = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
		hotProj.transform.parent = junkContainer.transform;
		//hotProj.rigidbody.AddForce(new Vector3(1000, 1000, 1000));
		Vector3 attackVector =  breakBox.transform.position - projectileSpawn.position; 
		hotProj.rigidbody.AddForce(attackVector * projectileSpeed);

		//Sound effect
		gunSound.Play ();

		//Muzzle Flash
	hotFlash = Instantiate(muzzleFlashPrefab, transform.position, transform.rotation) as GameObject;
	StartCoroutine("EndFlash");		
	}
	
	IEnumerator EndFlash(){
		
		yield return new WaitForSeconds(muzzleFlashDuration);
		Destroy(hotFlash);
	}

	void UpdateItemStats()
	{
		//Assign item sound effect(s)
		//gunSound.clip;

		//Assign projectile properties

		//Assign item appearance

	}
}
