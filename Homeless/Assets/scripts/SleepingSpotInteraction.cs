using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingSpotInteraction : InteractionHandler {

  public override void interact() {
    var gc = GameController.instance;
    SleepingSpot spot = this.GetComponent<SleepingSpot>();
    if(spot == null) {
      Debug.Log("No SleepingSpot script attached to " + name);
    }
    gc.sleep(spot);
    endInteraction();
    suspend(5.0f);
  }

}
