using SpriterDotNetUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : PausableObject {

  public Color color = Color.white;
  public float speed;
  public float oldSpeed;

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
  public override void OnPauseGame()
  {
    if (ignoreOnPause)
      return;
    base.OnPauseGame();
    oldSpeed = spriterAnimator.Speed;
    spriterAnimator.Speed = 0;
  }
  public override void OnUnpauseGame()
  {
    if (ignoreOnPause)
    {
      ignoreOnPause = false;
      return;
    }
    base.OnUnpauseGame();
    spriterAnimator.Speed = oldSpeed;
  }
}
