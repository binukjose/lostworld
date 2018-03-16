using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sharkMove : MonoBehaviour {
	 
	//public 

	public GameObject ArcoreCamera;
	public GameObject waterBody;
	//public GameObject target = null;
	public Vector3 SharkTarget;
	public int shark_swim_radius_x ;
	public int shark_swim_radius_z ;
	public GameObject Hit_wound;


	//private
	//private  GameObject m_target;
	private float waterSurfaceHeight;
	private float MobCurrentSpeed = 1f;
	private float distance;
	private int my_wound_count = 0;
 
	Vector3 getSharkTarget(){
		Vector3 spos;
		Random.InitState(System.DateTime.Now.Millisecond);
		spos= new Vector3(Random.Range
							(ArcoreCamera.transform.position.x - shark_swim_radius_x , 
				         	ArcoreCamera.transform.position.x + shark_swim_radius_x), 
							waterSurfaceHeight  ,  
				         	Random.Range(
				         	ArcoreCamera.transform.position.z - shark_swim_radius_z, 
							ArcoreCamera.transform.position.z + shark_swim_radius_z));

		//BINU if (spos.y > waterSurfaceHeight) spos.y = waterSurfaceHeight;
		//m_target.transform.SetPositionAndRotation (spos, Quaternion.identity);
		return spos;
	}
	void Start () {
		waterSurfaceHeight = waterBody.transform.position.y -1;  
		SharkTarget = getSharkTarget ();
		//m_target = Instantiate (target);
	}
	
	// Update is called once per frame
	void Update () {
		//if (m_target == null) {
		//	m_target = Instantiate (target);
		//	Debug.Log ("target instantiated in update");
		//}
		if (Input.touchCount > 1)
		{
			//angular_drag = angular_drag + 0.3f;
		}
		distance = Vector3.Distance(SharkTarget, transform.position);
	
		// If we are arriving at target, pick a new target for our shark and reset the last attack if there was one
		if (distance <= 2f || distance > Mathf.Abs(shark_swim_radius_z) )
		{
			SharkTarget = getSharkTarget ();
			//startTime = Time.time;
		}
			
		Vector3 relativePos =   SharkTarget- transform.position;
		transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(relativePos),Time.deltaTime*2);

		// shark translate in forward direction.
		transform.Translate (Vector3.forward * Time.deltaTime * MobCurrentSpeed);

	}


	void OnCollisionEnter (Collision col)
	{

		try {
			//Destroy(col.gameObject);
			if(col.gameObject.tag == "dynamic")
			{	//col.gameObject.GetComponent<AudioSource> ().Play ();
				var hit_fish_wound = Instantiate (Hit_wound, col.transform.position,Quaternion.identity);
				Vector3 hit_point = col.contacts[0].point;
				for (int i = 0 ; i< col.contacts.Length;  i++) {
					Debug.Log (" OnCollisionEnter hit  "  +  col.contacts[i].point);
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

				//too much wound die 
				if(my_wound_count ++ >5 ) {
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
