using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicDepthSorting : MonoBehaviour {

  private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
    spriteRenderer = this.GetComponent<SpriteRenderer>();
    spriteRenderer.sortingLayerName = "Dynamic";
	}
	
	// Update is called once per frame
	void Update () {
    spriteRenderer.sortingOrder = (int) this.transform.position.y * (-10);
	}
}
