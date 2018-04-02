using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour {

	//public variables
	public GameObject SharkGameObj; 
	public GameObject StingRayGameObj; 
	public GameObject HammerHeadGameObj;
	public GameObject OctopusGameObj;
	public Camera ArcoreCamera;
	public GameObject gun;
	public GameObject kuntham;
	public GameObject water;
	//public GameObject Barrel;
	public int shark_swim_radius_x ;
	public int shark_swim_radius_z ;
	public int NUM_SHARKS  ;

	//private 
	private List <GameObject> mSharks =new List<GameObject>();
	private List <GameObject> mRays =new List<GameObject>();
	private List <GameObject> mOctopuses =new List<GameObject>();
	private List <GameObject> mHammerHeads =new List<GameObject>();

	private int mSharkCount;
	private int mRayCount;
	private int mHammerHeadCount;
	private int mOctopusCount;

	private bool fire = false;
	private bool gamePause = false;
	private float waterSurfaceHeight;

	void PlayPause()
	{
		string step = "play";
		if(gamePause) {
			step = "play";
			gamePause = false;
		} else {
			step = "pause";
			gamePause = true;
		}
		for ( int i=0; i< mSharks.Count; i++) {
			mSharks[i].SendMessage("GlobalMessage", step);
		} 
		for ( int i=0; i< mRays.Count; i++) {
			mRays[i].SendMessage("GlobalMessage", step);
		}
		for ( int i=0; i< mOctopuses.Count; i++) {
			mOctopuses[i].SendMessage("GlobalMessage", step);
		}
		for ( int i=0; i< mHammerHeads.Count; i++) {
			mHammerHeads[i].SendMessage("GlobalMessage", step);
		}
		SharkGameObj.SendMessage("GlobalMessage", step);
		StingRayGameObj.SendMessage("GlobalMessage", step);
		HammerHeadGameObj.SendMessage("GlobalMessage", step);
		OctopusGameObj.SendMessage("GlobalMessage", step);

	}

	GameObject AddAShark(GameObject Master)
	{ 
		GameObject ret;
		Random.InitState(System.DateTime.Now.Millisecond);
		Vector3 spos= new Vector3(Random.Range
			(transform.position.x - shark_swim_radius_x , 
				transform.position.x + shark_swim_radius_x), 
			waterSurfaceHeight  ,   
			Random.Range(
				transform.position.z - shark_swim_radius_z, 
				transform.position.z + shark_swim_radius_z));
	 
			ret =  Instantiate (Master ,spos,Quaternion.identity) ;
		return ret;
	}

	void Start () {
		//Set the height of water surface
		 waterSurfaceHeight = water.transform.position.y -1;
		 SharkGameObj.GetComponent<Animation>().Play("GW_Move");

	}
	

	void Update () {


		Debug.Log ("NUM_SHARKS" +NUM_SHARKS + "shark list count  " + mSharks.Count + "Shar count" + mSharkCount);
		if (mSharkCount < NUM_SHARKS) {
			mSharks.Add(AddAShark (SharkGameObj));
			mSharkCount++;
			Debug.Log ("shark list count  " + mSharks.Count + "Shar count" + mSharkCount);
		}
		if (mRayCount < NUM_SHARKS) {
			mRays.Add(AddAShark (StingRayGameObj));
			mRayCount++;
			Debug.Log ("Ray list count  " + mRays.Count + "Shar count" + mRayCount);
		}

		if ( mHammerHeadCount < NUM_SHARKS) {
			mHammerHeads.Add(AddAShark (HammerHeadGameObj));
			mHammerHeadCount++;
			 
		}
		if ( mOctopusCount < NUM_SHARKS) {
			mOctopuses.Add(AddAShark (OctopusGameObj));
			mOctopusCount++;

		}



		/*Set the gun position */
		Vector3 startpos = transform.position;
		startpos.y = startpos.y-0.5f;
		startpos.z = startpos.z + 1;

		gun.transform.SetPositionAndRotation (startpos, transform.rotation);

		/* Make sure the barrels are floating */
		//startpos = Barrel.transform.position;
		//startpos.y = water.transform.position.y;
		//Barrel.transform.position = startpos;

		/*Set the water y-position , to avoid going inside the water this is not working.
		if ((transform.position.y - water.transform.position.y) < 1) {
			startpos = water.transform.position;
			startpos.y = transform.position.y - 1;
			water.transform.position = startpos;	
		}
		*/
		/*Set the water position , to avoid running out of water*/
		float distance = Vector3.Distance (transform.position, water.transform.position);
		if (distance > 20) {
			startpos = transform.position;
			startpos.y = water.transform.position.y;
			water.transform.position = startpos;
			Debug.Log ("Main: water position changes to " + startpos);

		}

		/* Need to trigger a spear shot if button is pressed */
		if (fire) {
			try {
				startpos = kuntham.transform.position;
				var x = Instantiate (kuntham, startpos, transform.rotation);
				x.tag="dynamic"; /* only dymanic spears get destroyed*/
				gun.GetComponent<Animation>().Play();
			}
			catch (System.Exception e)
			{
				Debug.Log ("Main: Kuntham creation failed " + e);
			}
			fire = false;
		}
		/* TODO: needs to avoid camera going under the water */
		Debug.Log ("shark count  " + mSharks.Count);
	}

	void OnGUI() {
		Rect gui = new Rect (0, Screen.height - 150, Screen.width, 150);
		GUILayout.BeginArea (gui);
		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Pause", GUILayout.Height (150))) { //, 
			PlayPause();
		}
		if (GUILayout.Button ("FIRE" ,GUILayout.Height (150))) {
			fire = true;
		}
		GUILayout.EndHorizontal ();
		GUILayout.EndArea ();
	}
}
