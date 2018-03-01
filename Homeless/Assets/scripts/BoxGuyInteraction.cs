﻿using KarmaSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
    class BoxGuyInteraction : CharacterInteraction
    {
        public void giveFoodY()
        {
            Inventory player_inventory = GameController.instance.player.GetComponent<Inventory>();
            Inventory this_inventory = gameObject.GetComponent<Inventory>();
            if (player_inventory.giveItem("Food", this_inventory))
            {
                //GameController.instance.karmaController.SocialAction(GameController.instance.player, SocialConstants.sharingBeer, gameObject);
                GameController.instance.player.GetComponent<Character>().permisionToSleepInBox = true;
                Debug.Log("Shared some food");
            }
            else
            {
                Debug.LogWarning("Couldn't give Food to BoxGuy");
            }

            GameController.instance.karmaController.DebugKarmaList();
            SetNextTree("HappyBoxGuy");
        }

        public void giveFoodN()
        {
            GameController.instance.player.GetComponent<Character>().permisionToSleepInBox = false;
        }
    }
}