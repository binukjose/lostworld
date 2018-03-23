using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MineExplosion : MonoBehaviour {
	public GameObject Mine;
	public float mineRadius;
	public GameObject waterBody;

	private GameObject mMine;


	float getRand(float x){
		return Random.Range (
			x - mineRadius, 
			x + mineRadius);
	}

	Vector3 getMinePos(){
		Vector3 spos;
		Random.InitState(System.DateTime.Now.Millisecond);
		spos = new Vector3 (getRand(transform.position.x), 
							waterBody.transform.position.y , 
							getRand(transform.position.z));
		return spos;
	}

	void Start () {
	
		mMine = Instantiate (Mine);
		mMine.transform.SetPositionAndRotation (getMinePos (), Quaternion.identity);

	}
	
	// Update is called once per frame
	void Update () {

		if (mMine.tag == "Exploded") {
			mMine.transform.SetPositionAndRotation (getMinePos (), Quaternion.identity);
			mMine.tag = "Mine";
		}
		
	}
}
