using System;
using System.Collections.Generic;
using UnityEngine;


public class TrashcanInteraction : InteractionHandler {

  public List<GameObject> items;
  public float startChance;
  public float chanceIncrement;
  private float yieldChance;

  public void Start() {
    yieldChance = startChance;
  }

  public override void interact() {
    if (yieldChance < 0.1) {
      interactText.text = "Nothing of use here...";
      return;
    }
    if (UnityEngine.Random.Range(0.0f, 1.0f) < yieldChance) {
      int num = UnityEngine.Random.Range(0, items.Count);
      GameObject drop = Instantiate(items[num], transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
      drop.name = drop.name.Replace("(Clone)", "");
      suspend(3.5f);
      suspendWhileActive(drop);
    }
    else {
      interactText.text = "Nothing of use here...";
    }
    yieldChance = 0;
  }

  protected override void updatePausable() {
    base.updatePausable();
    if (yieldChance < 1) {
      yieldChance += Time.deltaTime * chanceIncrement;
    }
  }

  protected override bool displayInteractionText()
  {
    interactText.text = "Press 'E' to search the trash";
    return true;
  }
}

