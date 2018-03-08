using System;
using UnityEngine;


public class SleepingBoxInteraction : CharacterInteraction {
  public float healthGain;
  public float sanityGain;
  public String hasPermission() {
    if (GameController.instance.player.GetComponent<Character>().permisionToSleepInBox) {
      return "Y";
    }
    return "N";
  }
  public void Sleep() {
    GameController.instance.player.GetComponent<Character>().permisionToSleepInBox = false;
    SetNextTree("default");
    GameController.instance.player.GetComponent<CharacterAnimation>().SendMessage("OnUnpauseGame", SendMessageOptions.RequireReceiver);
    GameController.instance.sleep(GetComponent<SleepingSpot>());
    Debug.Log("Slept in the box");
  }
  protected override bool displayInteractionText()
  {
    interactionText.text = "Press 'E' to interact";
    return true;
  }
}

