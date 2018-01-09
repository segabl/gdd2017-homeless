﻿using System.Collections;
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

    GameObject canvas = GameController.instance.screenCanvas;
    text = canvas.GetComponentInChildren<Text>();

    if(!canvas || !text) {
      Debug.Log("Screen canvas or text is null");
      return;
    }

    if (Vector3.Distance(this.transform.position, GameController.instance.player.transform.position) < triggerDistance) {
      //TODO: set the value of text offset to something reasonable
      float textOffset = 0.5f;
      text.transform.position = this.transform.position + new Vector3(0.0f, textOffset, 0.0f);
      interactObject = this;
      canvas.SetActive(true);

    }
    else if(interactObject == this) {
      canvas.SetActive(false);
      text.text = defaultText;
      interactObject = null;
    }
  }

  public abstract void interact();

}
