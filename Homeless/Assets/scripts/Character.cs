﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : PausableObject {

  public float maxRepletion;
  public float maxHealth;
  public float maxSanity;
  public float maxIntoxication;
  public float repletion;
  public float health;
  public float sanity;
  public float intoxication { get; set; }
  private float accumulatedDeltaRepletion;
  private float accumulatedDeltaHealth;
  private float accumulatedDeltaSanity;
  private float accumulatedDeltaIntox;
  private bool alive;

  void Start () {
    alive = true;
    accumulatedDeltaRepletion = 0.0f;
    accumulatedDeltaHealth = 0.0f;
    accumulatedDeltaSanity = 0.0f;
    accumulatedDeltaIntox = 0.0f;
    intoxication = 0.0f;
	}

  protected override void updatePausable() {
    if (alive) {
      //separate accumulated deltas for now, idk I'm a bit tired
      accumulatedDeltaRepletion += Time.deltaTime;
      accumulatedDeltaHealth += Time.deltaTime;
      accumulatedDeltaSanity += Time.deltaTime;
      accumulatedDeltaIntox += Time.deltaTime;
      updateRepletion();
      updateHealth();
      updateSanity();
      updateIntoxication();
    }
    #if UNITY_EDITOR
      updateStatsDebugText();
    #endif
  }

  private void updateStatsDebugText() {
    if (this.name.Equals("MainCharacter")) {
      Text[] texts = GameController.instance.menuCanvas.GetComponentsInChildren<Text>();
      foreach (Text text in texts) {
        if (text.name.Equals("StatsDebugText")) {
          text.text = "Repletion is " + this.repletion + ", Health is " + this.health + ", Sanity is " + this.sanity + ", Intoxication level is " + this.intoxication;
        }
      }
    }
  }

  public void adjustStats(float repletionAdjustment, float healthAdjustment, float sanityAdjustment, float intoxicationAdjustment) {
    if (alive) {
      adjustRepletion(repletionAdjustment);
      adjustHealth(healthAdjustment);
      adjustSanity(sanityAdjustment);
      adjustIntoxication(intoxicationAdjustment);
    }
  }

  private void adjustRepletion(float repletionAdjustment) {
    this.repletion += repletionAdjustment;
    this.repletion = this.repletion > this.maxRepletion ? this.maxRepletion : this.repletion;
    if(this.repletion <= 0.0f) {
      die("Starvation");
    }
  }

  private void adjustHealth(float healthAdjustment) {
    this.health += healthAdjustment;
    this.health = this.health > this.maxHealth ? this.maxHealth : this.health;
    if (this.health <= 0.0f) {
      die("Bad health condition");
    }
  }

  private void adjustSanity(float sanityAdjustment) {
    this.sanity += sanityAdjustment;
    this.sanity = this.sanity > this.maxSanity ? this.maxSanity : this.sanity;
  }

  private void adjustIntoxication(float intoxicationAdjustment) {
    this.intoxication += intoxicationAdjustment;
    this.intoxication = this.intoxication < 0.0f ? 0.0f : this.intoxication;
    if(this.intoxication >= this.maxIntoxication) {
      die("Too high intoxication level");
    }
  }

  private void updateRepletion() {
    float minRef = accumulatedDeltaRepletion * 24.0f * 60.0f / GameController.instance.dayLength;
    if (minRef >= 1.0f) {
      adjustRepletion(-0.1f);
      accumulatedDeltaRepletion = 0.0f;
    }
  }

  private void updateHealth() {
    float healthDecrement = 0.0f;
    float minRef = accumulatedDeltaHealth * 24.0f * 60.0f / GameController.instance.dayLength;
    if (this.repletion <= 50.0f && minRef >= 1.0f) {
      healthDecrement -= (this.maxRepletion - this.repletion) / this.maxRepletion + (this.maxSanity - this.sanity) / this.maxSanity + this.intoxication / this.maxIntoxication;
      accumulatedDeltaHealth = 0.0f;
    }
    adjustHealth(healthDecrement);
  }

  private void updateSanity() {
    float sanityDecrement = 0.0f;
    float minRef = accumulatedDeltaSanity * 24.0f * 60.0f / GameController.instance.dayLength;
    if ((this.health <= 20.0f || this.repletion <= 50.0f) && minRef >= 1.0f) {
      sanityDecrement -= (this.maxHealth - this.health) / this.maxHealth + (this.maxRepletion - this.repletion) / this.maxRepletion - this.intoxication / this.maxIntoxication;
      accumulatedDeltaSanity = 0.0f;
    }
    adjustSanity(sanityDecrement);
  }

  private void updateIntoxication() {
    float hourRef = accumulatedDeltaIntox * 24 / GameController.instance.dayLength;
    if (hourRef >= 1.0f) {
      accumulatedDeltaIntox = 0.0f;
      float intoxicationDecrement = -0.1f;
      adjustIntoxication(intoxicationDecrement);
    }
  }

  private void die(String reason) {
    //TODO: switch to game over scene
    this.alive = false;
    Debug.Log("Player died because of: " + reason);
  }
}