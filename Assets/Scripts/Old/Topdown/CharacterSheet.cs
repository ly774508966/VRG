using UnityEngine;
using System.Collections;

//TODO Deprecated

public class CharacterSheet : MonoBehaviour {
	
	public float HP = 3.0F;
	public bool isInvulnerable = false;
	public Material invulnerabilityMaterial;
	public Material defaultMaterial;
	// Use this for initialization
	void Start () {
	gameObject.transform.parent.GetComponent<Renderer>().material = defaultMaterial;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider hit){
	bool bulletIsActive	= hit.gameObject.GetComponent<Bullet>().isActive;
	if(!isInvulnerable && bulletIsActive){
		HP -= 1.0F;
		if(HP <= 0.0F){
			Destroy(gameObject.transform.parent.gameObject);
			}
		
		StartCoroutine("Invulnerability");
		}
	}
	
IEnumerator Invulnerability (){
	gameObject.transform.parent.GetComponent<Renderer>().material = invulnerabilityMaterial;
	isInvulnerable = true;
	yield return new WaitForSeconds(1.0F);
	gameObject.transform.parent.GetComponent<Renderer>().material = defaultMaterial;
	isInvulnerable = false;
	
	}
	
}
