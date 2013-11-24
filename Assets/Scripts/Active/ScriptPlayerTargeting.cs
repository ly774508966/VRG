using UnityEngine;
using System.Collections;

public class ScriptPlayerTargeting : MonoBehaviour {

	//Draw visible ray with length equal to character's weapon, which oscillates z rotation so that ray passes over the enemy
	//Take clicks as input and return break box (if any)
	//Use break box to run new result on hit location
	//Pass result through game master (and physics controller)

	public float distance = 5.0F;
	public int theAngle = 45;
	public int segments = 10;

	bool shotInputReady = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//if(Input.GetKeyDown (KeyCode.R))
		  // {
			RaycastSweep();
		//}
		//Update ray position
		//Ray indicatorRay = new Ray(transform.position, new Vector3(

		//Draw ray
		//Debug.DrawRay(indicatorRay.origin, indicatorRay.direction, Color.green);

		//if(shotInputReady)
		//{

		//}

	}

	void RaycastSweep()
		{
			Vector3 startPosition = transform.position;
			Vector3 targetPosition = Vector3.zero;

			int startAngle = (int)(-theAngle * 0.5);
			int finishAngle = (int) (theAngle * 0.5);

		int rayIncrement = (int)(theAngle / segments);

		RaycastHit hit;

		for(int i = startAngle; i < finishAngle; i+= rayIncrement)
		{
			targetPosition = (Quaternion.Euler(0, i, 0) * transform.forward).normalized * distance;

			if(Physics.Linecast(startPosition, targetPosition, out hit))
			{
				Debug.Log ("Hit" + hit.collider.gameObject.name);
			}

			Debug.DrawLine (startPosition, targetPosition, Color.green);
		}


		}
}
