using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameControllerHandler : PausableObject {

  public GameObject player;
  public float dayLength;

  void Start () {
    GameController.instance.player = player;
    GameController.instance.dayLength = dayLength;
	}
	
	protected override void updatePausable () {
    GameController.instance.update();
	}

}
