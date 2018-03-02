using SpriterDotNetUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Car : PausableObject {

  public Color color = Color.white;

  private UnityAnimator spriterAnimator;

  void Start() {
    spriterAnimator = gameObject.GetComponentInChildren<SpriterDotNetBehaviour>().Animator;
    spriterAnimator.Speed = 0.0f;
    foreach (SpriteRenderer renderer in gameObject.GetComponentsInChildren<SpriteRenderer>()) {
      if (renderer.sprite.name == "color") {
        renderer.color = color;
      }
    }
  }

  protected override void updatePausable() {

  }
}
