using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : InteractionHandler {

  public override void interact() {
    Debug.Log("Item interaction");
    Inventory inventory = GameController.instance.player.GetComponent<Inventory>();
    Collectible c = this.GetComponent<Collectible>();
    if(c == null) {
      Debug.Log("No Collectible script attached to: " + this.name);
      return;
    }
    inventory.addItem(c);
    this.gameObject.SetActive(false);
    text.enabled = false;
    endInteraction();
  }
}
