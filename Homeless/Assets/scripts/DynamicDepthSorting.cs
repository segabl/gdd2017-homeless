using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriterDotNetUnity;

public class DynamicDepthSorting : MonoBehaviour {

  private SpriteRenderer spriteRenderer;
  private SpriterDotNetBehaviour spriterBehaviour;

  // Use this for initialization
  void Start () {
    spriteRenderer = GetComponent<SpriteRenderer>();
    if (spriteRenderer) {
      spriteRenderer.sortingLayerName = "Dynamic";
    }
    spriterBehaviour = GetComponent<SpriterDotNetBehaviour>();
    if (spriterBehaviour) {
      spriterBehaviour.SortingLayer = "Dynamic";
    }
	}
	
	// Update is called once per frame
	void Update () {
    if (spriteRenderer) {
      spriteRenderer.sortingOrder = (int)(transform.position.y * (-50));
    }
    if (spriterBehaviour) {
      spriterBehaviour.SortingOrder = (int)(transform.position.y * (-50));
    }
  }
}
