using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMovement : PausableObject {

  private enum MouseButton { LEFT, RIGHT, MIDDLE };
  private Vector3 targetPosition;
  public float movementSpeed;

  void Start() {
    targetPosition = this.transform.position;
  }

  protected override void updatePausable() {
    float step = movementSpeed * Time.deltaTime;
    handleMouseMovementInput();
    handleKeyboardMovementInput(step);

    updatePosition(step);
  }

  private void handleMouseMovementInput() {
    if (Input.GetMouseButtonDown((int)MouseButton.LEFT)) {
      Debug.Log("LMB");
      targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      targetPosition.z = this.transform.position.z;
      Debug.Log("Mouse clicked position in world is: " + targetPosition);
    }
  }

  private void handleKeyboardMovementInput(float step) {
    if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")) {
      targetPosition = this.transform.position;
    }

    if (Input.GetKey("w")) {
      targetPosition += new Vector3(0, step);
    }
    if (Input.GetKey("a")) {
      targetPosition += new Vector3(-step, 0);
    }
    if (Input.GetKey("s")) {
      targetPosition += new Vector3(0, -step);
    }
    if (Input.GetKey("d")) {
      targetPosition += new Vector3(step, 0);
    }
  }

  private void updatePosition(float step) {
    if (this.transform.position != targetPosition && !Physics2D.Raycast(this.transform.position, (targetPosition - this.transform.position).normalized, step)) {
      this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, step);
    }
  }
}
