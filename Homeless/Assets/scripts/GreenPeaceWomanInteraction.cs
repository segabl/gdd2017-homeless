﻿using KarmaSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts {
  class GreenPeaceWomanInteraction : CharacterInteraction {
    public GameObject Reward;

    public void TradeTrash() {
      Inventory player_inventory = GameController.instance.player.GetComponent<Inventory>();
      Inventory this_inventory = gameObject.GetComponent<Inventory>();
      if (player_inventory.giveItem(player_inventory.findMatch(Collectible.Type.TRASH), this_inventory)) {
        GameObject drop = Instantiate(Reward, transform.position + new Vector3(0, -1.5f, 0), Quaternion.identity);
        drop.name = drop.name.Replace("(Clone)", "");
        SetNextTree("hasTrash");
      }
    }
  }
}
