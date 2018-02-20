using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 

public class Thrascans : InteractionHandler {

	public GameObject item1;
	public GameObject item2;
	public GameObject item3;

		public override void interact() {
			Debug.Log("Item interaction");
		int num = Random.Range (1, 4);
		Vector3 positionx = new Vector3 (1, 0, 0);
		switch (num) {
		case 1:
			Instantiate (item1, transform.position+positionx, Quaternion.identity);
			break;

		case 2:
			Instantiate (item2, transform.position+positionx,Quaternion.identity);
			break;

		case 3:
			Instantiate (item3, transform.position + positionx, Quaternion.identity);
			break;

		}
			/*Collectible c = new Collectible(this.name);
			Inventory inventory = GameController.instance.player.GetComponent<Inventory>();
			inventory.addItem(c);
			this.gameObject.SetActive(false);*/
			text.enabled = false;
		}
	}

