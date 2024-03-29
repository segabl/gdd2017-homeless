﻿using System;
using System.Reflection;
using UnityEngine;

public class CharacterInteraction : InteractionHandler {

  [SerializeField]
  Dialogues npc;

  protected string nextTree = "";
  protected int interactions = 0;
  protected const String DECISION_YES = "Y";
  protected const String DECISION_NO = "N";

  public override void interact() {
    Debug.Log("Character Interaction");
    onCharacterInteractionStart();

    if (interactions == 0) {
      npc.SetTree(npc.TabList[0]);
    }
    else if (nextTree != "") {
      npc.SetTree(nextTree);
      nextTree = "";
    }
    else {
      npc.Next();
    }

    Display();
    onCharacterInteractionEnd();
    interactions++;
  }

  public void Choice(int index) {
    Choice(npc.GetChoices()[index]);
  }

  public void Choice(String choice) {
    npc.NextChoice(choice);
    Display();
  }

  public void SetNextTree(string treeName) {
    nextTree = treeName;
  }

  public String HasItemOfName(String itemName) {
    Inventory player_inventory = GameController.instance.player.GetComponent<Inventory>();
    if (player_inventory.findMatch(itemName)) {
      return DECISION_YES;
    }
    return DECISION_NO;
  }

  public String HasItemOfType(String itemType) {
    Inventory player_inventory = GameController.instance.player.GetComponent<Inventory>();
    if (player_inventory.findMatch((Collectible.Type)Enum.Parse(typeof(Collectible.Type), itemType))) {
      return DECISION_YES;
    }
    return DECISION_NO;
  }

  public String isPlayerTrustedBy(String npc_name) {
    GameObject npc = GameObject.Find(npc_name);
    if (GameController.instance.karmaController.aTrustsB(npc, GameController.instance.player)) {
      return DECISION_YES;
    }
    return DECISION_NO;
  }
  public String isPlayerLikedBy(String npc_name) {
    GameObject npc = GameObject.Find(npc_name);
    if (GameController.instance.karmaController.aLikesB(npc, GameController.instance.player)) {
      return DECISION_YES;
    }
    return DECISION_NO;
  }
  public String isPlayerCharitable() {
    if (GameController.instance.karmaController.isCharitable(GameController.instance.player))
      return DECISION_YES;
    return DECISION_NO;
  }
  public String isPlayerReliable() {
    if (GameController.instance.karmaController.isReliable(GameController.instance.player))
      return DECISION_YES;
    return DECISION_NO;
  }
  public String isPlayerCriminal() {
    if (GameController.instance.karmaController.isCriminal(GameController.instance.player))
      return DECISION_YES;
    return DECISION_NO;
  }
  public String isPlayerCruel() {
    if (GameController.instance.karmaController.isCruel(GameController.instance.player))
      return DECISION_YES;
    return DECISION_NO;
  }

  public void Display() {
    handleTrigger();
    if (npc.GetChoices().Length > 0) {
      GameController.instance.modalPanel.MessageBox(npc.GetCurrentDialogue(), Choice, npc.GetChoices());
    }
    else {
      interactText.text = npc.GetCurrentDialogue();
    }
  }

  protected void onCharacterInteractionStart() {

  }

  protected void onCharacterInteractionEnd() {

  }

  private void handleTrigger() {
    if (npc.HasTrigger()) {
      String res = callDecisionFunction(npc.GetTrigger());
      if (npc.GetChoices().Length > 0) {
        npc.NextChoice(res);
        handleTrigger();
        return;
      }
    }
  }

  private String callDecisionFunction(String triggerString) {
    String[] split = triggerString.Split(',');
    object[] parameters = new object[split.Length - 1];
    Array.Copy(split, 1, parameters, 0, parameters.Length);
    Type thisType = this.GetType();
    MethodInfo method = thisType.GetMethod(split[0]);
    return (string)method.Invoke(this, parameters);
  }

  protected override bool displayInteractionText()
  {
    interactText.text = "Press 'E' to talk to " + this.name;
    return true;
  }
}
