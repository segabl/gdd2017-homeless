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
    if (Random.Range(0.0f, 1.0f) < yieldChance) {
      int num = Random.Range(0, items.Count);
      Instantiate(items[num], transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
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

