using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteraction : InteractionHandler {

  public override void interact() {
    text.text = "This is a character text";
    Debug.Log("Character Interaction");
    endInteraction();
  }

}
