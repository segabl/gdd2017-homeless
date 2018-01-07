using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionHandler : PausableObject {

  public GameObject mainCharacter;
  public GameObject canvas;
  public float triggerDistance;
  public UnityEngine.UI.Text text;
  private string defaultText;

  void Start() {
    defaultText = text.text;
  }

  protected override void updatePausable() { 

    if (Vector3.Distance(this.transform.position, mainCharacter.transform.position) < triggerDistance) {
      Debug.Log("Interaction with " + this.name + " possible");
      //TODO: set the value of text offset to something reasonable
      float textOffset = 0.5f;
      text.transform.position = this.transform.position + new Vector3(0.0f, textOffset, 0.0f);
      canvas.SetActive(true);
      if (Input.GetKeyDown(KeyCode.E)) {
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
