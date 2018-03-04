using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMovement : PausableObject {

  private enum MouseButton { LEFT, RIGHT, MIDDLE };
  private Vector3 targetPosition;
  public float movementSpeed;
  public bool walking { get; set; }
  private float walkingDirection;

  void Start() {
    targetPosition = this.transform.position;
    walking = false;
  }

  protected override void updatePausable() {
    float step = movementSpeed * Time.deltaTime;
    if (!GetComponent<Character>().alive) {
      return;
    }
    handleMouseMovementInput();
    handleKeyboardMovementInput(step);
    updatePosition(step);
    float corr = 0.01f;
    if (walking) {
      if (walkingDirection >= Mathf.PI * 0.25f + corr && walkingDirection < Mathf.PI * 0.75f) {
        //RIGHT
        if (this.transform.localScale.x < 0.0f) {
          this.transform.localScale = new Vector3(this.transform.localScale.x * -1.0f, this.transform.localScale.y, this.transform.localScale.y);
        }
        this.GetComponent<CharacterAnimation>().setAnimation = "walking_side";
      } else if (walkingDirection < Mathf.PI * 0.25f && walkingDirection > Mathf.PI * -0.25f) {
        //UP
        this.GetComponent<CharacterAnimation>().setAnimation = "walking_front";
      } else if (walkingDirection <= Mathf.PI * -0.25f - corr && walkingDirection > Mathf.PI * - 0.75) {
        //LEFT
        if (this.transform.localScale.x > 0.0f) {
          this.transform.localScale = new Vector3(this.transform.localScale.x * -1.0f, this.transform.localScale.y, this.transform.localScale.y);
        }
        this.GetComponent<CharacterAnimation>().setAnimation = "walking_side";
      } else if (walkingDirection >= Mathf.PI * 0.75f + corr || walkingDirection <= Mathf.PI * -0.75 - corr) {
        //DOWN
        this.GetComponent<CharacterAnimation>().setAnimation = "walking_front";
      }
    } else if (!walking && this.GetComponent<CharacterAnimation>().setAnimation.Contains("walking_")){
      this.GetComponent<CharacterAnimation>().setAnimation = "idle";
    }
  }

  private void handleMouseMovementInput() {
    if (Input.GetMouseButton((int)MouseButton.LEFT)) {
      targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
    walking = false;
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
      if (!Physics2D.Raycast(this.transform.position, direction_left, step, 1 << 0)) {
        direction_vector = direction_left;
        break;
      } else if (!Physics2D.Raycast(this.transform.position, direction_right, step, 1 << 0)) {
        direction_vector = direction_right;
        break;
      }
      a += Mathf.Deg2Rad;
    }
    if (direction_vector != Vector3.zero) {
      walking = true;
      walkingDirection = Mathf.Atan2(direction_vector.x, direction_vector.y);
      this.transform.position += direction_vector * Mathf.Min(step, dis);
    }
  }

  public void stopMovement() {
    walking = false;
    this.GetComponent<CharacterAnimation>().setAnimation = "idle";
    this.GetComponent<CharacterAnimation>().playCurrentAnimation();
    targetPosition = this.transform.position;
  }

}
