using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaraCharacterInteraction : CharacterInteraction {

  public void runOverByTrain() {
    GameController.instance.train(true);
    //TODO: adjust Karma
  }

  public void draggedAwayFromTrain() {
    GameController.instance.train(false);
    //TODO: adjust Karma
  }
}
