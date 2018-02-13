using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : PausableObject {

  public float maxRepletion;
  public float maxHealth;
  public float maxSanity;
  public float maxIntoxication;
  public float repletion;
  public float health;
  public float sanity;
  public float intoxication { get; set; } 

  void Start () {
    intoxication = 0.0f;
	}

  protected override void updatePausable() {
    updateRepletion();
    updateHealth();
    updateSanity();
    updateIntoxication();
  }

  public void adjustStats(float repletionAdjustment, float healthAdjustment, float sanityAdjustment, float intoxicationAdjustment) {
    adjustRepletion(repletionAdjustment);
    adjustHealth(healthAdjustment);
    adjustSanity(sanityAdjustment);
    adjustIntoxication(intoxicationAdjustment);
  }

  //TODO: stats < 0 -> die
  private void adjustRepletion(float repletionAdjustment) {
    this.repletion += repletionAdjustment;
    this.repletion = this.repletion > this.maxRepletion ? this.maxRepletion : this.repletion;
  }

  private void adjustHealth(float healthAdjustment) {
    this.health += healthAdjustment;
    this.health = this.health > this.maxHealth ? this.maxHealth : this.health;
  }

  private void adjustSanity(float sanityAdjustment) {
    this.sanity += sanityAdjustment;
    this.sanity = this.sanity > this.maxSanity ? this.maxSanity : this.sanity;
  }

  //TODO: intox to high -> die
  private void adjustIntoxication(float intoxicationAdjustment) {
    this.intoxication += intoxicationAdjustment;
  }

  private void updateRepletion() {
    //TODO some great update functionality
  }

  private void updateHealth() {
    //TODO some great update functionality
  }

  private void updateSanity() {
    //TODO some great update functionality
  }

  private void updateIntoxication() {
    //TODO some great update functionality
  }
}
