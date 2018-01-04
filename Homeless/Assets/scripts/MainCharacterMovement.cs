using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMovement : MonoBehaviour {

  private enum MouseButton{LEFT, RIGHT, MIDDLE };
  private Vector3 targetPosition;
  public float movementSpeed;
	
  void Start() {
    targetPosition = this.transform.position;
  }

	void Update () {
    if (Input.GetMouseButtonDown((int)MouseButton.LEFT)) {
      Debug.Log("LMB");
      targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      targetPosition.z = 10.0f;
      Debug.Log("Mouse clicked position in world is: " + targetPosition);
    }
    if(this.transform.position != targetPosition) {
      float step = movementSpeed * Time.deltaTime;
      this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, step);
    }
  }

}
