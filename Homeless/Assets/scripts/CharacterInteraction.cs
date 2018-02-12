using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KarmaSystem;

public class CharacterInteraction : InteractionHandler {

  public override void interact() {
    text.text = "This is a character text";
    Debug.Log("Character Interaction");

    //sample social interaction
    Inventory player_inventory = GameController.instance.player.GetComponent<Inventory>();
    Inventory this_inventory = gameObject.GetComponent<Inventory>();
    Collectible alcohol = new Collectible("Alcohol", null);
    if (player_inventory.giveItem(alcohol, this_inventory))
    {
      GameController.instance.karmaController.SocialAction(GameController.instance.player, SocialConstants.sharingBeer,gameObject);
      Debug.Log("Shared some beer");
    }

    GameController.instance.karmaController.DebugKarmaList();
    //endInteraction();
  }

}
