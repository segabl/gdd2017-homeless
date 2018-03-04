using SpriterDotNetUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : PausableObject {

  public Color color = Color.white;
  public float speed;

  private UnityAnimator spriterAnimator;

  void Start() {
    Vector3 scale = gameObject.transform.localScale;
    scale.x = speed > 0 ? 1 : -1;
    gameObject.transform.localScale = scale;
    foreach (SpriteRenderer renderer in gameObject.GetComponentsInChildren<SpriteRenderer>()) {
      if (renderer.sprite.name == "color") {
        renderer.color = color;
      }
    }
  }

  protected override void updatePausable() {
    if (spriterAnimator == null) {
      spriterAnimator = gameObject.GetComponentInChildren<SpriterDotNetBehaviour>().Animator;
    }
    spriterAnimator.Speed = -speed / 20;
    gameObject.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    // Todo: reset when outside world
  }
}
