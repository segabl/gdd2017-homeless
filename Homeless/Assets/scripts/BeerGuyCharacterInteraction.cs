using KarmaSystem;
using UnityEngine;

namespace Assets.scripts {
  class BeerGuyCharacterInteraction : CharacterInteraction {
    public void giveBeerY() {
      Inventory player_inventory = GameController.instance.player.GetComponent<Inventory>();
      Inventory this_inventory = gameObject.GetComponent<Inventory>();
      if (player_inventory.giveItem(player_inventory.findMatch(Collectible.Type.DRINK), this_inventory)) {
        GameController.instance.karmaController.SocialAction(GameController.instance.player, SocialConstants.sharingBeer, gameObject);
        Debug.Log("Shared some beer");
      } else {
        Debug.LogWarning("Couldn't give Alcohol to Beerguy");
      }

      GameController.instance.karmaController.DebugKarmaList();
      SetNextTree("HappyBeerGuy");
    }

    public void giveBeerN() {
      
    }
  }
}
