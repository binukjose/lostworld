using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusMove : MonoBehaviour {

	//public 
	public GameObject ArcoreCamera;
	public GameObject waterBody;

	public Vector3 moctopusTarget;
	public int octopus_swim_radius_x ;
	public int octopus_swim_radius_z ;
	public GameObject Hit_wound;
	public  float MobCurrentSpeed = 2f;
	public  float angularVelocity = .01f;

	//private
	private float waterSurfaceHeight;   

	private float distance;
	private int my_wound_count = 0;
	private int health = 100;


	enum octopusState {Idle=1,Move,Attack,JumpStart,Jump,JumpSplash,Dead,Pause};  
	octopusState moctopusState = octopusState.Move;
	octopusState moctopusPrevState = octopusState.Move;

	float getoctopusTargetX(){
		return Random.Range
			(ArcoreCamera.transform.position.x - octopus_swim_radius_x, 
				ArcoreCamera.transform.position.x + octopus_swim_radius_x);
	}

	float getoctopusTargetZ(){
		return Random.Range (
			ArcoreCamera.transform.position.z - octopus_swim_radius_z, 
			ArcoreCamera.transform.position.z + octopus_swim_radius_z);
	}

	void SpearHit(Vector3 pos) {
		Debug.Log ("SpearHit inside octopus  :" + pos);
		var hit_fish_wound = Instantiate (Hit_wound, pos,Quaternion.identity);
		hit_fish_wound.transform.SetParent (transform);
		my_wound_count ++;
		health = health - 20;

	}
	void GlobalMessage (string message) {
		Debug.Log ("GlobalMessage :" + message);
		if (message == "pause") {
			moctopusPrevState = moctopusState;
			moctopusState = octopusState.Pause;
		} else if (message == "play") {
			if (moctopusState == octopusState.Pause) {
				moctopusState = moctopusPrevState;
			}
		} 
	}

	Vector3 getoctopusTarget(){
		Vector3 spos;
		Random.InitState(System.DateTime.Now.Millisecond);
		spos = new Vector3 (getoctopusTargetX(), 
			waterSurfaceHeight,  
			getoctopusTargetZ());
		return spos;
	}

	void Start () {
		waterSurfaceHeight = waterBody.transform.position.y;  
		moctopusTarget = getoctopusTarget ();

	}

	// Update is called once per frame
	void Update () {

		Debug.Log ("Move : moctopusState " + moctopusState + " for " + tag);//+ " test_move_count" + test_move_count);
		//test_move_count++;
		if (moctopusState == octopusState.Pause) {
			//Dont move .
		}
		else if (moctopusState == octopusState.Move) {

			distance = Vector3.Distance (moctopusTarget, transform.position);
			if (distance <= 5f  ) {
				moctopusTarget = getoctopusTarget();
			}	
		/*	else if (octopus_swim_radius_z - Mathf.Abs (transform.position.z) < 0){
				moctopusTarget.z = getoctopusTargetZ ();
			}
			else if (octopus_swim_radius_x - Mathf.Abs (transform.position.x) < 0){
				moctopusTarget.x = getoctopusTargetX ();
			}
			*/
			Vector3 relativePos = moctopusTarget - transform.position;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (relativePos), Time.deltaTime * angularVelocity);
			transform.Translate (Vector3.forward * Time.deltaTime * MobCurrentSpeed);
			if (my_wound_count > 0) {
				//moctopusState = octopusState.Attack;
				moctopusState = octopusState.JumpStart;
			}
		} else if (moctopusState == octopusState.Attack) {


			moctopusTarget = new Vector3 (transform.position.x,
				waterSurfaceHeight,
				transform.position.z);
			Vector3 relativePos = moctopusTarget - transform.position;
			distance = Vector3.Distance (moctopusTarget, transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (relativePos), Time.deltaTime * angularVelocity*5);
			// octopus translate in forward direction.
			transform.Translate (Vector3.forward * Time.deltaTime * MobCurrentSpeed * 2);


			//Debug 
			//moctopusState = octopusState.JumpStart;
			//end debug 

			if (distance <= 5f) {
				moctopusState = octopusState.JumpStart;
			}
		} else if (moctopusState == octopusState.JumpStart) {
			moctopusTarget = new Vector3 (transform.position.x,
				transform.position.y + 5f,
				transform.position.z);
			distance = Vector3.Distance (moctopusTarget, transform.position);
			transform.Translate (Vector3.forward * Time.deltaTime * MobCurrentSpeed*3 );
			if ( (transform.position.y > (transform.position.y +1f)) ) {
				moctopusState = octopusState.Dead;
			}
		} else if (moctopusState == octopusState.Dead) {
			moctopusTarget = new Vector3 (10f,
				waterSurfaceHeight,
				10f);

			transform.Translate (moctopusTarget);
			moctopusState = octopusState.Move;
			my_wound_count = 0;
			health = 100;
		}
	}
}

