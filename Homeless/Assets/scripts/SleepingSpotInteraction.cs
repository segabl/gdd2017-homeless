﻿using System;
using UnityEngine;

public class SleepingSpotInteraction : InteractionHandler {

  public override void interact() {
    var gc = GameController.instance;
    SleepingSpot spot = this.GetComponent<SleepingSpot>();
    if (spot == null) {
      Debug.Log("No SleepingSpot script attached to " + name);
    }
    gc.pauseGameAndBlend(GameController.PauseReason.SLEEPING, false, gameObject);
    endInteraction();
    suspend(5.0f);
  }
  protected override bool displayInteractionText() {
    float direction = Mathf.Atan2(this.transform.position.x - GameController.instance.player.transform.position.x,
      this.transform.position.y - GameController.instance.player.transform.position.y);
    if (!(direction > -Mathf.PI / 10.0f && direction < Mathf.PI / 10.0f)) {
      return false;
    }
    interactText.text = "Press 'E' to sleep";
    return true;
  }

}
