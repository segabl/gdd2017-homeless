using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMovement : PausableObject {

  private enum MouseButton{LEFT, RIGHT, MIDDLE};
  private Vector3 targetPosition;
  public float movementSpeed;
	
  void Start() {
    targetPosition = this.transform.position;
  }

	protected override void updatePausable () {

    if (Input.GetMouseButtonDown((int)MouseButton.LEFT)) {
      Debug.Log("LMB");
      targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      targetPosition.z = this.transform.position.z;
      Debug.Log("Mouse clicked position in world is: " + targetPosition);
    }
    float step = movementSpeed * Time.deltaTime;
    if (this.transform.position != targetPosition && !Physics2D.Raycast(this.transform.position, (targetPosition - this.transform.position).normalized, step)) {
      this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, step);
    }

  }

}
