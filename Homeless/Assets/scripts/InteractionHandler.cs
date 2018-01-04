using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionHandler : MonoBehaviour {

  public GameObject mainCharacter;
  public GameObject canvas;
  public float triggerDistance;
  public UnityEngine.UI.Text text;
  private string defaultText;

  void Start() {
    defaultText = text.text;
  }

	void Update () {
    if (Vector3.Distance(this.transform.position, mainCharacter.transform.position) < triggerDistance) {
      canvas.SetActive(true);
      if (Input.GetKeyDown("e")) {
        interact();
      }
    }
    else {
      canvas.SetActive(false);
      text.text = defaultText;
    }
  }

  public abstract void interact();

}
