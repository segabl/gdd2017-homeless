using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : InteractionHandler {

  private void Start() {
    interactClip = (AudioClip)Resources.Load("sfx/grab-item");
  }

  public override void interact() {
    Debug.Log("Item interaction");
    Inventory inventory = GameController.instance.player.GetComponent<Inventory>();
    Collectible c = this.GetComponent<Collectible>();
    if(c == null) {
      Debug.Log("No Collectible script attached to: " + this.name);
      return;
    }
    if (inventory.addItem(c) && interactClip) {
      AudioSource audioSource = GameController.instance.player.gameObject.GetComponent<AudioSource>();
      if (audioSource) {
        audioSource.clip = interactClip;
        audioSource.Play();
      }
    }
    this.gameObject.SetActive(false);
    text.enabled = false;
    endInteraction();
  }
}
