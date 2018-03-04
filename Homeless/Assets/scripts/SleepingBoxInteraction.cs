using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts {
    public class SleepingBoxInteraction : CharacterInteraction
    {

        public String hasPermission()
        {
            if(GameController.instance.player.GetComponent<Character>().permisionToSleepInBox)
            {
                return "Y";
            }
            return "N";
        }
        public void Sleep()
        {
            GameController.instance.player.GetComponent<Character>().permisionToSleepInBox = false;
            SetNextTree("default");
            
            Debug.Log("Slept in the box");
            //TODO implement sleeping functionality

        }
    }
}
