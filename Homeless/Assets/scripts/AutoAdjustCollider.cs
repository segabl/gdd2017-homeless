using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AutoAdjustCollider : MonoBehaviour {

  public float widthAdjustment;
  public float heightAdjustment;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    Vector2 spriteSize = GetComponent<SpriteRenderer>().size;
    GetComponent<BoxCollider2D>().size = new Vector2(spriteSize.y + widthAdjustment, spriteSize.y + heightAdjustment);
	}
}
