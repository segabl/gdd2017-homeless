using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : InteractionHandler {

  public override void interact() {
    Debug.Log("Item interaction");
    Collectible c = new Collectible(this.name);
    Inventory inventory = Inventory.getInstance();
    inventory.addItem(c);
    this.gameObject.SetActive(false);
  }
}
