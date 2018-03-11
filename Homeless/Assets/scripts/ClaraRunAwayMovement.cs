using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaraRunAwayMovement : PausableObject {
  private float walkingDirection;
  public float movementSpeed {get; set;}
  public Vector3 hideoutPosition { get; set; }

  // Use this for initialization
  void Start () {
    movementSpeed = 0;
	}

  protected override void updatePausable() {
    if (movementSpeed == 0) {
      return;
    } 
    if (GetComponent<CarHit>().hit) {
      return;
    }

    float step = movementSpeed * Time.deltaTime;
    float dis = Mathf.Sqrt(Mathf.Pow(this.transform.position.x - hideoutPosition.x, 2) + Mathf.Pow(this.transform.position.y - hideoutPosition.y, 2));
    if (dis <= 0) {
      return;
    }
    float direction = Mathf.Atan2(this.transform.position.x - hideoutPosition.x, this.transform.position.y - hideoutPosition.y);
    float a = 0;
    Vector3 direction_left = Vector3.zero;
    Vector3 direction_right = Vector3.zero;
    Vector3 direction_vector = Vector3.zero;
    while (a < Mathf.Deg2Rad * 80) {
      direction_left.x = -Mathf.Sin(direction + a);
      direction_left.y = -Mathf.Cos(direction + a);
      direction_right.x = -Mathf.Sin(direction - a);
      direction_right.y = -Mathf.Cos(direction - a);
      RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction_left, step, 1 << 0);
      if (hit.collider == null) {
        direction_vector = direction_left;
        break;
      }
      hit = Physics2D.Raycast(this.transform.position, direction_right, step, 1 << 0);
      if (hit.collider == null) {
        direction_vector = direction_right;
        break;
      }
      a += Mathf.Deg2Rad;
    }
    
    if (direction_vector != Vector3.zero) {
      walkingDirection = Mathf.Atan2(direction_vector.x, direction_vector.y);
      this.transform.position += direction_vector * Mathf.Min(step, dis);
    }
    /*
    if (walkingDirection >= Mathf.PI * 0.25f + corr && walkingDirection < Mathf.PI * 0.75f) {
      //RIGHT
      if (this.transform.localScale.x < 0.0f) {
        this.transform.localScale = new Vector3(this.transform.localScale.x * -1.0f, this.transform.localScale.y, this.transform.localScale.y);
      }
      this.GetComponent<CharacterAnimation>().currentAnimation = "walking_side";
    }
    else if (walkingDirection < Mathf.PI * 0.25f && walkingDirection > Mathf.PI * -0.25f) {
      //UP
      this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.y);
      this.GetComponent<CharacterAnimation>().currentAnimation = "walking_back";
    }
    else if (walkingDirection <= Mathf.PI * -0.25f - corr && walkingDirection > Mathf.PI * -0.75) {
      //LEFT
      if (this.transform.localScale.x > 0.0f) {
        this.transform.localScale = new Vector3(this.transform.localScale.x * -1.0f, this.transform.localScale.y, this.transform.localScale.y);
      }
      this.GetComponent<CharacterAnimation>().currentAnimation = "walking_side";
    }
    else if (walkingDirection >= Mathf.PI * 0.75f + corr || walkingDirection <= Mathf.PI * -0.75 - corr) {
      //DOWN
      this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.y);
      this.GetComponent<CharacterAnimation>().currentAnimation = "walking_front";
    }*/
  }
}
