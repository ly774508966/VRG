using UnityEngine;
using System.Collections;

public class ZoomCameraScript : MonoBehaviour {
	
	public float zoomSpeed = 1f;
	public float zoomInput;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	/*float*/ zoomInput = Input.GetAxis ("CameraZoom");
	GetComponent<Camera>().orthographicSize += zoomInput * Time.deltaTime * zoomSpeed;
	}
}
