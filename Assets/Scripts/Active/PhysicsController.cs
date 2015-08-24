using UnityEngine;
using System.Collections;

public class PhysicsController : MonoBehaviour
{
	//Force
	public float propelForceConstant = 100F;
	public float headExplodeForce = 1000;

	//Geometry
	public float wallJointStrength = 1.0F;
	public float wallThresholdVelocity = 1.0F;

	//Blood effect
	public GameObject bloodLeak;

	//Containers
	public GameObject panelContainer;
	public Transform junkContainer;

	//Local variables
	public Vector3 rangedAttack;
	public Vector3 propelVector;
	public bool partIsDestroyed;

	//Temp
	public GameObject debug;

	private static PhysicsController _instance;
	public static PhysicsController Instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType<PhysicsController>();
			}
			return _instance;
		}
	}

	void Awake () {
		_instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		panelContainer = GameObject.Find ("ContainerGeometry");
		
		//foreach(GameObject wall in GameObject.Find ("ObjectBreakableWall")){
		//RegisterAllPanels(panelContainer);
		//GameObject poorBastard = GameObject.Find ("ObjectCharacterModel");
		//PropelChunk(poorBastard, propelForce);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.B)) {
			BlastWall (new Vector3 (1000, 1000, 1000), debug);	
		}
	
		if (Input.GetKeyDown (KeyCode.D)) {
			BlastWall (new Vector3 (500, 0, 0), panelContainer);	
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			Ragdollify(ScriptGameMaster.Instance.charactersInPlay[0].gameObject);
		}
	}

	public void Unragdollify (GameObject chunk)
	{
		foreach (Transform child in chunk.transform) {
			if (child.GetComponent<Rigidbody> () != null) {
				child.GetComponent<Rigidbody> ().isKinematic = true;
			} else {
				Unragdollify (child.gameObject);
			}
		}
	}
	
	public void Ragdollify (GameObject chunk)
	{
		foreach (Transform child in chunk.transform) {
			if (child.GetComponent<Rigidbody> () != null) {
				//	Debug.Log (child);
				if(!child.gameObject.name.Contains("head")) {
					child.GetComponent<Rigidbody> ().isKinematic = false;
				}

//				child.GetComponent<Rigidbody> ().WakeUp ();
			} else {
				Ragdollify (child.gameObject);
			}
		}
	}
	
	void Propel (Vector3 propelVector, GameObject targetObject)
	{
		foreach (Transform child in targetObject.transform) {
			if (child.GetComponent<Rigidbody> () != null) {
				child.GetComponent<Rigidbody> ().AddForce (propelVector);	
			} else {
				Propel (propelVector, child.gameObject);
			}
		}
	}
	
	/*
	void ExecuteCharacter(GameObject targetCharacter){
		Ragdollify(targetCharacter);
		//GameObject lastAttacker = targetCharacter.GetComponent<ScriptCharacterSheet>().lastAttacker ;
		//ScriptControllerTargeting hotCont = lastAttacker.GetComponentInChildren<ScriptControllerTargeting>(); 
		
		if(propel){
		Vector3 AttackDirection = (targetCharacter.transform.position - 
			targetCharacter.GetComponent<ScriptCharacterSheet>().lastAttacker.
				GetComponentInChildren<ScriptControllerTargeting>().transform.position);
		AttackDirection.Normalize();
		Propel (AttackDirection, targetCharacter);
		}
		if(blowUpHead){
		GameObject targetHead = targetCharacter.transform.Find("ObjectCharacterModel/head").gameObject;
		//Debug.Log (targetHead);
		targetHead.SendMessage("HeadExplode", headExplodeForce);
		//testHead.SendMessage("HeadExplode", 1000);
	
		}
	}
	*/
	/*
	IEnumerator KillCam(){
		yield return new WaitForSeconds(0.1);
		
		Debug.Break();
	}
	*/
	
	void BlastWall (Vector3 blastForce, GameObject hotChunk)
	{
		//Debug.Log ("BlastWall");
		foreach (Transform child in hotChunk.transform) {
			if (child.GetComponent<Collider> ()) {
				//if(Random.value <= 0.75)
				if (child.GetComponent<Rigidbody> () == null) {
					child.gameObject.AddComponent<Rigidbody> ();
				}
				child.GetComponent<Rigidbody> ().mass = 1;
				child.GetComponent<Rigidbody> ().drag = 0;
				child.GetComponent<Rigidbody> ().angularDrag = 0.05F;
				child.GetComponent<Rigidbody> ().useGravity = true;
				child.GetComponent<Rigidbody> ().isKinematic = false;
				child.GetComponent<Rigidbody> ().WakeUp ();
				child.GetComponent<Rigidbody> ().AddForce (blastForce);
				//	}
			} else {
				BlastWall (blastForce, child.gameObject);	
			}
		}
	}
	
	public void InitiateActionEffect (Result result)
	{
		//Reset "local" variables
		propelVector = Vector3.zero;
		partIsDestroyed = false;

		//For all damage, if limb is broken, spurt blood and dismember. If not, spurt blood only

		//Get attack direction
		rangedAttack = result.actingCharacter.
			GetComponentInChildren<ScriptControllerTargeting> ().rangedAttack;
		rangedAttack.Normalize ();


		//Debug.Log(result.targetNetHitProfile.head.ToString() + result.targetNetHitProfile.body.ToString() + 
		//      result.targetNetHitProfile.leftArm.ToString() + result.targetNetHitProfile.rightArm.ToString() + 
		//  result.targetNetHitProfile.leftLeg.ToString() + result.targetNetHitProfile.rightLeg.ToString());

		//Upon successful attack
		if (result.success) {
			ScriptModelController targetModelController = result.targetCharacter.
				GetComponentInChildren<ScriptModelController> ();

			//Ragdoll entire character if dead
			if (!result.targetCharacter.inPlay) {
				Ragdollify (result.targetCharacter.gameObject);
			}

			GameObject modelPart = null;
			//If damage is inflicted on body part and that body part is destroyed
			if (result.targetNetHitProfile.head < 0) {
				//Assign execution variables
				modelPart = targetModelController.head;
				propelVector = rangedAttack * -result.targetCharacter.currentHitProfile.head * propelForceConstant;
				if (result.targetCharacter.currentHitProfile.head <= 0) {
					partIsDestroyed = true;
				}
			}

			if (result.targetNetHitProfile.body < 0) {
				modelPart = targetModelController.spine;
				propelVector = rangedAttack * -result.targetCharacter.currentHitProfile.body * propelForceConstant;
				if (result.targetCharacter.currentHitProfile.body <= 0) {
					partIsDestroyed = true;
				}
			}

			if (result.targetNetHitProfile.leftArm < 0) {
				modelPart = targetModelController.leftArm;
				propelVector = rangedAttack * -result.targetCharacter.currentHitProfile.leftArm * propelForceConstant;
				if (result.targetCharacter.currentHitProfile.leftArm <= 0) {
					partIsDestroyed = true;
				}
			}

			if (result.targetNetHitProfile.rightArm < 0) {
				modelPart = targetModelController.rightArm;
				propelVector = rangedAttack * -result.targetCharacter.currentHitProfile.rightArm * propelForceConstant;
				if (result.targetCharacter.currentHitProfile.rightArm <= 0) {
					partIsDestroyed = true;
				}
			}

			if (result.targetNetHitProfile.leftLeg < 0) {
				modelPart = targetModelController.leftLeg;
				propelVector = rangedAttack * -result.targetCharacter.currentHitProfile.leftLeg * propelForceConstant;
				if (result.targetCharacter.currentHitProfile.leftLeg <= 0) {
					partIsDestroyed = true;
				}
			}

			if (result.targetNetHitProfile.rightLeg < 0) {
				modelPart = targetModelController.rightLeg;
				propelVector = rangedAttack * -result.targetCharacter.currentHitProfile.rightLeg * propelForceConstant;
				if (result.targetCharacter.currentHitProfile.rightLeg <= 0) {
					partIsDestroyed = true;
				}
			
			}
		
			//Assign struckBox randomly based on model part (if not already assigned by alpha)
			if (result.playerShotInfo.shotLocation == null) {
				result.playerShotInfo.shotLocation = modelPart.transform.GetChild ((int)Mathf.Floor (Random.value * modelPart.transform.childCount)).gameObject;
			}

			//Fire weapon at target's part if there is a struckBox
			if (result.playerShotInfo.shotLocation) {
				result.actingCharacter.GetComponentInChildren<ScriptModelController> ().weapon.GetComponent<ScriptWeapon> ().SendMessage ("GunshotEffect", result.playerShotInfo.shotLocation);
				//Debug.Log(struckBox.name);
			} else {
				Debug.Log ("struckBox requested but not assigned");
			}
			//	hotSheet.gameObject.GetComponentInChildren<ScriptModelController>().SendMessage("WeaponEffect");

			//Dismember if destroyed
			if (partIsDestroyed) {
//				Ragdollify (modelPart);
//				BreakJoints (result.playerShotInfo.shotLocation);
//				print ("propel vector, model part: " + propelVector +", " + modelPart);
//				Propel (propelVector, modelPart);
//				modelPart.transform.parent = junkContainer;
			}

			//Spurt blood
			//GameObject bloodBox = breakBox;
			//targetModelController.spine.transform.FindChild("spineBox1").gameObject;
			GameObject hotLeak = Instantiate (bloodLeak, result.playerShotInfo.shotLocation.transform.position, result.playerShotInfo.shotLocation.transform.rotation) as GameObject;
			hotLeak.transform.parent = result.playerShotInfo.shotLocation.transform;

		} else if (result.success == false) {

			//Fire weapon straight ahead
			result.actingCharacter.GetComponentInChildren<ScriptModelController> ().
				weapon.SendMessage ("GunshotEffect", result.playerShotInfo.shotRay); //natural shot ray

		} else {
			Debug.Log ("Something went horribly wrong");
		}

	}
	
	void BreakJoints (GameObject breakBox)
	{
		foreach (FixedJoint hotJoint in breakBox.GetComponents<FixedJoint>()) {
			Destroy (hotJoint);	
		}
			
	}

	void DecapitateModel (ScriptModelController targetCharacter, int damage)
	{
		//Instantiate(bloodLeak, targetCharacter.spine.transform.FindChild("spineBox1"));
	}

}
