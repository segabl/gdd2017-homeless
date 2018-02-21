using System;
using System.Reflection;
using UnityEngine;

public class CharacterInteraction : InteractionHandler {

  const String DECISION_YES = "Y";
  const String DECISION_NO = "N";

  [SerializeField]
  Dialogues npc;

  string nextTree = "";
  int interactions = 0;

  public override void interact() {
    Debug.Log("Character Interaction");
    onCharacterInteractionStart();

    if (interactions == 0) {
      npc.SetTree(npc.TabList[0]);
    } else if (nextTree != "") {
      npc.SetTree(nextTree);
      nextTree = "";
    } else if (npc.GetChoices().Length > 0) {
      npc.NextChoice(npc.GetChoices()[0]);
    } else {
      npc.Next();
    }

    Display();
    onCharacterInteractionEnd();
    interactions++;
  }

  public void Choice(int index) {
    npc.NextChoice(npc.GetChoices()[index]); 
    Display();
  }

  public void SetNextTree(string treeName) {
    nextTree = treeName;
  }

  public String HasItem(String itemName) {
    Inventory player_inventory = GameController.instance.player.GetComponent<Inventory>();
    if (player_inventory.containsItem(itemName)) {
      return DECISION_YES;
    }
    return DECISION_NO;
  }

  public void Display() {
    handleTrigger();
    text.text = npc.GetCurrentDialogue();
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
    return (string) method.Invoke(this, parameters);
  }
}
