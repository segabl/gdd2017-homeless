using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InteractionHandler : PausableObject {

  public float triggerDistance;
  private string defaultText = "Press 'E' to interact";
  protected Text text;
  public static InteractionHandler interactObject; 

  void Start() {
  }

  protected override void updatePausable() {

    GameObject canvas = GameController.instance.menuCanvas;
    text = canvas.GetComponentInChildren<Text>();

    if(!canvas || !text) {
      Debug.Log("Screen canvas or text is null");
      return;
    }

    if (Vector3.Distance(this.transform.position, GameController.instance.player.transform.position) < triggerDistance) {
      //TODO: set the value of text offset to something reasonable
      float textOffset = 0.5f;
      text.transform.position = this.transform.position + new Vector3(0.0f, textOffset, 0.0f);
      if (interactObject != this) {
        if (this is ItemInteraction) {
          text.text = "Press 'E' to pick up " + this.name;
        } else {
          text.text = defaultText;
        }
      }
      text.enabled = true;
      interactObject = this;
    } else if(interactObject == this) {
      text.enabled = false;
      interactObject = null;
    }
  }

  public abstract void interact();

}
