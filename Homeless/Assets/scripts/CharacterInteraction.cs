using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KarmaSystem;

public class CharacterInteraction : InteractionHandler {

  public override void interact() {
    Debug.Log("Character Interaction");

    //sample social interaction
    Inventory player_inventory = GameController.instance.player.GetComponent<Inventory>();
    Inventory this_inventory = gameObject.GetComponent<Inventory>();
    if (player_inventory.giveItem("Alcohol", this_inventory))
    {
      GameController.instance.karmaController.SocialAction(GameController.instance.player, SocialConstants.sharingBeer,gameObject);
      Debug.Log("Shared some beer");
      text.text = "Thanks!";
    }
    else
    {
      text.text = "I'm soo thirsty...";
    }

    GameController.instance.karmaController.DebugKarmaList();
    //endInteraction();
  }

}
