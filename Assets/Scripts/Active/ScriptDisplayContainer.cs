using UnityEngine;
using System.Collections;

public class ScriptDisplayContainer : MonoBehaviour {

	public Camera camera00;
	//public Camera camera01;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//Vector3 vectorToCamera = mainCamera.transform.position - tranform.position;

		if(Camera.main == camera00)
		{
			//Debug.Log ("forward");
			transform.eulerAngles = Vector3.zero;
		}
		else if(Camera.main.transform.position.x > transform.position.x)
		{
			//Debug.Log ("right");
			transform.eulerAngles = new Vector3(0, 270, 0);
		}
		else if(Camera.main.transform.position.x <= transform.position.x)
		{
			//Debug.Log ("left");
			transform.eulerAngles = new Vector3(0, 90, 0);
		}
		else
		{
			Debug.Log ("Invalid Camera State");
		}

		//transform.LookAt (mainCamera.transform);
		//transform.Rotate(0,180,0);
	}

	
	void DisplayOver () {
	Destroy (gameObject);
}
	
}
