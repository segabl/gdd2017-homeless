using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible {

  public Collectible(string name, Sprite sprite) {
    this.name = name;
    this.sprite = sprite;
    this.inventoryIndex = -1;
  }

  public string name { get; set; }
  public Sprite sprite { get; set; }
  public int inventoryIndex { get; set; }

}
