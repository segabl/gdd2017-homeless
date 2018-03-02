using SpriterDotNetUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : PausableObject {

  private String currentAnimation;
  public String setAnimation { get; set; }
  private UnityAnimator spriterAnimator;

  // Use this for initialization
  void Start () {
    spriterAnimator = gameObject.GetComponentInChildren<SpriterDotNetBehaviour>().Animator;
    spriterAnimator.Play("idle");
    currentAnimation = "idle";
    setAnimation = "idle";
  }
	
  protected override void updatePausable() {
    playCurrentAnimation();
  }

  public void playCurrentAnimation() {
    if (!currentAnimation.Equals(setAnimation)) {
      spriterAnimator.Play(setAnimation);
      currentAnimation = setAnimation;
    }
  }
}
