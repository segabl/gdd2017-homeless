using KarmaSystem;
using UnityEngine;
using System;

namespace Assets.scripts {
  class HotdogInteraction : InteractionHandler {
    protected bool noMoney;
    Action action;
    public GameObject hotdog;
    public override void interact() {
      Inventory player_inventory = GameController.instance.player.GetComponent<Inventory>();
      Inventory this_inventory = gameObject.GetComponent<Inventory>();
      action = () => resetInteraction();
      if (player_inventory.findMatch("Money") && player_inventory.giveItem(player_inventory.findMatch("Money"), this_inventory))
      {
        GameObject hotdog_instance = GameController.createItemInstance(hotdog);

        player_inventory.addItem(hotdog_instance.GetComponent<Collectible>());
        hotdog_instance.SetActive(false);
        GameController.instance.player.GetComponent<CharacterAnimation>().playOnce("give_front", "idle");
        endInteraction();
        
        suspendThenCall(2f, action);
      }
      else
      {
        noMoney = true;
        displayInteractionText();
        waitThenCall(2f, action);
      }

      
    }
    protected override bool displayInteractionText()
    {
      if (noMoney)
        interactionText.text = "Not enough money";
      else
        interactionText.text = "Press 'E' to buy a hotdog";
      return true;
    }
    protected void resetInteraction()
    {
      noMoney = false;
      Debug.Log("noMoney = false");
      displayInteractionText();
    }
  }
}
