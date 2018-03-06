using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingSoundPlayer : PausableObject {

  public AudioClip[] walks;
  private int currentSound;
  public AudioSource audioSource;

  // Use this for initialization
  void Start () {
    audioSource = GetComponent<AudioSource>();
    walks = new AudioClip[]{(AudioClip)Resources.Load("sfx/walking/footstep1"),
                                     (AudioClip)Resources.Load("sfx/walking/footstep2"),
                                     (AudioClip)Resources.Load("sfx/walking/footstep3"),
                                     (AudioClip)Resources.Load("sfx/walking/footstep4"),
                                     (AudioClip)Resources.Load("sfx/walking/footstep5"),
                                     (AudioClip)Resources.Load("sfx/walking/footstep6")};
    currentSound = 0;
  }

  protected override void updatePausable() {
    bool walking = GetComponent<MainCharacterMovement>().walking;
    bool carHit = GetComponent<CarHit>().hit;
    if (walking && !audioSource.isPlaying && !carHit) {
      System.Random rnd = new System.Random();
      int clip = rnd.Next(0, 6);
      audioSource.clip = walks[clip];
      audioSource.Play();
    }
  }
}
