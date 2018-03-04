using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts {
    public class SleepingBoxInteraction : CharacterInteraction
    {
        public float healthGain;
        public float sanityGain;
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
            SleepingSpot spot = new SleepingSpot();
            spot.healthGain = 5;
            spot.sanityGain = 5;
            var gc = GameController.instance;
            gc.sleep(spot);
            Debug.Log("Slept in the box");
            

        }
    }
}
