using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : InteractionHandler {

  public override void interact() {
    Debug.Log("Item interaction");
    SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
    if(spriteRenderer == null) {
      Debug.Log("SpriteRenderer not found, can't set SpriteRenderer to Collectible. Item can't be added to inventory.");
      endInteraction();
      return;
    }
    Sprite sprite = spriteRenderer.sprite;
    Collectible c = new Collectible(this.name, sprite);
    Inventory inventory = GameController.instance.player.GetComponent<Inventory>();
    inventory.addItem(c);
    this.gameObject.SetActive(false);
    text.enabled = false;
    endInteraction();
  }
}
