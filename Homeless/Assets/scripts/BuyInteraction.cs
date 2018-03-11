using KarmaSystem;
using UnityEngine;
using System;

namespace Assets.scripts {
  class BuyInteraction : InteractionHandler {
    protected bool noMoney;
    Action action;
    public GameObject item;
    public override void interact() {
      Inventory player_inventory = GameController.instance.player.GetComponent<Inventory>();
      Inventory this_inventory = gameObject.GetComponent<Inventory>();
      action = () => resetInteraction();
      if (player_inventory.findMatch("Money") && player_inventory.giveItem(player_inventory.findMatch("Money"), this_inventory))
      {
        GameObject drop = Instantiate(item, transform.position + new Vector3(0, -0.8f, 0), Quaternion.identity);
        drop.name = drop.name.Replace("(Clone)", "");
        drop.GetComponent<Collectible>().Start();
        drop.GetComponent<ItemInteraction>().Start();
        drop.GetComponent<ItemInteraction>().interact();
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
        interactText.text = "Not enough money";
      else
        interactText.text = "Press 'E' to buy a " + item.name;
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
