using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaraCharacterInteraction : CharacterInteraction {

  public void runOverByTrainN() {
    GameController.instance.train(true);
    GameController.instance.karmaController.SocialAction(GameController.instance.player, KarmaSystem.SocialConstants.letNpcDieNegative);
  }

  public void runOverByTrainHalt() {
    GameController.instance.train(true);
    GameController.instance.karmaController.SocialAction(GameController.instance.player, KarmaSystem.SocialConstants.letNpcDieNeutral);
  }

  public void draggedAwayFromTrain() {
    GameController.instance.train(false);
    GameController.instance.karmaController.SocialAction(GameController.instance.player, KarmaSystem.SocialConstants.saveNpcFromDying);
  }

  public void runningAway() {
    GetComponent<ClaraRunAwayMovement>().movementSpeed = 4.0f;
    GetComponent<ClaraRunAwayMovement>().hideoutPosition = new Vector3(0,0,0);
  }

}
