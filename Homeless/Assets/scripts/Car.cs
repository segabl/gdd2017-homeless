using SpriterDotNetUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : PausableAnimatedObject {

  public Color color = Color.white;
  public float speed;
  private float oldSpeed;
  private bool stop;
  private bool emergencyStop;

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
      spriterAnimator.AnimationFinished += onAnimationFinished;
      spriterAnimator.Play("drive");
    }
    float deltaTime = Time.deltaTime;
    if (stop) {
      if (Mathf.Abs(speed) >= deltaTime * 20.0f) {
        speed -= Mathf.Sign(speed) * deltaTime * 20.0f;
      } else {
        speed = 0.0f;
      }
    }
    if (stop && emergencyStop) {

    } else {
      spriterAnimator.Speed = -speed / 20;
    }
    gameObject.transform.position += new Vector3(speed * deltaTime, 0, 0);
    // Todo: reset when outside world
  }

  private void onAnimationFinished(string animation) {
    if (animation == "break") {
      spriterAnimator.Speed = 0;
    }
  }

  public void stopMoving(bool emergency = false) {
    if (!stop) {
      oldSpeed = speed;
      stop = true;
      emergencyStop = emergency;
      if (emergencyStop) {
        spriterAnimator.Speed = 0.25f;
        spriterAnimator.Play("break");
      }
    }
  }
}
