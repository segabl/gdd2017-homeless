using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
      interactionText.text = "Nothing of use here...";
      return;
    }
    if (Random.Range(0.0f, 1.0f) < yieldChance) {
      int num = Random.Range(0, items.Count);
      GameObject drop =  Instantiate(items[num], transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
      drop.name = drop.name.Replace("(Clone)", "");
    }
    else {
      interactionText.text = "Nothing of use here...";
    }
    yieldChance = 0;
  }

  protected override void updatePausable() {
    base.updatePausable();
    if (yieldChance < 1) {
      yieldChance += Time.deltaTime * chanceIncrement;
    }
  }
}

