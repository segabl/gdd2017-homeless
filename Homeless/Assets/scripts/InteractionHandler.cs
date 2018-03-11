using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class InteractionHandler : PausableObject {

  public float triggerDistance;
  //private string defaultText = "Press 'E' to interact";
  protected Text interactText;
  public static InteractionHandler interactObject;
  public static bool playerCanInteract;

  protected AudioClip interactClip;
  private float suspendStart = 0;
  private float suspendLength = 0;
  private Action callAfterSuspend = null;
  private GameObject suspendTarget = null;
  private float delayStart = 0;
  private float delayLength = 0;
  private Action callAfterDelay = null;

  protected override void updatePausable() {
    if (!interactText) {
      GameObject canvas = GameController.instance.menuCanvas;
      Text[] texts = canvas.GetComponentsInChildren<Text>();
      foreach (Text t in texts) {
        if (t.name.Equals("InteractionText")) {
          interactText = t;
          break;
        }
      }
      if (!interactText) {
        Debug.Log("interactText is null");
        return;
      }
    }
    if (suspendTarget != null) {
      if (suspendTarget.activeSelf) {
        if (interactObject == this)
          endInteraction();
        return;
      }
      else {
        suspendTarget = null;
      }
    }
    if (suspendStart != 0f) {
      if (GameController.instance.dayTime - suspendStart >= suspendLength) {
        suspendStart = 0f;
        suspendLength = 0f;
        if (callAfterSuspend != null)
        {
          callAfterSuspend();
          callAfterSuspend = null;
        }
        
      }
      
      if (interactObject == this)
        endInteraction();
      return;
    }
    if (delayStart != 0)
    {
      if (GameController.instance.dayTime - delayStart >= delayLength)
      {
        delayStart = 0;
        delayLength = 0;
        if (callAfterDelay != null)
        {
          callAfterDelay();
          callAfterDelay = null;
        }
      }
    }
    if (Vector3.Distance(this.transform.position, GameController.instance.player.transform.position) < triggerDistance) {
      interactText.transform.position = Camera.main.WorldToScreenPoint(this.transform.position) + new Vector3(0, 50, 0);
      if (interactObject != this) {
        if (!displayInteractionText())
          return;
      }
      interactText.enabled = true;
      interactObject = this;
      playerCanInteract = true;

    }
    else if (interactObject == this) {
      endInteraction();
    }
  }

  public abstract void interact();
  protected void endInteraction() {
    interactText.enabled = false;
    interactObject = null;
    playerCanInteract = false;
  }
  protected void suspend(float time) {
    suspendStart = GameController.instance.dayTime;
    suspendLength = time / GameController.instance.dayLength;
  }
  protected void suspendWhileActive(GameObject target) {
    suspendTarget = target;
  }
  protected void suspendThenCall(float time, Action function)
  {
    suspend(time);
    callAfterSuspend = function;
  }
  protected void waitThenCall(float time, Action call)
  {
    delayStart = GameController.instance.dayTime;
    delayLength = time / GameController.instance.dayLength;
    callAfterDelay = call;
  }
  protected abstract bool displayInteractionText();
}
