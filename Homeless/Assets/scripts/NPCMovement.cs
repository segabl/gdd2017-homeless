using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour {

	private Rigidbody2D myRigidBody;
	public bool isWalking;
	public bool isWaiting;
	public float waitTime;
	public float walkTime;
	private float walkCounter;
	private float waitCounter;
	public float NPCspeed;
	private int walkDirection;

	private Vector3 targetPosition;

	private SpriteRenderer[] sprites;

	// Use this for initialization
	void Start () {

		myRigidBody = GetComponent<Rigidbody2D> ();
		waitCounter = waitTime;
		walkCounter = walkTime;
		ChooseDirection ();

		sprites = GetComponentsInChildren<SpriteRenderer> ();

		for (int i = 0; i <= sprites.Length; i++) {

			sprites [i].color = new Color (Random.Range (0, 255)*Time.deltaTime, Random.Range (0, 255)*Time.deltaTime, Random.Range (0, 255)*Time.deltaTime);
		}

		
	}


	
	// Update is called once per frame
	void Update () {

		if (isWalking) {

			walkCounter -= Time.deltaTime;
		
			switch (walkDirection) {

			case 0:
				myRigidBody.velocity = new Vector2 (0, NPCspeed);
				break;

			case 1:
				myRigidBody.velocity = new Vector2 (NPCspeed, 0);
				break;

			case 2:
				myRigidBody.velocity = new Vector2 (0, -NPCspeed);
				break;

			case 3:
				myRigidBody.velocity = new Vector2 (-NPCspeed, 0);
				break;

			}




			if (walkCounter < 0) {
				waitCounter = waitTime;
				isWalking = false;
			} 
		}
			else {
				waitCounter -= Time.deltaTime;
				myRigidBody.velocity = Vector2.zero;
				if (waitCounter < 0) {
					
					ChooseDirection ();

				}
			
			}
		
		}


		

		


	void ChooseDirection(){

		walkDirection = Random.Range (0, 4);
		isWalking = true;
		walkCounter = walkTime;
	
	}


}

