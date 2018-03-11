using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaraCharacterInteraction : CharacterInteraction {

  public Collectible oldHotDog;
  public Collectible hotDog;

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
    GameController.instance.karmaController.SocialAction(GameController.instance.player, new KarmaSystem.SocialEffector(null, new KarmaSystem.RelationshipEffector(gameObject, 2, 2)), gameObject);
  }

  public void runningAwayY() {
    GameController.instance.pauseGameAndBlend(GameController.PauseReason.RUNAWAY);
    GameController.instance.karmaController.SocialAction(GameController.instance.player, KarmaSystem.SocialConstants.wantsToHelp);
    GameController.instance.karmaController.SocialAction(GameController.instance.player, new KarmaSystem.SocialEffector(null, new KarmaSystem.RelationshipEffector(gameObject, 2, 2)), gameObject);
  }

  public void runningAwayN() {
    GameController.instance.pauseGameAndBlend(GameController.PauseReason.RUNAWAY);
    GameController.instance.karmaController.SocialAction(GameController.instance.player, KarmaSystem.SocialConstants.doesntWantToHelp);
  }

  public void hasGuitar() {
    string hasGuitar = HasItemOfName("Guitar");
    if (hasGuitar == "Y" && GameController.instance.karmaController.aLikesB(gameObject, GameController.instance.player, 3)) {
      Debug.Log("Talkative");
      GetComponent<Dialogues>().SetTree("ClaraTalkative");
    } else if (hasGuitar == "Y" && GameController.instance.karmaController.aLikesB(gameObject, GameController.instance.player, 1)) {
      Debug.Log("Thankful");
      GetComponent<Dialogues>().SetTree("ClaraThankful");
    }
  }

  public void receiveHotDog() {
    var inventory = GameController.instance.player.GetComponent<Inventory>();
    inventory.giveItem(inventory.findMatch("Guitar"), GetComponent<Inventory>());
    GameController.instance.player.GetComponent<Inventory>().addItem(oldHotDog);
  }

  public void receiveGoodHotDog() {
    var inventory = GameController.instance.player.GetComponent<Inventory>();
    inventory.giveItem(inventory.findMatch("Guitar"), GetComponent<Inventory>());
    GameController.instance.player.GetComponent<Inventory>().addItem(hotDog);
  }

  public void disappear() {
    Destroy(this);
  }

  public void resetTalkative() {
    GameController.instance.karmaController.SocialAction(GameController.instance.player, new KarmaSystem.SocialEffector(null, new KarmaSystem.RelationshipEffector(gameObject, -1, -1)), gameObject);
    GetComponent<Dialogues>().Reset();
  }

}
