using UnityEngine;

public class BuildingTransparency : MonoBehaviour {

  private SpriteRenderer rend;

  // Use this for initialization
  void Start() {

    rend = GetComponent<SpriteRenderer>();


  }

  // Update is called once per frame
  void Update() {

  }

  void OnColliderEnter2D(Collider2D col) {

    if (col.gameObject.tag == "Player") {
      rend.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);


    }
  }
}
