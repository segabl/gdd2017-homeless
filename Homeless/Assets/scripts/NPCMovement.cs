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


	private Vector2 minWalkArea;
	private Vector2 maxWalkArea;
	public Collider2D walkArea;
	private bool inWalkArea=false;
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

		if (walkArea != null) {
		
			minWalkArea = walkArea.bounds.min;
			maxWalkArea = walkArea.bounds.max;
			inWalkArea = true;

		
		}

		
	}


	
	// Update is called once per frame
	void Update () {

		if (isWalking) {

			walkCounter -= Time.deltaTime;
		
			switch (walkDirection) {

			case 0:
				myRigidBody.velocity = new Vector2 (0, NPCspeed);
				if (inWalkArea==true && transform.position.y > maxWalkArea.y) {
				
					waitCounter = waitTime;
					isWalking = false;
				}
				break;

			case 1:
				myRigidBody.velocity = new Vector2 (NPCspeed, 0);
				if (inWalkArea==true && transform.position.x > maxWalkArea.x) {

					waitCounter = waitTime;
					isWalking = false;
				}
				break;

			case 2:
				myRigidBody.velocity = new Vector2 (0, -NPCspeed);
				if (inWalkArea==true && transform.position.y < minWalkArea.y) {

					waitCounter = waitTime;
					isWalking = false;
				}
				break;

			case 3:
				myRigidBody.velocity = new Vector2 (-NPCspeed, 0);
				if (inWalkArea==true && transform.position.x < minWalkArea.y) {

					waitCounter = waitTime;
					isWalking = false;
				}
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

