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
	public int octopus_idleCount = 100 ;
	public GameObject Hit_wound;
	public  float MobCurrentSpeed = 2f;
	public  float angularVelocity = .01f;

	//private
	private float waterSurfaceHeight;   

	private float distance;
	private int my_wound_count = 0;
	private int health = 100;
	private int idleCount = 10;


	enum octopusState {Idle=1,Move,Attack,JumpStart,Jump,JumpSplash,Dead,Pause};  
	octopusState mOctopusState = octopusState.Move;
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
			moctopusPrevState = mOctopusState;
			mOctopusState = octopusState.Pause;
		} else if (message == "play") {
			if (mOctopusState == octopusState.Pause) {
				mOctopusState = moctopusPrevState;
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
		mOctopusState = octopusState.Move;
		idleCount = octopus_idleCount;

	}

	// Update is called once per frame
	void Update () {

		Debug.Log ("Move : mOctopusState " + mOctopusState + " for " + tag);//+ " test_move_count" + test_move_count);
		//test_move_count++;
		if (mOctopusState == octopusState.Pause) {
			//Dont move .
		} else if (mOctopusState == octopusState.Idle) {

			if (my_wound_count > 0) {
				mOctopusState = octopusState.Attack;

			}

			idleCount--;

			if (idleCount <= 0) {
				moctopusTarget = getoctopusTarget ();
				mOctopusState = octopusState.Move;
			}
		} else if (mOctopusState == octopusState.Move) {

			distance = Vector3.Distance (moctopusTarget, transform.position);
			if (distance <= 1f) {
				idleCount = octopus_idleCount;
				mOctopusState = octopusState.Idle;
			} else if (distance > octopus_swim_radius_x) {
				moctopusTarget = getoctopusTarget ();
			}

			Vector3 relativePos = moctopusTarget - transform.position;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (relativePos), Time.deltaTime * angularVelocity);
			transform.Translate (Vector3.forward * Time.deltaTime * MobCurrentSpeed);
			if (my_wound_count > 0) {
				mOctopusState = octopusState.Attack;
				 
			}
		
		} else if (mOctopusState == octopusState.Attack) {
			Vector3 scaling = new Vector3 (0.5F, 0.5f, 0.5f);
			transform.localScale += scaling;

			if (my_wound_count > 3) {
				mOctopusState = octopusState.JumpStart;

			}

			/*moctopusTarget = new Vector3 (transform.position.x,
				transform.position.y + 5f,
				transform.position.z);
			distance = Vector3.Distance (moctopusTarget, transform.position);
			transform.Translate (Vector3.forward * Time.deltaTime * MobCurrentSpeed * 3);
			if ((transform.position.y > (transform.position.y + 1f))) {
				mOctopusState = octopusState.Dead;
			}
			*/

		} else if ( mOctopusState == octopusState.JumpStart) {

			MobCurrentSpeed = 20f;

			moctopusTarget = new Vector3 (ArcoreCamera.transform.position.x,
				ArcoreCamera.transform.position.y+1,
				ArcoreCamera.transform.position.z);
			Vector3 relativePos = moctopusTarget - transform.position;
			distance = Vector3.Distance (moctopusTarget, transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (relativePos), Time.deltaTime * 50);
			// octopus translate in forward direction.
			transform.Translate (Vector3.forward * Time.deltaTime * MobCurrentSpeed );


			//Debug 
			//mOctopusState = octopusState.JumpStart;
			//end debug 

			if (distance <= 1f) {
				mOctopusState = octopusState.Dead;
			}
		}


		else if (mOctopusState == octopusState.Dead) {
			moctopusTarget = new Vector3 (10f,
				waterSurfaceHeight,
				10f);

			transform.Translate (moctopusTarget);
			mOctopusState = octopusState.Move;
			my_wound_count = 0;
			health = 100;
		}
	}
}

