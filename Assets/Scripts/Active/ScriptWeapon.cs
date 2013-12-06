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

	//Hit overload
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

	//Miss overload
	void GunshotEffect (ScriptModelController missedModel)
	{
		//Shoot projectile randomly
		GameObject hotProj = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
		hotProj.transform.parent = junkContainer.transform;
		Vector3 attackVector =  (missedModel.gameObject.transform.position 
		                         ) + new Vector3(0, (Random.value - 0.5F)*4, (Random.value - 0.5F)*4) //Magic numbers
		                          - projectileSpawn.position; 
		hotProj.rigidbody.AddForce(attackVector * projectileSpeed);

		//Sound effect
		gunSound.Play ();

		//Muzzle Flash
		hotFlash = Instantiate(muzzleFlashPrefab, transform.position, transform.rotation) as GameObject;
		StartCoroutine("EndFlash");
	}

	//Wild miss overload
	void GunshotEffect (int dummyArg)
	{
		//Shoot projectile randomly
		Debug.Log("Wild Miss Overload");
		GameObject hotProj = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
		hotProj.transform.parent = junkContainer.transform;
		Vector3 attackVector = new Vector3();
		if(transform.parent.parent.rotation.y == 180)
		{
		attackVector =  new Vector3(10, (Random.value - 0.5F)*4, (Random.value - 0.5F)*4) //Magic numbers
			- projectileSpawn.position; 
		}
		else
		{
			attackVector =  new Vector3(-10, (Random.value - 0.5F)*4, (Random.value - 0.5F)*4) //Magic numbers
				- projectileSpawn.position; 
		}
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
