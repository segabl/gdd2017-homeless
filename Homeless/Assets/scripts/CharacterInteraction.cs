using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KarmaSystem;
using UnityEngine.UI;
using System.Reflection;

public class CharacterInteraction : InteractionHandler {

  [SerializeField]
  Dialogues npc;
  //[SerializeField]
  //Text dialogueText;

  int interactions = 0;

  public override void interact() {
    Debug.Log("Character Interaction");
    if (interactions == 0) {
      npc.SetTree("default");
    } else {
      if (npc.GetChoices().Length > 0) {
        npc.NextChoice(npc.GetChoices()[0]);
      } else {
        npc.Next();
      }
    }

    Display();
    interactions++;
  }

  public void Choice(int index) {
    npc.NextChoice(npc.GetChoices()[index]); 
    Display();
  }

  private void handleTrigger() {
    if (npc.HasTrigger()) {
      int res = callDecisionFunction(npc.GetTrigger());
      if (npc.GetChoices().Length > 0) {
        npc.NextChoice(npc.GetChoices()[res]);
        handleTrigger();
        return;
      }
    }
  }

  public void Display() {
    handleTrigger();
    text.text = npc.GetCurrentDialogue();
  //  if (nextEnd == true) {
  //    backPanel.SetActive(false);
  //    nextTreeButton.SetActive(true);
  //  } else {
  //    backPanel.SetActive(true);
  //    nextTreeButton.SetActive(false);
  //  }

  //  //Sets our text to the current text
  //  dialogueText.text = npc.GetCurrentDialogue();
  //  //Just debug log our triggers for example purposes
  //  if (npc.HasTrigger())
  //    Debug.Log("Triggered: " + npc.GetTrigger());
  //  //This checks if there are any choices to be made
  //  if (npc.GetChoices().Length != 0) {
  //    //Setting the text's of the buttons to the choices text, in my case I know I'll always have a max of three choices for this example.
  //    leftText.text = npc.GetChoices()[0];
  //    middleText.text = npc.GetChoices()[1];
  //    //If we only have two choices, adjust accordingly
  //    if (npc.GetChoices().Length > 2)
  //      rightText.text = npc.GetChoices()[2];
  //    else
  //      rightText.text = npc.GetChoices()[1];
  //    //Setting the appropriate buttons visability
  //    leftText.transform.parent.gameObject.SetActive(true);
  //    rightText.transform.parent.gameObject.SetActive(true);
  //    if (npc.GetChoices().Length > 2)
  //      middleText.transform.parent.gameObject.SetActive(true);
  //    else
  //      middleText.transform.parent.gameObject.SetActive(false);
  //  } else {
  //    middleText.text = "Continue";
  //    //Setting the appropriate buttons visability
  //    leftText.transform.parent.gameObject.SetActive(false);
  //    rightText.transform.parent.gameObject.SetActive(false);
  //    middleText.transform.parent.gameObject.SetActive(true);
  //  }

  //  if (npc.End()) //If this is the last dialogue, set it so the next time we hit "Continue" it will hide the panel
  //    nextEnd = true;
  }

  private int callDecisionFunction(String functionName) {
    Type thisType = this.GetType();
    MethodInfo method = thisType.GetMethod(functionName);
    int result = (int) method.Invoke(this, new object[0]);
    return result;
  }
}
