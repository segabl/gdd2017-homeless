using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHit : MonoBehaviour {
  void OnTriggerEnter2D(Collider2D col) {
    Debug.Log("Collision with: " + col.gameObject.name);
    col.gameObject.GetComponent<Car>().speed = 0;
    GetComponent<CharacterAnimation>().playOnce("die", "NONE");
  }
}
