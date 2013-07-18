using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	
	public GameObject enemy;
	public bool spawnEnemyNow = true;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	if(spawnEnemyNow == true){
		Instantiate(enemy, new Vector3(transform.position.x + Random.value * 30 - 15, transform.position.y, transform.position.z), transform.rotation);
		spawnEnemyNow = false;
		StartCoroutine("EnemyTimer");
		}
	
	}
	
	IEnumerator EnemyTimer(){
	yield return new WaitForSeconds(1);
	spawnEnemyNow = true;
	}
}
