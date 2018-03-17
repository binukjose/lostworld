using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sharkMove : MonoBehaviour {
	 
	//public 
	public GameObject ArcoreCamera;
	public GameObject waterBody;
	public GameObject target ;
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

	private int test_move_count = 0;
	enum SharkState {Idle=1,Move,Attack,JumpStart,Jump,JumpSplash,Dead};  
	SharkState mSharkState = SharkState.Move;
 
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
		//m_target = Instantiate (target);
	}
	
	// Update is called once per frame
	void Update () {

		Debug.Log ("Move : mSharkState " +  mSharkState + " test_move_count" + test_move_count);
		test_move_count++;
		if (mSharkState == SharkState.Move) {
			distance = Vector3.Distance (mSharkTarget, transform.position);
			if (distance <= 2f ) {
				mSharkTarget = getSharkTarget();
			}	
			else if (shark_swim_radius_z - Mathf.Abs (transform.position.z) < 0){
				mSharkTarget.z = getSharkTargetZ ();
			}
			else if (shark_swim_radius_x - Mathf.Abs (transform.position.x) < 0){
				mSharkTarget.x = getSharkTargetX ();
			}
			Vector3 relativePos = mSharkTarget - transform.position;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (relativePos), Time.deltaTime * angularVelocity);
			transform.Translate (Vector3.forward * Time.deltaTime * MobCurrentSpeed);
		} else if (mSharkState == SharkState.Attack) {
			mSharkTarget = new Vector3 (ArcoreCamera.transform.position.x,
				waterSurfaceHeight,
				ArcoreCamera.transform.position.z);
			Vector3 relativePos = mSharkTarget - transform.position;
			distance = Vector3.Distance (mSharkTarget, transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (relativePos), Time.deltaTime * angularVelocity);
			// shark translate in forward direction.
			transform.Translate (Vector3.forward * Time.deltaTime * MobCurrentSpeed * 5);

			if (distance <= 3f) {
				mSharkState = SharkState.Jump;
			}
		} else if (mSharkState == SharkState.JumpStart) {
			mSharkTarget = new Vector3 (ArcoreCamera.transform.position.x,
				ArcoreCamera.transform.position.y + 1f,
				ArcoreCamera.transform.position.z);
			distance = Vector3.Distance (mSharkTarget, transform.position);
			transform.Translate (Vector3.forward * Time.deltaTime * MobCurrentSpeed * (distance / 2));
			if (distance <= .3f) {
				mSharkState = SharkState.Dead;
			}
		} else if (mSharkState == SharkState.Dead) {
			mSharkTarget = new Vector3 (ArcoreCamera.transform.position.x,
				waterSurfaceHeight-3f,
				ArcoreCamera.transform.position.z);
			distance = Vector3.Distance (mSharkTarget, transform.position);
			transform.Translate (Vector3.forward * Time.deltaTime * MobCurrentSpeed * (distance / 2));
			mSharkState = SharkState.Move;
		}
	}


	void OnCollisionEnter (Collision col)
	{
		Debug.Log (" CollisionEnter Parent Tag   "  + col.transform.transform.tag);
		try {
			//Destroy(col.gameObject);
			if(col.gameObject.tag == "dynamic")
			{	//col.gameObject.GetComponent<AudioSource> ().Play ();
				var hit_fish_wound = Instantiate (Hit_wound, col.transform.position,Quaternion.identity);
				Vector3 hit_point = col.contacts[0].point;
				for (int i = 0 ; i< col.contacts.Length;  i++) {
					Debug.Log (" Collision Enter hit  "  +  col.contacts[i].point);
				}

				Debug.Log (" OnCollisionEnter Parent Tag   "  + this.gameObject.transform.tag);
				//assign wound to fish 
				//Vector3 pos_diff = hit_fish_wound.transform.position - col.gameObject.transform.position;
				//Quaternion rot_diff =Quaternion.FromToRotation (col.gameObject.transform.position,hit_fish_wound.transform.position);
				hit_fish_wound.transform.SetParent(this.gameObject.transform); 
				//hit_fish_wound.transform.localPosition=pos_diff;
				//hit_fish_wound.transform.localRotation=rot_diff;

				//assign spear to fish
				//pos_diff = transform.position - col.gameObject.transform.position;
				//rot_diff =Quaternion.FromToRotation (col.gameObject.transform.position,transform.position);
				col.gameObject.transform.SetParent(this.gameObject.transform); 

				//transform.localPosition=pos_diff;
				//transform.localRotation=rot_diff;  

				//move_count = 0;
				//hit_fish_surface = true;

				my_wound_count ++;
				mSharkState = SharkState.Attack;
				//too much wound die 
				if(my_wound_count >5 ) {
					mSharkState = SharkState.Dead;
					Destroy(gameObject,3);
				}


			}
			else if( col.gameObject ){
				Debug.Log (" OnCollisionEnter "  +  col.gameObject.tag );
			}
			else {
				Debug.Log (" OnCollisionEnter No Audio to play " );
			}

		}
		catch (System.Exception e)
		{
			Debug.Log ("OnCollisionEnter " + e);
		}


	}
}
