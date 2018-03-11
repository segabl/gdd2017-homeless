using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaraCharacterInteraction : CharacterInteraction {

  public GameObject oldHotDog;
  public GameObject hotDog;

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
    GameObject item = Instantiate(oldHotDog, transform.position + new Vector3(0, -0.8f, 0), Quaternion.identity);
    item.name = item.name.Replace("(Clone)", "");
    item.GetComponent<Collectible>().Start();
    item.GetComponent<ItemInteraction>().Start();
    item.GetComponent<ItemInteraction>().interact();
    GameController.instance.player.GetComponent<CharacterAnimation>().playOnce("idle", "idle");
  }

  public void receiveGoodHotDog() {
    var inventory = GameController.instance.player.GetComponent<Inventory>();
    inventory.giveItem(inventory.findMatch("Guitar"), GetComponent<Inventory>());
    GameObject item = Instantiate(hotDog, transform.position + new Vector3(0, -0.8f, 0), Quaternion.identity);
    item.name = item.name.Replace("(Clone)", "");
    item.GetComponent<Collectible>().Start();
    item.GetComponent<ItemInteraction>().Start();
    item.GetComponent<ItemInteraction>().interact();
    GameController.instance.player.GetComponent<CharacterAnimation>().playOnce("idle", "idle");
  }

  public void disappear() {
    GameController.instance.pauseGameAndBlend(GameController.PauseReason.RUNAWAY);
    Destroy(gameObject);
  }

  public void resetTalkative() {
    GameController.instance.karmaController.SocialAction(GameController.instance.player, new KarmaSystem.SocialEffector(null, new KarmaSystem.RelationshipEffector(gameObject, -1, -1)), gameObject);
    GetComponent<Dialogues>().Reset();
  }

  public void stillHasGuitar() {
    if (HasItemOfName("Guitar") == "N") {
      GetComponent<Dialogues>().SetTree("LostGuitar");
    }
  }

}
