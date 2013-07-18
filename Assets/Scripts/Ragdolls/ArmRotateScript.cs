using UnityEngine;
using System.Collections;

public class ArmRotateScript : MonoBehaviour {
	
	public CharacterShellScript characterShellScript;
	public float armRotateSpeed = 100f;
	public float inputAimAxis = 0.0f;
	public float armZEulerAngle;
	
	void Start () {
	
	}
	
	void FixedUpdate () {
	
	if(inputAimAxis != 0f)
		{
	float armTransformRotation = inputAimAxis * Time.deltaTime * armRotateSpeed;
	transform.Rotate(0,0,armTransformRotation);
		}
	}
	
	
	}
	

