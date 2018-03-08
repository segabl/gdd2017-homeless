using UnityEngine;

[ExecuteInEditMode]
public class AutoAdjustCollider : MonoBehaviour {

  public float widthAdjustment;
  public float heightAdjustment;

  // Use this for initialization
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
    BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
    boxCollider.size = spriteRenderer.size + new Vector2(widthAdjustment, heightAdjustment);
  }
}
