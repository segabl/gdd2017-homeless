using SpriterDotNetUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour {

  private String currentAnimation;
  public String setAnimation { get; set; }
  private UnityAnimator spriterAnimator;
  private Action<string> animFinishedSetCurrent = null;
  private Action<string> animFinishedSetSetAnim = null;
  private Action<string> stopAnimation = null;

  // Use this for initialization
  void Start () {
    currentAnimation = "idle";
    setAnimation = "idle";
    animFinishedSetCurrent = f => currentAnimation = "NONE";
    animFinishedSetSetAnim = f => setAnimation = "idle";
  }
	
  void Update() {
    playCurrentAnimation();
  }

  public void playCurrentAnimation() {
    if (spriterAnimator == null) {
      spriterAnimator = gameObject.GetComponentInChildren<SpriterDotNetBehaviour>().Animator;
      spriterAnimator.Play("idle");
    }
    if (!currentAnimation.Equals(setAnimation) && !setAnimation.Equals("NONE")) {
      detachAnimationFinishedActions();
      spriterAnimator.Play(setAnimation);
      //TODO: adjust speed for animations in a more sophisticated way
      spriterAnimator.Speed = 1.6f;
      currentAnimation = setAnimation;
    } else if (currentAnimation.Equals("NONE") && setAnimation.Equals("NONE")) {
      detachAnimationFinishedActions();
    }
  }

  public void playOnce(String animation, String nextAnimation) {
    detachAnimationFinishedActions();
    spriterAnimator.Play(animation);
    animFinishedSetCurrent = f => currentAnimation = "NONE";
    animFinishedSetSetAnim = f => setAnimation = nextAnimation;
    stopAnimation = f => spriterAnimator.Speed = 0;
    attachAnimationFinishedActions();
  }

  //attach Actions to Animation Finished
  private void attachAnimationFinishedActions() {
    spriterAnimator.AnimationFinished += animFinishedSetCurrent;
    spriterAnimator.AnimationFinished += animFinishedSetSetAnim;
    spriterAnimator.AnimationFinished += stopAnimation;
  }

  //delete Actions from Animation Finished which are possibly added earlier
  private void detachAnimationFinishedActions() {
    spriterAnimator.AnimationFinished -= animFinishedSetCurrent;
    spriterAnimator.AnimationFinished -= animFinishedSetSetAnim;
    spriterAnimator.AnimationFinished -= stopAnimation;
  }
}
