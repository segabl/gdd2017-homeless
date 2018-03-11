using SpriterDotNetUnity;
using System;

public class CharacterAnimation : PausableAnimatedObject {

  public String currentAnimation { get; set; }
  public String followUpAnimation { get; protected set; }

  public bool oncePlaying { get; private set; }

  // Use this for initialization
  void Start() {
    currentAnimation = "idle";
    followUpAnimation = "idle";
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
    if (currentAnimation.Equals("idle") || currentAnimation.Equals("sitting")) {
      spriterAnimator.Speed = 0.7f;
    } else {
      spriterAnimator.Speed = 1.6f;
    }
  }

  public void playOnce(String animation, String next = null) {
    if (!currentAnimation.Equals(animation)) {
      followUpAnimation = next;
      currentAnimation = animation;
      oncePlaying = true;
    }
  }

  private void onAnimationFinished(string animation) {
    if (oncePlaying)
    {
      oncePlaying = false;
      if (followUpAnimation == null)
      {
        spriterAnimator.Speed = 0;
      }
      else if (currentAnimation == "draw_gun")
      {
        currentAnimation = "shoot";
        followUpAnimation = "idle";
        oncePlaying = true;
      }
      else
      {
        currentAnimation = followUpAnimation;
      }
    }
  }



}
