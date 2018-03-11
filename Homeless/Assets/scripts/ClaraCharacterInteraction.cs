using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaraCharacterInteraction : CharacterInteraction {

  public void runOverByTrainN() {
    GameController.instance.pauseGameAndBlend(GameController.PauseReason.TRAIN, true);
    GameController.instance.karmaController.SocialAction(GameController.instance.player, KarmaSystem.SocialConstants.letNpcDieNegative);
  }

  public void runOverByTrainHalt() {
    GameController.instance.pauseGameAndBlend(GameController.PauseReason.TRAIN, true);
    GameController.instance.karmaController.SocialAction(GameController.instance.player, KarmaSystem.SocialConstants.letNpcDieNeutral);
  }

  public void draggedAwayFromTrain() {
    GameController.instance.pauseGameAndBlend(GameController.PauseReason.TRAIN);
    GameController.instance.karmaController.SocialAction(GameController.instance.player, KarmaSystem.SocialConstants.saveNpcFromDying);
  }

  public void runningAway() {
    GameController.instance.pauseGameAndBlend(GameController.PauseReason.RUNAWAY);
  }

}
