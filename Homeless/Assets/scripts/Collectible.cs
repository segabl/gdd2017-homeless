using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour{

  public Sprite sprite { get; set; }
  public int inventoryIndex { get; set; }
  public float repletion;
  public float health;
  public float sanity;
  public float intoxication;
  public bool consumable;

  void Start () {
    this.name = name;
    this.sprite = sprite;
    this.inventoryIndex = -1;
    SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
    if (spriteRenderer == null) {
      Debug.Log("SpriteRenderer not found, can't set Sprite to Collectible. Item won't show up inventory.");
    }
    else {
      this.sprite = spriteRenderer.sprite;
    }
  }

  public bool use(Character character) {
    //If the influence on stat value is positive and the characters stat value wouldn't exceed the max stat value add stat value influence of item
    //Else if cut the characters repletion to max
    if (this.consumable) {
      character.adjustStats(this.repletion, this.health, this.sanity, this.intoxication);
      Debug.Log("Increase " + character.name + "'s Repletion by " + repletion + ", Health by " + health + ", Sanity by " + sanity + ", Intoxication level by " + intoxication);
      Debug.Log(character.name + "'s Repletion is " + character.repletion + ", Health is " + character.health + ", Sanity is " + character.sanity + ", Intoxication level is " + character.intoxication);
      return true;
    }

    //returns false if item can't be used
    Debug.Log("Can't use " + this.name + " at the moment");
    return false;
  }

}
