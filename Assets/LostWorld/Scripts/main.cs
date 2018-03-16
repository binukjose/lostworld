using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour {

	public GameObject SharkGameObj;
	 
	public Camera ArcoreCamera;
	public GameObject water;
	public GameObject gun;
	public int shark_swim_radius_x ;
	public int shark_swim_radius_z ;
	public int NUM_SHARKS  ;
	private List <GameObject> m_Sharks =new List<GameObject>();

  

	public GameObject kuntham;
	private bool fire = false;


	void Start () {

		/*Create the fishes */
		float waterSurfaceHeight = water.transform.position.y -1;  	 
		Random.InitState(System.DateTime.Now.Millisecond);
		for (int i=0;i<NUM_SHARKS;i++) {
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
	}
	
	/* Need to trigger a spear shot if button is pressed */
	void Update () {
		/*Set the gun position */
		Vector3 startpos = ArcoreCamera.transform.position;
		startpos.y--;
		gun.transform.SetPositionAndRotation (startpos, ArcoreCamera.transform.rotation);

		/* If triggered , Fire Spear*/
		if (fire) {
			try {
				var x = Instantiate (kuntham, startpos, ArcoreCamera.transform.rotation);
				x.tag="dynamic"; /* only dymanic spears get destroyed*/
			}
			catch (System.Exception e)
			{
				Debug.Log ("Main Kuntham failed " + e);
			}
			fire = false;
		}
		Debug.Log ("shark count  " + m_Sharks.Count);

		//if (m_Sharks.Count < NUM_SHARKS ) {
		//}

	
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
