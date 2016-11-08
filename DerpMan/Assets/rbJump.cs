using UnityEngine;
using System.Collections;

public class rbJump : MonoBehaviour {

	public float jumpStr = 50.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.W))
		{
			GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpStr);	
		}
	
	}
}
