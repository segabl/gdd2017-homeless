using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameControllerHandler : PausableObject {

  public GameObject player;
  public GameObject screenCanvas;
  public GameObject menuCanvas;
  public float dayLength;

  void Awake () {
    GameController.instance.player = player;
    GameController.instance.dayLength = dayLength;
    GameController.instance.screenCanvas = screenCanvas;
    GameController.instance.menuCanvas = menuCanvas;
	}
	
	protected override void updatePausable () {
    GameController.instance.update();
	}

}
