using KarmaSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts {
  class BeerGuyCharacterInteraction : CharacterInteraction {
    public int hasBeer() {
      Inventory player_inventory = GameController.instance.player.GetComponent<Inventory>();
      if (player_inventory.containsItem("Alcohol")) {
        return 0;
      }
      return 1;
    }

    public int giveBeerY() {
      //sample social interaction
      Inventory player_inventory = GameController.instance.player.GetComponent<Inventory>();
      Inventory this_inventory = gameObject.GetComponent<Inventory>();
      if (player_inventory.giveItem("Alcohol", this_inventory)) {
        GameController.instance.karmaController.SocialAction(GameController.instance.player, SocialConstants.sharingBeer, gameObject);
        Debug.Log("Shared some beer");
      }

      GameController.instance.karmaController.DebugKarmaList();
      return 0;
    }

    public int giveBeerN() {
      return 0;
    }
  }
}
