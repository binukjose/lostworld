using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oilBarrel : MonoBehaviour {


	public GameObject Barell;
	public GameObject BarellExplode;
	public float mineRadius;
	public GameObject waterBody;

	private GameObject mBarell;


	float getRand(float x){
		return Random.Range (
			x - mineRadius, 
			x + mineRadius);
	}

	Vector3 getBarellPos(){
		Vector3 spos;
		Random.InitState(System.DateTime.Now.Millisecond);
		spos = new Vector3 (getRand(transform.position.x), 
			waterBody.transform.position.y , 
			getRand(transform.position.z));
		return spos;
	}

	void Start () {

		mBarell = Instantiate (Barell);
		mBarell.tag = "Barell";
		mBarell.transform.SetPositionAndRotation (getBarellPos (), Quaternion.identity);

	}

	// Update is called once per frame
	void Update () {
		Debug.Log (" CollisionEnter mBarell is now    " + mBarell.tag + "  Child " +mBarell.transform.GetChild(0).tag );
		if (mBarell.transform.GetChild(0).tag == "Hit") {
			Debug.Log (" CollisionEnter mBarell is now  Hit, going to Explode  "  );
			GameObject explode = Instantiate (BarellExplode);
			explode.transform.SetPositionAndRotation (mBarell.transform.position, mBarell.transform.rotation);
			mBarell.transform.SetPositionAndRotation (getBarellPos (), Quaternion.identity);
			mBarell.transform.GetChild (0).tag = "Barell";
			mBarell.transform.tag = "Barell";
		}  

	}
}
