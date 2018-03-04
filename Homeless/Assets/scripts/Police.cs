using UnityEngine;
using System.Collections;

public class Police : MonoBehaviour {

	public Transform target;
	public float moveSpeed = 3;

	void Start () {
		
		target = GameObject.Find("MainObject").transform;
	}

	void OnCollisionEnter2D(Collision2D other) {

		

			Chase();

	}

	void Chase () {
		Vector3 targetDirection = target.position - transform.position;
		transform.position += targetDirection * moveSpeed * Time.deltaTime;
	}
}