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

  // Use this for initialization
  void Start () {
    spriterAnimator = gameObject.GetComponentInChildren<SpriterDotNetBehaviour>().Animator;
    spriterAnimator.Play("idle");
    currentAnimation = "idle";
    setAnimation = "idle";
    animFinishedSetCurrent = f => currentAnimation = "NONE";
    animFinishedSetSetAnim = f => setAnimation = "idle";
  }
	
  void Update() {
    playCurrentAnimation();
  }

  public void playCurrentAnimation() {
    if (!currentAnimation.Equals(setAnimation)) {
      //delete Actions from Animation Finished which are possibly added earlier
      spriterAnimator.AnimationFinished -= animFinishedSetCurrent;
      spriterAnimator.AnimationFinished -= animFinishedSetSetAnim;
      Debug.Log("Starting new animation: " + setAnimation +  " " + currentAnimation);
      spriterAnimator.Play(setAnimation);
      //TODO: adjust speed for animations in a more sophisticated way
      spriterAnimator.Speed = 1.6f;
      currentAnimation = setAnimation;
    }
  }

  public void playOnce(String animation) {
    //delete Actions from Animation Finished which are possibly added earlier
    spriterAnimator.AnimationFinished -= animFinishedSetCurrent;
    spriterAnimator.AnimationFinished -= animFinishedSetSetAnim;
    spriterAnimator.Play(animation);
    //add Actions to Animation Finished
    spriterAnimator.AnimationFinished += animFinishedSetCurrent;
    spriterAnimator.AnimationFinished += animFinishedSetSetAnim;
  }
}
