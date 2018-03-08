using System;
using KarmaSystem;

class LootableCarInteraction : CharacterInteraction {
  public Collectible loot;

  public void SearchCarY() {
    Inventory player_inventory = GameController.instance.player.GetComponent<Inventory>();
    loot.Start();
    player_inventory.addItem(loot);
    GameController.instance.karmaController.SocialAction(GameController.instance.player, SocialConstants.stealingBeerFromShop, null);
    SetNextTree("hasStolen");
  }
  protected override bool displayInteractionText()
  {
    interactionText.text = "Press 'E' to loot";
    return true;
  }
}