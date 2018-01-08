using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuListener : MonoBehaviour {

  public GameObject inGameMenu;

  void Update () {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      pauseAll();
      Debug.Log("ESC Key pressed");
      inGameMenu.SetActive(!inGameMenu.activeSelf);
      if (inGameMenu.activeSelf) {
        GameController.instance.saveGame();
      }
    }
  }

  private void pauseAll() {
    UnityEngine.Object[] objects = FindObjectsOfType(typeof(PausableObject));
    foreach (PausableObject pausableObject in objects) {
      pausableObject.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
    }
  }
}
