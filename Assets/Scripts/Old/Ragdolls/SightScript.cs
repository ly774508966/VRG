using UnityEngine;
using System.Collections;

public class SightScript : MonoBehaviour {
public GameObject observedObject1;
public float sightRange = 5.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	RaycastHit hit;
		if(Physics.Raycast (transform.position,transform.right,out hit,sightRange)) 
		{
		Debug.DrawLine (transform.position,hit.point,Color.yellow);
		observedObject1 = hit.collider.transform.root.gameObject;
		} else {
			observedObject1 = null;
			
			
		}
		
	}
	
	
	
	public GameObject LocateProperObject (GameObject part) 
	{
		if(part.tag == "Character") 
		{
			return part;
		} else if (!part.transform.parent.gameObject) {
				return null;
		} else {
			return LocateProperObject(part.transform.parent.gameObject);
		}	
	}
}