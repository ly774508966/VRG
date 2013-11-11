using UnityEngine;
using System.Collections;

public class EffectInfo
{
	//public ScriptCharacterSheet owner;
	public int hp;
	//public GameObject modelBodyPart;
	public GameObject breakBox;
	public Vector3 attackVector;
	public Vector3 attackForce;
}

public class ScriptPhysicsController : MonoBehaviour {
	
	public float wallJointStrength = 1.0F;
	public float wallThresholdVelocity = 1.0F;
	public float headExplodeForce = 1000;
	public float propelForce = 10000F;
	public GameObject panelContainer;
	public GameObject debug;
	
	
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
	
	void Propel(Vector3 propelDirection, GameObject targetCharacter){
		foreach(Transform child in targetCharacter.transform){
			if(child.rigidbody != null){
				child.rigidbody.AddForce( propelDirection * propelForce);	
				} else {
			Propel(propelDirection, child.gameObject);
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
	
	void BlastWall(Vector3 blastForce, GameObject hotWall)
	{
		foreach(Transform panel in hotWall.transform)
		{
			if(Random.value <= 0.75)
			{
			panel.gameObject.AddComponent<Rigidbody>();
			panel.rigidbody.mass = 1;
			panel.rigidbody.drag = 0;
			panel.rigidbody.angularDrag = 0.05F;
			panel.rigidbody.useGravity = true;
			panel.rigidbody.isKinematic = false;
			panel.rigidbody.WakeUp();
			panel.rigidbody.AddForce(blastForce);
			}
		}
	}
	
	void InitiateActionEffect(Result hotResult)
	{
		//Ragdoll entire character if dead
		if(!hotResult.targetCharacter.inPlay)
		{
			Ragdollify(hotResult.targetCharacter.gameObject);
		}
		
		EffectInfo effectInfo = GetEffectInfo(hotResult.targetCharacter, hotResult.hitLocation);
		
		
		//Ragdoll limb if applicable and assign hit part of model
		
		/*
		GameObject targetPart;
		switch(hotResult.hitLocation)
		{
		case BodyPart.Head:
			targetPart = hotResult.targetCharacter.
				GetComponentInChildren<ScriptModelController>().head;
			if(hotResult.targetCharacter.currentHeadHP <= 0)
			{
				//Destroy limb
				targetPart.SendMessage("HeadExplode", headExplodeForce);
				//Ragdollify(hotResult.targetCharacter);
			}
		case BodyPart.Body:
			targetPart = hotResult.targetCharacter.
				GetComponentInChildren<ScriptModelController>().body;
			if(hotResult.targetCharacter.currentBodyHP <= 0)
			{
				//Destroy limb
				DestroyLimb();
				//targetPart.SendMessage("HeadExplode", headExplodeForce);
				//Ragdollify(hotResult.targetCharacter);
			}
		break;
		default:
		Debug.Log ("Invalid Body Part: " + hotResult.hitLocation.ToString());
			break;
		}
		*/
		//if(hotResult.targetCharacter.currentHeadHP <= 0 || hotResult.targetCharacter.currentBodyHP <= 0)
		//{
		//Ragdollify(hotResult.targetCharacter.gameObject);
			
		//}
	}
	
	EffectInfo GetEffectInfo(ScriptCharacterSheet character, BodyPart bodyPart)
	{
		EffectInfo effectInfo = new EffectInfo();
		GameObject modelPart;
	switch(bodyPart)
		{
		case BodyPart.Head:
			modelPart = character.GetComponentInChildren<ScriptModelController>().head;
			effectInfo.hp = character.currentHeadHP;
			//effectInfo.breakBox = modelPart.transform[0];
				//(int)Mathf.Floor(Random.value*modelPart.transform.childCount)
		case BodyPart.Body:
			return character.GetComponentInChildren<ScriptModelController>().body;
		case BodyPart.LeftArm:
			return character.GetComponentInChildren<ScriptModelController>().leftArm;
		case BodyPart.RightArm:
			return character.GetComponentInChildren<ScriptModelController>().rightArm;
		case BodyPart.LeftLeg:
			return character.GetComponentInChildren<ScriptModelController>().leftLeg;
		case BodyPart.RightLeg:
			return character.GetComponentInChildren<ScriptModelController>().rightLeg;
		default:
			Debug.Log ("Invalid Body Part: " + hotResult.hitLocation.ToString());
			return null;
		}
		
		
		
	}
}
