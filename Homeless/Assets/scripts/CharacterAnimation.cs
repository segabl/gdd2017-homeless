using SpriterDotNetUnity;
using System;

public class CharacterAnimation : PausableAnimatedObject {

  public String currentAnimation;
  public String previousAnimation;

  private bool oncePlaying;

  // Use this for initialization
  void Start() {
    currentAnimation = "idle";
    previousAnimation = "idle";
    oncePlaying = false;
  }

  protected override void updatePausable() {
    if (spriterAnimator == null) {
      spriterAnimator = gameObject.GetComponentInChildren<SpriterDotNetBehaviour>().Animator;
      spriterAnimator.AnimationFinished += onAnimationFinished;
      spriterAnimator.Play(currentAnimation);
    }
    if (spriterAnimator.Name == currentAnimation) {
      return;
    }

    spriterAnimator.Play(currentAnimation);
    if (currentAnimation.Equals("idle")) {
      spriterAnimator.Speed = 0.7f;
    } else {
      spriterAnimator.Speed = 1.6f;
    }
  }

  public void playOnce(String animation, String nextAnimation) {
    if (!currentAnimation.Equals(animation)) {
      previousAnimation = nextAnimation;
      currentAnimation = animation;
      oncePlaying = true;
    }
  }

  private void onAnimationFinished(string animation) {
    if (oncePlaying) {
      oncePlaying = false;
      if (previousAnimation.Equals("NONE")) {
        spriterAnimator.Speed = 0;
      } else {
        currentAnimation = previousAnimation;
      }
    }
  }



}
