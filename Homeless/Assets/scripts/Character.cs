using System;
using UnityEngine;
using UnityEngine.UI;

public class Character : PausableObject {

  public float maxRepletion;
  public float maxHealth;
  public float maxSanity;
  public float maxIntoxication;
  [Range(0f, 100f)]
  public float repletion;
  [Range(0f, 100f)]
  public float health;
  [Range(0f, 100f)]
  public float sanity;
  public float intoxication { get; set; }
  public bool permisionToSleepInBox { get; set; }
  private float accumulatedDeltaRepletion;
  private float accumulatedDeltaHealth;
  private float accumulatedDeltaSanity;
  private float accumulatedDeltaIntox;
  public bool alive { get; private set; }
  public bool asleep { get; set; }
  private bool shot = false;
  private string shotReason;
  private float shotTime;
  private bool playedShotAnimation = false;

  void Start() {
    alive = true;
    accumulatedDeltaRepletion = 0.0f;
    accumulatedDeltaHealth = 0.0f;
    accumulatedDeltaSanity = 0.0f;
    accumulatedDeltaIntox = 0.0f;
    intoxication = 0.0f;
    permisionToSleepInBox = false;
    asleep = false;
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
    if (shot)
    {
      float diff = GameController.instance.dayTime - shotTime;
      if (diff > 0.8f / GameController.instance.dayLength && !playedShotAnimation)
      {
        this.alive = false;
        GameController.instance.player.GetComponent<CharacterAnimation>().playOnce("die");
      }
      if (diff > 1.4f / GameController.instance.dayLength)
      {
        GameController.instance.panelDead.SetActive(true);
        int days = GameController.instance.day;
        GameController.instance.panelDead.GetComponentInChildren<Text>().text = "You survived " + days + (days != 1 ? " days" : " day") + " before being shot for "
          + shotReason + ".";
      }
        
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
          text.text = "Repletion is " + roundDebug(repletion) + ", Health is " + roundDebug(health) + ", Sanity is " + roundDebug(sanity)
            + ", Intoxication level is " + roundDebug(intoxication);
        }
      }
    }
  }

  public void adjustStats(float repletionAdjustment, float healthAdjustment, float sanityAdjustment, float intoxicationAdjustment) {
    if (alive) {
      Debug.Log(name + " Repletion by " + repletionAdjustment + ", Health by " + healthAdjustment + ", Sanity by " + sanityAdjustment + ", Intoxication level by " + intoxicationAdjustment);
      adjustRepletion(repletionAdjustment);
      adjustHealth(healthAdjustment);
      adjustSanity(sanityAdjustment);
      adjustIntoxication(intoxicationAdjustment);
    }
  }

  private void adjustRepletion(float repletionAdjustment) {
    this.repletion += repletionAdjustment;
    this.repletion = this.repletion > this.maxRepletion ? this.maxRepletion : this.repletion;
    if (this.repletion <= 0.0f) {
      die("starvation");
    }
  }

  private void adjustHealth(float healthAdjustment) {
    this.health += healthAdjustment;
    this.health = this.health > this.maxHealth ? this.maxHealth : this.health;
    if (this.health <= 0.0f) {
      die("a bad health condition");
    }
  }

  private void adjustSanity(float sanityAdjustment) {
    this.sanity += sanityAdjustment;
    this.sanity = this.sanity > this.maxSanity ? this.maxSanity : this.sanity;
    if (sanity <= 0.0f) {
      die("lack of sanity");
    }
  }

  private void adjustIntoxication(float intoxicationAdjustment) {
    this.intoxication += intoxicationAdjustment;
    this.intoxication = this.intoxication < 0.0f ? 0.0f : this.intoxication;
    if (this.intoxication >= this.maxIntoxication) {
      die("too high intoxication level");
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
      healthDecrement -= ((this.maxRepletion - this.repletion) / this.maxRepletion + (this.maxSanity - this.sanity) / this.maxSanity + this.intoxication / this.maxIntoxication)*0.1f;
      accumulatedDeltaHealth = 0.0f;
    }
    adjustHealth(healthDecrement);
  }

  private void updateSanity() {
    float sanityDecrement = 0.0f;
    float minRef = accumulatedDeltaSanity * 24.0f * 60.0f / GameController.instance.dayLength;
    if ((this.health <= 20.0f || this.repletion <= 50.0f) && minRef >= 1.0f) {
      sanityDecrement -= ((this.maxHealth - this.health) / this.maxHealth + (this.maxRepletion - this.repletion) / this.maxRepletion - this.intoxication / this.maxIntoxication)*0.1f;
      accumulatedDeltaSanity = 0.0f;
    }
    adjustSanity(sanityDecrement);
  }

  private void updateIntoxication() {
    float hourRef = accumulatedDeltaIntox * 24 / GameController.instance.dayLength;
    if (hourRef >= 1.0f) {
      accumulatedDeltaIntox = 0.0f;
      adjustIntoxication(-0.1f);
    }
  }

  private void die(String reason) {
    this.alive = false;
    GameController.instance.player.GetComponent<CharacterAnimation>().playOnce("die");
    GameController.instance.panelDead.SetActive(true);
    int days = GameController.instance.day;
    GameController.instance.panelDead.GetComponentInChildren<Text>().text = "You survived " + days + (days != 1 ? " days" : " day") + " before dying due to " + reason;
  }
  private Decimal roundDebug(float f) {
    return System.Math.Round((Decimal)f, 1, MidpointRounding.AwayFromZero);
  }
  public void shoot(String reason)
  {
    shot = true;
    shotReason = reason;
    shotTime = GameController.instance.dayTime;
    
  }
}
