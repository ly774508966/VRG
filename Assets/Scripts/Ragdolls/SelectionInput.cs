using UnityEngine;
using System.Collections;

public class SelectionInput : MonoBehaviour {
	
	public GameObject selectedObject;
	public Vector3 destinationCoordinates;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	if(Input.GetMouseButtonDown (0)) {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit,Mathf.Infinity)){
			selectedObject = hit.transform.gameObject.transform.root.gameObject;
			} else {
				selectedObject = null;
				
			}
		}
		
	if(Input.GetMouseButton (1)) {
			if(selectedObject && selectedObject.tag == "Character")
			{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast (ray,out hit,Mathf.Infinity)){
			destinationCoordinates = hit.point;
			selectedObject.SendMessage("SetDestination",destinationCoordinates);
			}
		}
		}
		
	}
	
	
}
