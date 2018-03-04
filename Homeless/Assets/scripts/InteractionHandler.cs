﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InteractionHandler : PausableObject {

  public float triggerDistance;
  private string defaultText = "Press 'E' to interact";
  protected Text text;
  public static InteractionHandler interactObject;
  public static bool playerCanInteract;

  protected AudioClip interactClip;

  void Start() {
  }

  protected override void updatePausable() {

    GameObject canvas = GameController.instance.menuCanvas;
    text = canvas.GetComponentInChildren<Text>();

    if (!canvas || !text) {
      Debug.Log("Screen canvas or text is null");
      return;
    }

    if (Vector3.Distance(this.transform.position, GameController.instance.player.transform.position) < triggerDistance) {
      text.transform.position = Camera.main.WorldToScreenPoint(this.transform.position) + new Vector3(0, 50, 0);
      if (interactObject != this) {
        if (this is ItemInteraction) {
          text.text = "Press 'E' to pick up " + this.name;
        } else if (this is Thrascans) {
					var trashcan = (Thrascans)this;
					if (trashcan.isEmpty ()) {
						text.text = "Trashcan is empty";
					} else {
						text.text = "Press 'E' to search the trash";
					}
        } else if (this is CharacterInteraction) {
          text.text = "Press 'E' to talk to " + this.name;
        } else {
          text.text = defaultText;
        }
      }
      text.enabled = true;
      interactObject = this;
      playerCanInteract = true;
    }
    else if (interactObject == this) {
      endInteraction();
    }
  }

  public abstract void interact();
  protected void endInteraction() {
    text.enabled = false;
    interactObject = null;
    playerCanInteract = false;
  }
}
