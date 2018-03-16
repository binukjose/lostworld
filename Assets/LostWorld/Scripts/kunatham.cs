using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class kunatham : MonoBehaviour {

	//static 
	private static int  MAX_MOVE = 50;

	//public

	public Camera arcamera;
	public GameObject waterSurface;

	public GameObject Splash;

	//private 
	public bool kuntham_throwing;
	private int move_count;
	private bool hit_water_surface =false;
	private bool hit_shark_surface =false;
	 

	void Start () {
		if (tag.Equals ("dynamic")) {
			kuntham_throwing = true;
			move_count = MAX_MOVE;	 
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
		if (tag.Equals ("dynamic") && !hit_shark_surface ) {

			if (this.transform.parent &&  this.transform.parent.tag.ToString().StartsWith("SharkCube")){
				transform.Translate (Vector3.forward*0.2f);// IMP:move the arrow a bit more into the shark.
				Debug.Log (" Kuntham SharkCube HIT "  );
				hit_shark_surface = true;
			}
			else if (kuntham_throwing && move_count > 0  ) {
				//Debug.Log (" Kuntham kuntham_throwing && move_count > 0"  );
					transform.Translate (Vector3.forward * (move_count * 2 / MAX_MOVE));//* Time.deltaTime * 2.0f);
			
				if (transform.transform.position.y < waterSurface.transform.position.y) {
					if (hit_water_surface == false) { //spalsh sound and splash 
						GetComponent<AudioSource> ().Play ();
						hit_water_surface = true;
						var splash = Instantiate (Splash, transform.transform.position,Quaternion.identity);
						Destroy(splash,2);
					}
				}
				move_count--;
		 
			} else if (kuntham_throwing && move_count <= 0 ) { 
				//Debug.Log (" Kuntham kuntham_throwing && move_count <= 0"  );
				kuntham_throwing = false;
				Destroy (gameObject, 2);
				Destroy (this);
			}	
		}
	}//end Update 


}
