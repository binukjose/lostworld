using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StingRayMove : MonoBehaviour {

 

		//public 
		public GameObject ArcoreCamera;
		public GameObject waterBody;

		public Vector3 mSharkTarget;
		public int shark_swim_radius_x ;
		public int shark_swim_radius_z ;
		public GameObject Hit_wound;
		public  float MobCurrentSpeed = 2f;
		public  float angularVelocity = .01f;

		//private
		private float waterSurfaceHeight;   

		private float distance;
		private int my_wound_count = 0;
		private int health = 100;


		enum SharkState {Idle=1,Move,Attack,JumpStart,Jump,JumpSplash,Dead,Pause};  
		SharkState mSharkState = SharkState.Move;
		SharkState mSharkPrevState = SharkState.Move;

		float getSharkTargetX(){
			return Random.Range
			(ArcoreCamera.transform.position.x - shark_swim_radius_x, 
				ArcoreCamera.transform.position.x + shark_swim_radius_x);
		}

		float getSharkTargetZ(){
			return Random.Range (
			ArcoreCamera.transform.position.z - shark_swim_radius_z, 
			ArcoreCamera.transform.position.z + shark_swim_radius_z);
		}

		void SpearHit(Vector3 pos) {
			Debug.Log ("SpearHit inside shark  :" + pos);
			var hit_fish_wound = Instantiate (Hit_wound, pos,Quaternion.identity);
			hit_fish_wound.transform.SetParent (transform);
			my_wound_count ++;
			health = health - 20;

		}
		void GlobalMessage (string message) {
			Debug.Log ("GlobalMessage :" + message);
			if (message == "pause") {
				mSharkPrevState = mSharkState;
				mSharkState = SharkState.Pause;
			} else if (message == "play") {
				if (mSharkState == SharkState.Pause) {
					mSharkState = mSharkPrevState;
				}
			} 
		}

		Vector3 getSharkTarget(){
			Vector3 spos;
			Random.InitState(System.DateTime.Now.Millisecond);
			spos = new Vector3 (getSharkTargetX(), 
				waterSurfaceHeight,  
				getSharkTargetZ());
			return spos;
		}

		void Start () {
			waterSurfaceHeight = waterBody.transform.position.y -1;  
			mSharkTarget = getSharkTarget ();

		}

		// Update is called once per frame
		void Update () {

			Debug.Log ("Move : mSharkState " + mSharkState + " for " + tag);//+ " test_move_count" + test_move_count);
			//test_move_count++;
			if (mSharkState == SharkState.Pause) {
				//Dont move .
			}
			else if (mSharkState == SharkState.Move) {
				distance = Vector3.Distance (mSharkTarget, transform.position);
				if (distance <= 2f ) {
					mSharkTarget = getSharkTarget();
				}	
			/*	else if (shark_swim_radius_z - Mathf.Abs (transform.position.z) < 0){
					mSharkTarget.z = getSharkTargetZ ();
				}
				else if (shark_swim_radius_x - Mathf.Abs (transform.position.x) < 0){
					mSharkTarget.x = getSharkTargetX ();
				}
				*/
				Vector3 relativePos = mSharkTarget - transform.position;
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (relativePos), Time.deltaTime * angularVelocity);
				transform.Translate (Vector3.forward * Time.deltaTime * MobCurrentSpeed);
				if (my_wound_count > 0) {
					//mSharkState = SharkState.Attack;
					mSharkState = SharkState.JumpStart;
				}
			} else if (mSharkState == SharkState.Attack) {


				mSharkTarget = new Vector3 (transform.position.x,
					waterSurfaceHeight,
					transform.position.z);
				Vector3 relativePos = mSharkTarget - transform.position;
				distance = Vector3.Distance (mSharkTarget, transform.position);
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (relativePos), Time.deltaTime * angularVelocity*5);
				// shark translate in forward direction.
				transform.Translate (Vector3.forward * Time.deltaTime * MobCurrentSpeed * 2);


				//Debug 
				//mSharkState = SharkState.JumpStart;
				//end debug 

				if (distance <= 5f) {
					mSharkState = SharkState.JumpStart;
				}
			} else if (mSharkState == SharkState.JumpStart) {
				mSharkTarget = new Vector3 (transform.position.x,
					transform.position.y + 5f,
					transform.position.z);
				distance = Vector3.Distance (mSharkTarget, transform.position);
				transform.Translate (Vector3.forward * Time.deltaTime * MobCurrentSpeed*3 );
				if ( (transform.position.y > (transform.position.y +1f)) ) {
					mSharkState = SharkState.Dead;
				}
			} else if (mSharkState == SharkState.Dead) {
				mSharkTarget = new Vector3 (10f,
					waterSurfaceHeight,
					10f);

				transform.Translate (mSharkTarget);
				mSharkState = SharkState.Move;
				my_wound_count = 0;
				health = 100;
			}
		}
	}

