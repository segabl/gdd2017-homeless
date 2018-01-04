using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour {

  private Vector3 targetPosition;
  public float movementSpeed;

  void Start () {
    targetPosition = this.transform.position;
	}
	
	void Update () {
    GameObject mainCharacter = GameObject.Find("MainCharacter");
    targetPosition = mainCharacter.transform.position;
    targetPosition.z = this.transform.position.z;
    if (this.transform.position != targetPosition) {
      float step = movementSpeed * Time.deltaTime;
      this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, step);
    }
  }
}
