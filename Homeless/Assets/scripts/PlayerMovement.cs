using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private Rigidbody2D rigid;
	public float speed=5.0f;

	// Use this for initialization
	void Start () {

		rigid = GetComponent<Rigidbody2D> ();
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.W)) {
			rigid.transform.position += Vector3.up * Time.deltaTime * speed;
		}

		if (Input.GetKey (KeyCode.S)) {
			rigid.transform.position += Vector3.down * Time.deltaTime * speed;
		}

		if (Input.GetKey (KeyCode.A)) {
			rigid.transform.position += Vector3.left * Time.deltaTime * speed;
		}

		if (Input.GetKey (KeyCode.D)) {
			rigid.transform.position += Vector3.right * Time.deltaTime * speed;
		}
		
	}
}
