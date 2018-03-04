using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 

public class Thrascans : InteractionHandler {

	public List<GameObject> items;
	public float resetTime;


	void Start()
	{
		
	}
	public override void interact() {
	Debug.Log ("Item interaction");
		if (resetTime <= 0) {
			int num = Random.Range (0, items.Count);
			Vector3 positionx = new Vector3 (1, 0, 0);

			Instantiate (items [num], transform.position + positionx, Quaternion.identity);
			resetTime = 10;
		} else {
			
		}
	}
	public bool isEmpty()
	{
		if(resetTime > 0)
		{
			return true;
		}
		return false;
	}
	void Update()
	{
		updatePausable ();
		Debug.Log (resetTime);
		if(resetTime > 0)
		{
			resetTime -= Time.deltaTime;
		}
	}
}

