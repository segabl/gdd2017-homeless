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

    GameController.instance.unpauseAll();
    GameController.instance.pauseGameAndBlend(GameController.PauseReason.SLEEPING, false, gameObject);
    Debug.Log("Slept in the box");
  }
  protected override bool displayInteractionText()
  {

    interactText.text = "Press 'E' to interact";
    return true;
  }
}

