using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour {

	//public variables
	public GameObject SharkGameObj; 
	public GameObject StingRayGameObj; 
	public Camera ArcoreCamera;
	public GameObject gun;
	public GameObject kuntham;
	public GameObject water;
	public GameObject Barrel;
	public int shark_swim_radius_x ;
	public int shark_swim_radius_z ;
	public int NUM_SHARKS  ;

	//private 
	private List <GameObject> m_Sharks =new List<GameObject>();
	private int mSharkCount;
	private bool fire = false;
	private bool gamePause = false;
	private float waterSurfaceHeight;

	void AddAShark()
	{ 
		Random.InitState(System.DateTime.Now.Millisecond);
		Vector3 spos= new Vector3(Random.Range
			(ArcoreCamera.transform.position.x - shark_swim_radius_x , 
				ArcoreCamera.transform.position.x + shark_swim_radius_x), 
			waterSurfaceHeight  ,   
			Random.Range(
				ArcoreCamera.transform.position.z - shark_swim_radius_z, 
				ArcoreCamera.transform.position.z + shark_swim_radius_z));
		try {

			m_Sharks.Add(Instantiate (SharkGameObj ,spos,Quaternion.identity));
		}
		catch (System.Exception ex) {
			Debug.Log (" Target exception " + ex.ToString());
		} 
	}

	void Start () {
		//Set the height of water surface
		 waterSurfaceHeight = water.transform.position.y -1;
		 SharkGameObj.GetComponent<Animation>().Play("GW_Move");

	}
	

	void Update () {
		Debug.Log ("NUM_SHARKS" +NUM_SHARKS + "shark list count  " + m_Sharks.Count + "Shar count" + mSharkCount);
		if (mSharkCount < NUM_SHARKS) {
			AddAShark ();
			mSharkCount++;
			Debug.Log ("shark list count  " + m_Sharks.Count + "Shar count" + mSharkCount);
		}
			
		/*Set the gun position */
		Vector3 startpos = ArcoreCamera.transform.position;
		startpos.y = startpos.y-1;
		gun.transform.SetPositionAndRotation (startpos, ArcoreCamera.transform.rotation);

		/* Make sure the barrels are floating */
		//startpos = Barrel.transform.position;
		//startpos.y = water.transform.position.y;
		//Barrel.transform.position = startpos;

		/*Set the water y-position , to avoid going inside the water this is not working.
		if ((ArcoreCamera.transform.position.y - water.transform.position.y) < 1) {
			startpos = water.transform.position;
			startpos.y = ArcoreCamera.transform.position.y - 1;
			water.transform.position = startpos;	
		}
		*/
		/*Set the water position , to avoid running out of water*/
		float distance = Vector3.Distance (ArcoreCamera.transform.position, water.transform.position);
		if (distance > 20) {
			startpos = ArcoreCamera.transform.position;
			startpos.y = water.transform.position.y;
			water.transform.position = startpos;
			Debug.Log ("Main: water position changes to " + startpos);

		}

		/* Need to trigger a spear shot if button is pressed */
		if (fire) {
			try {
				startpos = ArcoreCamera.transform.position;
				var x = Instantiate (kuntham, startpos, ArcoreCamera.transform.rotation);
				x.tag="dynamic"; /* only dymanic spears get destroyed*/
				gun.GetComponent<Animation>().Play("Reload");
			}
			catch (System.Exception e)
			{
				Debug.Log ("Main: Kuntham creation failed " + e);
			}
			fire = false;
		}
		/* TODO: needs to avoid camera going under the water */
		Debug.Log ("shark count  " + m_Sharks.Count);
	}

	void OnGUI() {
		Rect gui = new Rect (0, Screen.height - 150, Screen.width, 150);
		GUILayout.BeginArea (gui);
		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Pause", GUILayout.Height (150))) { //, 
			if(gamePause) {
				gamePause = false;
				for ( int i=0; i< m_Sharks.Count; i++) {
					m_Sharks[i].SendMessage("GlobalMessage", "play");
				}
				SharkGameObj.SendMessage("GlobalMessage", "play");
				StingRayGameObj.SendMessage("GlobalMessage", "play");
			} else {
				for ( int i=0; i< m_Sharks.Count; i++) {
					m_Sharks[i].SendMessage("GlobalMessage", "pause");
				}
				SharkGameObj.SendMessage("GlobalMessage", "pause");
				StingRayGameObj.SendMessage("GlobalMessage", "pause");
				gamePause = true;
			}
		}
		if (GUILayout.Button ("FIRE" ,GUILayout.Height (150))) {
			fire = true;
		}
		GUILayout.EndHorizontal ();
		GUILayout.EndArea ();
	}
}
