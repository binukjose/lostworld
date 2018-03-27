using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class kuntham : MonoBehaviour {

	//static 
	private static int  MAX_MOVE = 50;
	private enum State { init, moving, hit, inside,trash };
	private State mKunthamState = State.init;
	//public

	public Camera arcamera;
	public GameObject waterSurface;
	public GameObject Splash;
	public GameObject Wound;
	//public GameObject stuck_spear;
	public GameObject Explosive;
	//private 
	private int move_count;
	private bool hit_water_surface =false;
	private bool hit_shark_surface =false;
	 

	void Start () {
		if (tag.Equals ("dynamic")) {
			move_count = MAX_MOVE;	
			mKunthamState = State.moving;
			Vector3 startpos = arcamera.transform.position;
			startpos.y--;
			transform.SetPositionAndRotation (startpos, arcamera.transform.rotation);

		}
		/*RaycastHit hit;
		Ray ray = arcamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		if (Physics.Raycast (ray, out hit)) {
			Transform objectHit = hit.transform;
			Debug.Log (" Ray I'm looking at " + hit.transform.name + " pos:"+ hit.transform.position);
		} else {
			 
			Debug.Log(" Ray I'm looking at nothing!");
		}	
		Debug.Log(" Ray pos kuntham:" +transform.position + " Camera" +  arcamera.transform.position);

		*/
	}
	
	// Update is called once per frame
	void Update () {
		if (tag.Equals ("dynamic") && mKunthamState != State.inside ) {
			
			if ( mKunthamState == State.inside) {
				  
				//DO nothing let the kuntham be inside 
			}
			else if (mKunthamState == State.moving && move_count > 0  ) {
				transform.Translate (Vector3.forward * (move_count * 2 / MAX_MOVE));
				if (transform.transform.position.y < waterSurface.transform.position.y) {
					if (hit_water_surface == false) { //spalsh sound and splash 
						GetComponent<AudioSource> ().Play ();
						hit_water_surface = true;
						var splash = Instantiate (Splash, transform.transform.position,Quaternion.identity);
						Destroy(splash,2);
					}
				}
				move_count--;
		 
			} else if (mKunthamState == State.moving && move_count <= 0 ) { 
				mKunthamState = State.trash;
				Destroy (gameObject, 2);
				Destroy (this);
			}	
		}
	}//end Update 


	void OnCollisionEnter (Collision col)
	{
		if (mKunthamState != State.hit) { //if kuntham is inside the shark , it will be continues collision 
			
			Debug.Log (" CollisionEnter Parent Tag inside Kuntham  " + col.transform.transform.tag +
			"  movecount" + move_count);
			//HingeJoint hinge = gameObject.GetComponentInParent(typeof(HingeJoint)) as HingeJoint;

			ContactPoint hitpoint = col.contacts [0];
			col.transform.SendMessageUpwards ("SpearHit", hitpoint.point);

			if ((col.transform.tag.ToString ().StartsWith ("GWSharkStatic")) ||
			   (col.transform.tag.ToString ().StartsWith ("StingRayStatic"))) {
				transform.SetParent (transform);//set kuntham as child of shark
				hit_shark_surface = true;
				mKunthamState = State.hit;
				//After hit remove collider to avoid further collision 
				Collider collider = GetComponent <Collider>();
				DestroyImmediate(collider);
				Debug.Log (" CollisionEnter  Kuntham  destroied ");
					
			} else if (col.transform.tag.ToString ().StartsWith ("Mine")) {
				Debug.Log (" CollisionEnter Hit mine  ");
			
				col.transform.gameObject.tag = "Exploded";
				Destroy (gameObject);
				Debug.Log (" CollisionEnter Mine Destroyed    ");
			}else if (col.transform.tag.ToString ().StartsWith ("Barell")) {
				Debug.Log (" CollisionEnter Hit Barell  ");

				col.transform.gameObject.tag = "Hit";
				Destroy (gameObject);
				Debug.Log (" CollisionEnter Barell Hit , kuntham destroyed     ");
			}


		}

		/*
		Transform t = col.transform;
		while (t.parent != null) {
			Debug.Log ("  CollisionEnter t.parent.tag " + t.parent.tag);
			if (t.tag.ToString ().StartsWith ("SharkCube")) {
				transform.SetParent(t.transform);//set kuntham as child of shark
				var k = Instantiate(stuck_spear,t.transform);
				k.transform.SetPositionAndRotation (transform.position, transform.rotation);
				hit_shark_surface = true;
				mKunthamState = State.hit;
				Destroy (gameObject);
				break;
			}
			t = t.parent.transform;
		}

*/
		/*
		if (col.transform.transform.tag.ToString().StartsWith("SharkCube")){
			transform.Translate (Vector3.forward*0.2f);// IMP:move the arrow a bit more into the shark.


			
		 
			//var hit_fish_wound = Instantiate (Wound, hitpoint.point,Quaternion.identity);
			//hit_fish_wound.transform.SetParent(col.transform);//set wound as child of shark 
			//

			col.gameObject.SendMessage ("GlobalMessage","hit");

	
		}*/

	}

}
