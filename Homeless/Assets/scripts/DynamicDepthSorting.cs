using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriterDotNetUnity;

public class DynamicDepthSorting : MonoBehaviour {

  private SpriteRenderer[] spriteRenderers;
  private SpriterDotNetBehaviour spriterBehaviour;

  // Use this for initialization
  void Start () {
    spriterBehaviour = GetComponentInChildren<SpriterDotNetBehaviour>();
    if (spriterBehaviour) {
      spriterBehaviour.SortingLayer = "Dynamic";
    }
    else {
      spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
      foreach (var spriteRenderer in spriteRenderers) {
        spriteRenderer.sortingLayerName = "Dynamic";
      }
    }
	}
	
	// Update is called once per frame
	void Update () {
    if (spriterBehaviour) {
      spriterBehaviour.SortingOrder = (int)(transform.position.y * (-50));
    } else {
      foreach (var spriteRenderer in spriteRenderers) {
        spriteRenderer.sortingOrder = (int)(transform.position.y * (-50));
      }
    }
  }
}
