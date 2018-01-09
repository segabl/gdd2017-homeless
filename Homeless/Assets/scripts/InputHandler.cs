using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

  void Update () {
    GameController instance = GameController.instance;
    if (Input.GetKeyDown(KeyCode.Escape)) { 
      Debug.Log("ESC Key pressed");
      GameObject menuCanvas = instance.menuCanvas;
      menuCanvas.SetActive(!menuCanvas.activeSelf);
      if (menuCanvas.activeSelf) {
        instance.pauseAll();
        instance.saveGame();
      }
      else {
        instance.unpauseAll();
      }
    }

    if (instance.paused) {
      return;
    }

    if (Input.GetKeyDown(KeyCode.I)) {
      Debug.Log("I Key pressed");
      //open inventory
    }
    if(Input.GetKeyDown(KeyCode.E)) {
      Debug.Log("E Key pressed");
        InteractionHandler.interactObject.interact();
    }    
  }

}
