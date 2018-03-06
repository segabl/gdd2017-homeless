using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InteractionHandler : PausableObject {

  public float triggerDistance;
  private string defaultText = "Press 'E' to interact";
  protected Text interactionText;
  public static InteractionHandler interactObject;
  public static bool playerCanInteract;

  protected AudioClip interactClip;

  void Start() {

  }

  protected override void updatePausable() {

    if (!interactionText) {
      GameObject canvas = GameController.instance.menuCanvas;
      Text[] texts = canvas.GetComponentsInChildren<Text>();
      foreach (Text t in texts) {
        if (t.name.Equals("InteractionText")) {
          interactionText = t;
          break;
        }
      }
      if (!interactionText) {
        Debug.Log("interactionText is null");
        return;
      }
    }

    if (Vector3.Distance(this.transform.position, GameController.instance.player.transform.position) < triggerDistance) {
      interactionText.transform.position = Camera.main.WorldToScreenPoint(this.transform.position) + new Vector3(0, 50, 0);
      if (interactObject != this) {
        if (this is ItemInteraction) {
          interactionText.text = "Press 'E' to pick up " + this.name;
        }
        else if (this is TrashcanInteraction) {
          interactionText.text = "Press 'E' to search the trash";
        } 
        else if (this is LootableCarInteraction) {
          interactionText.text = "Press 'E' to loot";
        } 
        else if (this is CharacterInteraction) {
          interactionText.text = "Press 'E' to talk to " + this.name;
        }
        else if (this is SleepingSpotInteraction) {
          interactionText.text = "Press 'E' to sleep";
        }
        else {
          interactionText.text = defaultText;
        }
      }
      interactionText.enabled = true;
      interactObject = this;
      playerCanInteract = true;
    }
    else if (interactObject == this) {
      endInteraction();
    }
  }

  public abstract void interact();
  protected void endInteraction() {
    interactionText.enabled = false;
    interactObject = null;
    playerCanInteract = false;
  }
}
