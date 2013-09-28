using UnityEngine;
using System.Collections;

public class ScriptWeapon : MonoBehaviour {
	
	public GameObject muzzleFlashPrefab;
	public float muzzleFlashDuration = 0.5F;
	public GameObject hotFlash;
	public AudioSource gunSound;
	
	// Use this for initialization
	void Start () {
	//MuzzleFlash();
		
		gunSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void GunshotEffect(){
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
}
