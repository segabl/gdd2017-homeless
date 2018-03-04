﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

  void Update () {
    GameController controller = GameController.instance;
    if (Input.GetKeyDown(KeyCode.Escape)) { 
      Debug.Log("ESC Key pressed");
      controller.player.GetComponent<MainCharacterMovement>().stopMovement();
      if (!controller.panelInGameMenu.activeSelf && !controller.panelInventory.activeSelf) {
        controller.pauseAll();
        controller.deactivateChildsOfMenuPanels(controller.panelInGameMenu);
        controller.saveGame();
        controller.panelInGameMenu.SetActive(!controller.panelInGameMenu.activeSelf);
      } else if (!controller.panelInGameMenu.activeSelf) {
        controller.panelInventory.SetActive(false);
        controller.unpauseAll();
      }
      else {
        controller.unpauseAll();
        controller.panelInGameMenu.SetActive(!controller.panelInGameMenu.activeSelf);
      }
    }


    if (Input.GetKeyDown(KeyCode.I) && !controller.panelInGameMenu.activeSelf) {
      Debug.Log("I Key pressed");
      controller.player.GetComponent<MainCharacterMovement>().stopMovement();
      controller.panelInventory.SetActive(!controller.panelInventory.activeSelf);
      if (controller.panelInventory.activeSelf) {
        controller.pauseAll();
      } else {
        controller.unpauseAll();
      }
    }

    if (controller.paused) {
      return;
    }

    if(Input.GetKeyDown(KeyCode.E)) {
      Debug.Log("E Key pressed");
      controller.player.GetComponent<MainCharacterMovement>().stopMovement();
      if (InteractionHandler.playerCanInteract)
      {
        InteractionHandler.interactObject.interact();
      }
    }
    if (Input.GetKeyDown(KeyCode.F))
      Debug.Log("F Key pressed");

    if (Input.GetKey(KeyCode.F))
    {
      
      //controller.player.GetComponent<MainCharacterMovement>().stopMovement();
      if (TheftHandler.playerCanSteal)
      {
        TheftHandler.playerIsStealing = true;
      }
    }
    /*
    if (Input.GetKey(KeyCode.F))
    {
      if (TheftHandler.playerCanSteal && TheftHandler.playerIsStealing)
      {
        TheftHandler.theftObject.pickPocket();
      }
    }
    */
    if (Input.GetKeyUp(KeyCode.F))
    {
      TheftHandler.playerIsStealing = false;
    }

  }

}
