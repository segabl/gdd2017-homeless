using SpriterDotNetUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : PausableAnimatedObject {

  public String currentAnimation { get; private set; }
  public String setAnimation { get; set; }

  private Action<string> animFinishedSetCurrent = null;
  private Action<string> animFinishedSetSetAnim = null;
  private Action<string> stopAnimation = null;
  private Action<string> oncePlayingStopped = null;
  private float animationSpeed;
  private bool oncePlaying;

  // Use this for initialization
  void Start () {
    currentAnimation = "idle";
    setAnimation = "idle";
    animFinishedSetCurrent = f => currentAnimation = "NONE";
    animFinishedSetSetAnim = f => setAnimation = "idle";
    oncePlaying = false;
  }
	
  protected override void updatePausable() {
    playCurrentAnimation();
  }

  public void playCurrentAnimation() {
    if (spriterAnimator == null) {
      spriterAnimator = gameObject.GetComponentInChildren<SpriterDotNetBehaviour>().Animator;
      spriterAnimator.Play("idle");
    }
    if (oncePlaying || currentAnimation.Equals(setAnimation)) {
      return;
    }
    else if (currentAnimation.Equals("NONE") && setAnimation.Equals("NONE")) {
      detachAnimationFinishedActions();
      return;
    }
    detachAnimationFinishedActions();
    //TODO: adjust speed for animations in a more sophisticated way
    spriterAnimator.Speed = 1.6f;
    spriterAnimator.Play(setAnimation);
    detachAnimationFinishedActions();
    currentAnimation = setAnimation;
  }

  public void playOnce(String animation, String nextAnimation) {
    detachAnimationFinishedActions();

    currentAnimation = animation;
    setAnimation = animation;
    oncePlaying = true;
    spriterAnimator.Speed = 1.6f;

    spriterAnimator.Play(animation);

    animFinishedSetCurrent = f => currentAnimation = "NONE";
    animFinishedSetSetAnim = f => setAnimation = nextAnimation;
    stopAnimation = f => spriterAnimator.Speed = 0;
    oncePlayingStopped = f => oncePlaying = false;

    attachAnimationFinishedActions();
  }

  //attach Actions to Animation Finished
  private void attachAnimationFinishedActions() {
    spriterAnimator.AnimationFinished += animFinishedSetCurrent;
    spriterAnimator.AnimationFinished += animFinishedSetSetAnim;
    spriterAnimator.AnimationFinished += stopAnimation;
    spriterAnimator.AnimationFinished += oncePlayingStopped;
  }

  //delete Actions from Animation Finished which are possibly added earlier
  private void detachAnimationFinishedActions() {
    spriterAnimator.AnimationFinished -= animFinishedSetCurrent;
    spriterAnimator.AnimationFinished -= animFinishedSetSetAnim;
    spriterAnimator.AnimationFinished -= stopAnimation;
    spriterAnimator.AnimationFinished -= oncePlayingStopped;
  }



}
