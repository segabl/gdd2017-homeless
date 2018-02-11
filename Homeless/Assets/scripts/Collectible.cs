using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible {

  public Collectible(string name, SpriteRenderer spriteRenderer) {
    this.name = name;
    this.spriteRenderer = spriteRenderer;
    this.inventoryIndex = -1;
  }

  public string name { get; set; }
  public SpriteRenderer spriteRenderer { get; set; }
  public int inventoryIndex { get; set; }

}
