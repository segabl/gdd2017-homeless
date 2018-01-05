using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundAudioLoop : MonoBehaviour {

  public float loopStart;
  public float loopEnd;

  private AudioSource audioSource;

  private float fadeFrom = 0;
  private float fadeTo = 1;
  private float fadeDelay = 0;
  private float fadeTime = -1;

  // Use this for initialization
  void Start () {
    audioSource = gameObject.GetComponent<AudioSource>();
    if (loopEnd <= loopStart) {
      loopEnd = audioSource.clip.length;
    }
    DontDestroyOnLoad(gameObject);
  }

  // Update is called once per frame
  void Update () {
    if (audioSource.time >= loopEnd) {
      audioSource.time = loopStart + audioSource.time - loopEnd;
      if (!audioSource.isPlaying) {
        audioSource.Play();
      }
    }
    if (Time.time <= fadeTime + fadeDelay) {
      audioSource.volume = Mathf.Lerp(fadeFrom, fadeTo, (Time.time - fadeTime) / fadeDelay);
    } else if (audioSource.volume != fadeTo) {
      audioSource.volume = fadeTo;
    }
  }
  public void fadeToVolume(float volume, float time) {
    fadeTime = Time.time;
    fadeDelay = time;
    fadeFrom = audioSource.volume;
    fadeTo = volume;
  }
}
