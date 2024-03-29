﻿using UnityEngine;

public class FollowCharacter : MonoBehaviour {

  private Vector3 targetPosition;
  public float movementSpeed;
  public GameObject mainCharacter;

  void Start() {
    targetPosition = this.transform.position;
  }

  void Update() {
    targetPosition = mainCharacter.transform.position;
    targetPosition.z = this.transform.position.z;
    if (this.transform.position != targetPosition) {
      float step = Vector3.Distance(this.transform.position, targetPosition) * movementSpeed * Time.deltaTime;//movementSpeed * Time.deltaTime;
      this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, step);
    }
  }
}
