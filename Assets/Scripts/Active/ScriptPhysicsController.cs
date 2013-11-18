using UnityEngine;
using System.Collections;



public class ScriptPhysicsController : MonoBehaviour {

	public float propelForceConstant = 100F;

	public float wallJointStrength = 1.0F;
	public float wallThresholdVelocity = 1.0F;
	public float headExplodeForce = 1000;
	//public float propelForce = 10000F;
	public GameObject panelContainer;
	public GameObject debug;
	
	//Local variables
	public GameObject modelPart;
	public GameObject breakBox;
	public Vector3 rangedAttack;
	public Vector3 propelVector;
	
	//Local variables
	//public bool propel;
	//public bool blowUpHead;
	
	
	
	//public GameObject testHead;
	
	// Use this for initialization
	void Start () {
		
	panelContainer = GameObject.Find ("ConPanel");
		
	//foreach(GameObject wall in GameObject.Find ("ObjectBreakableWall")){
	//RegisterAllPanels(panelContainer);
	//GameObject poorBastard = GameObject.Find ("ObjectCharacterModel");
	//PropelChunk(poorBastard, propelForce);
	
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	if(Input.GetKeyDown(KeyCode.B))
	{
	BlastWall(new Vector3(1000, 1000, 1000), debug);	
	}
	
	if(Input.GetKeyDown(KeyCode.D))
		{
		BlastWall (Vector3.zero, panelContainer);	
		}
		
	}
	/*
	void RegisterAllPanels(GameObject wallSegment){
		foreach(Transform child in wallSegment.transform){
			if(child.rigidbody == null){
				//Debug.Log ("Run");
				RegisterAllPanels(child.gameObject);
			} else {
				GameObject hotPanel = child.gameObject;
				//Destroy(hotPanel.rigidbody);
				//Add panel scripts
				//hotPanel.AddComponent("ScriptWallPanel");
				//hotPanel.GetComponent<ScriptWallPanel>().wallImpactThreshold = wallThresholdVelocity;
				//hotPanel.AddComponent("FixedJoint");
		
		//FixedJoint hotJoint = hotPanel.GetComponent<FixedJoint>();
		//		hotJoint.breakForce = wallJointStrength; 
		//		hotJoint.rigidbody.isKinematic = true;
			}
		}
	}
	*/
		void Unragdollify(GameObject chunk){
		
		foreach(Transform child in chunk.transform){
		if(child.rigidbody != null){
			child.rigidbody.isKinematic = true;
			} else {
			Unragdollify(child.gameObject);
			}
		}
	}
	
	
	void Ragdollify(GameObject chunk){
		
		foreach(Transform child in chunk.transform){
		if(child.rigidbody != null){
			//	Debug.Log (child);
			child.rigidbody.isKinematic = false;
			child.rigidbody.WakeUp();
			} else {
			Ragdollify(child.gameObject);
			}
		}
	}
	
	void Propel(Vector3 propelVector, GameObject targetObject){
		foreach(Transform child in targetObject.transform){
			if(child.rigidbody != null){
				child.rigidbody.AddForce( propelVector);	
				} else {
			Propel(propelVector, child.gameObject);
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
	
	void BlastWall(Vector3 blastForce, GameObject hotChunk)
	{
		//Debug.Log ("BlastWall");
		foreach(Transform child in hotChunk.transform)
		{
			if(child.collider)
			{
			//if(Random.value <= 0.75)
			//{
			child.gameObject.AddComponent<Rigidbody>();
			child.rigidbody.mass = 1;
			child.rigidbody.drag = 0;
			child.rigidbody.angularDrag = 0.05F;
			child.rigidbody.useGravity = true;
			child.rigidbody.isKinematic = false;
			child.rigidbody.WakeUp();
			child.rigidbody.AddForce(blastForce);
			//	}
			}
			else
			{
				BlastWall(blastForce, child.gameObject);	
			}
		}
	}
	
	void InitiateActionEffect(Result result)
	{
		modelPart = null;
		breakBox = null;
		propelVector = Vector3.zero;

	

		//Ragdoll entire character if dead
		if(!result.targetCharacter.inPlay)
		{
			Ragdollify(result.targetCharacter.gameObject);
		}
		
		//EffectInfo effectInfo = GetEffectInfo(result.targetCharacter, result.hitLocation);
		
		ScriptModelController targetModelController = result.targetCharacter.
			GetComponentInChildren<ScriptModelController>();

		//For all damage, if limb is broken, spurt blood and dismember. If not, spurt blood only

		//Get attack direction
		rangedAttack = result.actingCharacter.
			GetComponentInChildren<ScriptControllerTargeting>().rangedAttack;
		rangedAttack.Normalize();


		//Debug.Log(result.targetNetHitProfile.head.ToString() + result.targetNetHitProfile.body.ToString() + 
		  //      result.targetNetHitProfile.leftArm.ToString() + result.targetNetHitProfile.rightArm.ToString() + 
		    //  result.targetNetHitProfile.leftLeg.ToString() + result.targetNetHitProfile.rightLeg.ToString());

		//Upon successful attack
		if(result.success)
		{

		//If damage is inflicted on body part and that body part is destroyed
		if(result.targetNetHitProfile.head < 0 && result.targetCharacter.currentHitProfile.head <= 0)
		{
			//Assign execution variables
			modelPart = targetModelController.head;
			breakBox = modelPart.transform.GetChild((int)Mathf.Floor(Random.value * targetModelController.head.transform.childCount)).gameObject;
			propelVector = rangedAttack * -result.targetCharacter.currentHitProfile.head * propelForceConstant;
		}

		if(result.targetNetHitProfile.body < 0 && result.targetCharacter.currentHitProfile.body <= 0)
		{
			modelPart = targetModelController.spine;
			breakBox = modelPart.transform.GetChild((int)Mathf.Floor(Random.value * targetModelController.spine.transform.childCount)).gameObject;
			propelVector = rangedAttack * -result.targetCharacter.currentHitProfile.body * propelForceConstant;
		}

		if(result.targetNetHitProfile.leftArm < 0 && result.targetCharacter.currentHitProfile.leftArm <= 0)
		{
			modelPart = targetModelController.leftArm;
			breakBox = modelPart.transform.GetChild((int)Mathf.Floor(Random.value * targetModelController.leftArm.transform.childCount)).gameObject;
			propelVector = rangedAttack * -result.targetCharacter.currentHitProfile.leftArm * propelForceConstant;
		}

		if(result.targetNetHitProfile.rightArm < 0 && result.targetCharacter.currentHitProfile.rightArm <= 0)
		{
			modelPart = targetModelController.rightArm;
			breakBox = modelPart.transform.GetChild((int)Mathf.Floor(Random.value * targetModelController.rightArm.transform.childCount)).gameObject;
			propelVector = rangedAttack * -result.targetCharacter.currentHitProfile.rightArm * propelForceConstant;
		}

		if(result.targetNetHitProfile.leftLeg < 0 && result.targetCharacter.currentHitProfile.leftLeg <= 0)
		{
			modelPart = targetModelController.leftLeg;
			breakBox = modelPart.transform.GetChild((int)Mathf.Floor(Random.value * targetModelController.leftLeg.transform.childCount)).gameObject;
			propelVector = rangedAttack * -result.targetCharacter.currentHitProfile.leftLeg * propelForceConstant;
		}

		if(result.targetNetHitProfile.rightLeg < 0 && result.targetCharacter.currentHitProfile.rightLeg <= 0)
		{
			modelPart = targetModelController.rightLeg;
			breakBox =  modelPart.transform.GetChild((int)Mathf.Floor(Random.value * targetModelController.rightLeg.transform.childCount)).gameObject;
			propelVector = rangedAttack * -result.targetCharacter.currentHitProfile.rightLeg * propelForceConstant;
		}
		
		//Shoot projectile

		//Fire weapon at target's part if hit, randomly if miss
		if(breakBox)
		{
		result.actingCharacter.GetComponentInChildren<ScriptModelController>().weapon.GetComponent<ScriptWeapon>().SendMessage("GunshotEffect", breakBox);
		//Debug.Log(breakBox.name);
		}
		else
		{
			Debug.Log("breakBox requested but not assigned");
		}
		//	hotSheet.gameObject.GetComponentInChildren<ScriptModelController>().SendMessage("WeaponEffect");




		//Dismember
		if(modelPart && breakBox)
		{
		Ragdollify(modelPart);
		BreakJoints(breakBox);
		Propel (propelVector, modelPart);
		}

		//Spurt blood

		}
		else if(!result.success)
		{
			//Fire weapon in random direction
			result.actingCharacter.GetComponentInChildren<ScriptModelController>().weapon.
				GetComponent<ScriptWeapon>().SendMessage("GunshotEffect");

		}
		else
		{
			Debug.Log ("Something went horribly wrong");
		}

		//Assign model part and break box

		/*
	switch(hotResult.hitLocation)
		{
		case BodyPart.Head:
			modelPart = hotModelController.head;
			breakBox = modelPart.transform.FindChild("headBox1").gameObject;
			break;
		case BodyPart.Body:
			modelPart = hotModelController.spine;
			breakBox = modelPart.transform.FindChild("spineBox4").gameObject;
			break;
		case BodyPart.LeftArm:
			modelPart = hotModelController.leftArm;
			breakBox = modelPart.transform.FindChild("leftArmBox3").gameObject;
			break;
		case BodyPart.RightArm:
			modelPart = hotModelController.rightArm;
			breakBox = modelPart.transform.FindChild("rightArmBox3").gameObject;
			break;
		case BodyPart.LeftLeg:
			modelPart = hotModelController.leftLeg;
			breakBox = modelPart.transform.FindChild("leftLegBox3").gameObject;
			break;
		case BodyPart.RightLeg:
			modelPart = hotModelController.rightLeg;
			breakBox = modelPart.transform.FindChild("rightLegBox3").gameObject;
			break;
		default:
			Debug.Log ("Invalid Body Part: " + hotResult.hitLocation.ToString());
			break;
		}
		
		//Debug.Log (modelPart.ToString() + breakBox.ToString());



		Vector3 rangedAttack = hotResult.actingCharacter.
			GetComponentInChildren<ScriptControllerTargeting>().rangedAttack;
		
		if(hotResult.hitLocation == BodyPart.Head)
		{
					if(hotResult.targetCharacter.currentHeadHP <= 0)
			{
			modelPart.SendMessage("HeadExplode", headExplodeForce);	
		}

		if(hotResult.hitLocation == BodyPart.Body)
			{
				if(hotResult.targetCharacter.currentBodyHP <= 0)
				{

				}
			}
		Ragdollify(modelPart);
		BreakJoints(breakBox);
		Propel(rangedAttack * 200, hotResult.targetCharacter.gameObject);
		//breakBox.rigidbody.AddForce(rangedAttack * 500);
		//Debug.Log ("Broke the " + hotResult.hitLocation);
*/
	}
	
	void BreakJoints(GameObject breakBox)
	{
		foreach(FixedJoint hotJoint in breakBox.GetComponents<FixedJoint>())
		{
			Destroy (hotJoint);	
		}
			
	}
	
}
