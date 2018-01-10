using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

  void Update () {
    GameController controller = GameController.instance;
    if (Input.GetKeyDown(KeyCode.Escape)) { 
      Debug.Log("ESC Key pressed");
      controller.toggleActiveStateForMenuCanvasPanel(controller.panelInGameMenu);
      if (controller.panelInGameMenu.activeSelf) {
        controller.pauseAll();
        controller.deactivateChildsOfMenuPanels(controller.panelInGameMenu);
        controller.saveGame();
      }
      else {
        controller.unpauseAll();
      }
    }


    if (Input.GetKeyDown(KeyCode.I) && !controller.panelInGameMenu.activeSelf) {
      Debug.Log("I Key pressed");
      controller.toggleActiveStateForMenuCanvasPanel(controller.panelInventory);
      if (controller.panelInventory.activeSelf) {
        controller.pauseAll();
      }
      else {
        controller.unpauseAll();
      }
    }

    if (controller.paused) {
      return;
    }

    if(Input.GetKeyDown(KeyCode.E)) {
      Debug.Log("E Key pressed");
        InteractionHandler.interactObject.interact();
    }    
  }

}
