using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour {

	//public variables
	public GameObject SharkGameObj; 
	public Camera ArcoreCamera;
	public GameObject gun;
	public GameObject kuntham;
	public GameObject water;
	public int shark_swim_radius_x ;
	public int shark_swim_radius_z ;
	public int NUM_SHARKS  ;

	//private 
	private List <GameObject> m_Sharks =new List<GameObject>();
	private int mSharkCount;
	private bool fire = false;
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

	}
	
	/* Need to trigger a spear shot if button is pressed */
	void Update () {
		Debug.Log ("NUM_SHARKS" +NUM_SHARKS + "shark list count  " + m_Sharks.Count + "Shar count" + mSharkCount);
		if (mSharkCount < NUM_SHARKS) {
			AddAShark ();
			mSharkCount++;
			Debug.Log ("shark list count  " + m_Sharks.Count + "Shar count" + mSharkCount);
		}
			
		/*Set the gun position */
		Vector3 startpos = ArcoreCamera.transform.position;
		startpos.y = startpos.y - 0.4f;
		gun.transform.SetPositionAndRotation (startpos, ArcoreCamera.transform.rotation);

		/*Set the water position , to avoid running out of water*/
		float distance = Vector3.Distance (ArcoreCamera.transform.position, water.transform.position);
		if (distance > 20) {
			startpos = ArcoreCamera.transform.position;
			startpos.y = water.transform.position.y;
			water.transform.position = startpos;
			Debug.Log ("Main: water position changes to " + startpos);

		}

		/* If triggered , Fire Spear*/
		if (fire) {
			try {
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
		Rect gui = new Rect ((Screen.width+300)/2, Screen.height - 150, 300, 300);
		GUILayout.BeginArea (gui);
		if (GUILayout.Button ("FIRE", GUILayout.Height (150))) {
			fire = true;
		}
		GUILayout.EndVertical();
	}
}
