using System;
using UnityEngine;

public class ItemInteraction : InteractionHandler {

  protected bool canPickUp = true;
  private void Start() {
    interactClip = (AudioClip)Resources.Load("sfx/grab-item");
  }

  public override void interact() {
    Debug.Log("Item interaction");
    Inventory inventory = GameController.instance.player.GetComponent<Inventory>();
    Collectible c = this.GetComponent<Collectible>();
    if (c == null) {
      Debug.Log("No Collectible script attached to: " + this.name);
      return;
    }
    if (inventory.addItem(c) && interactClip) {
      AudioSource audioSource = GameController.instance.player.gameObject.GetComponent<AudioSource>();
      if (audioSource) {
        audioSource.clip = interactClip;
        audioSource.Play();
      }
      GameController.instance.player.GetComponent<CharacterAnimation>().playOnce("pickup_front", "idle");
      this.gameObject.SetActive(false);
      interactionText.enabled = false;
      endInteraction();
    }
    else
    {
      canPickUp = false;
      displayInteractionText();
    }
    
  }

  protected override void displayInteractionText()
  {
    if (canPickUp)
      interactionText.text = "Press 'E' to pick up " + this.name;
    else
      interactionText.text = "Inventory is full";
  }
}
