using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMovement : PausableObject {

  private enum MouseButton { LEFT, RIGHT, MIDDLE };
  private Vector3 targetPosition;
  public float movementSpeed;
  public bool walking { get; set; }

  void Start() {
    targetPosition = this.transform.position;
    walking = false;
  }

  protected override void updatePausable() {
    float step = movementSpeed * Time.deltaTime;
    Vector3 oldPosition = targetPosition;
    handleMouseMovementInput();
    bool keyPressed = handleKeyboardMovementInput(step);

    updatePosition(step);
    if (Vector3.Distance(this.transform.position, targetPosition) >= 10.01f || keyPressed) {
      walking = true;
    } else {
      walking = false;
    } 

  }

  private void handleMouseMovementInput() {
    if (Input.GetMouseButton((int)MouseButton.LEFT)) {
      //Debug.Log("LMB");
      targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      //Debug.Log("Mouse clicked position in world is: " + targetPosition);
    }
  }

  private bool handleKeyboardMovementInput(float step) {
    bool keyPressed = false;
    if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")) {
      targetPosition = this.transform.position;
      keyPressed = true;
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
    return keyPressed;
  }

  private void updatePosition(float step) {
    float dis = Mathf.Sqrt(Mathf.Pow(this.transform.position.x - targetPosition.x, 2) + Mathf.Pow(this.transform.position.y - targetPosition.y, 2));
    if (dis <= 0) {
      return;
    }
    float direction = Mathf.Atan2(this.transform.position.x - targetPosition.x, this.transform.position.y - targetPosition.y);
    float a = 0;
    Vector3 direction_left = Vector3.zero;
    Vector3 direction_right = Vector3.zero;
    Vector3 direction_vector = Vector3.zero;
    while (a < Mathf.Deg2Rad * 80) {
      direction_left.x = -Mathf.Sin(direction + a);
      direction_left.y = -Mathf.Cos(direction + a);
      direction_right.x = -Mathf.Sin(direction - a);
      direction_right.y = -Mathf.Cos(direction - a);
      if (!Physics2D.Raycast(this.transform.position, direction_left, step,0)) {
        direction_vector = direction_left;
        break;
      } else if (!Physics2D.Raycast(this.transform.position, direction_right, step,0)) {
        direction_vector = direction_right;
        break;
      }
      a += Mathf.Deg2Rad;
    }
    this.transform.position += direction_vector * Mathf.Min(step, dis);
  }
}
