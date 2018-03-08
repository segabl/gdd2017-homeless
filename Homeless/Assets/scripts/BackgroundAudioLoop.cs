using UnityEngine;

public class BackgroundAudioLoop : MonoBehaviour {

  public AudioClip audioClip;
  public float loopStart;
  public float loopEnd;

  private AudioSource audioSource;

  private float fadeFrom = 0;
  private float fadeTo = 1;
  private float fadeDelay = 0;
  private float fadeTime = -1;

  // Use this for initialization
  void Awake() {
    audioSource = gameObject.AddComponent<AudioSource>();
    audioSource.clip = audioClip;
    audioSource.playOnAwake = false;
    if (loopEnd <= loopStart) {
      loopEnd = audioSource.clip.length;
    }
    DontDestroyOnLoad(gameObject);
  }

  // Update is called once per frame
  void Update() {
    if (audioSource.time >= loopEnd) {
      audioSource.time = loopStart + audioSource.time - loopEnd;
      if (!audioSource.isPlaying) {
        audioSource.Play();
      }
    }
    if (Time.time <= fadeTime + fadeDelay) {
      audioSource.volume = Mathf.Lerp(fadeFrom, fadeTo, (Time.time - fadeTime) / fadeDelay);
    }
    else if (audioSource.volume != fadeTo) {
      audioSource.volume = fadeTo;
    }
    if (audioSource.clip != audioClip && Time.time > fadeTime + fadeDelay) {
      audioSource.clip = audioClip;
      audioSource.Play();
      fadeToVolume(fadeFrom, fadeDelay);
    }
  }
  public void fadeToVolume(float volume, float time) {
    fadeTime = Time.time;
    fadeDelay = time;
    fadeFrom = audioSource.volume;
    fadeTo = volume;
  }

  public void fadeToAudioClip(AudioClip clip, float time) {
    if (clip == audioClip) {
      return;
    }
    audioClip = clip;
    fadeTime = Time.time;
    fadeDelay = time;
    fadeFrom = audioSource.volume;
    fadeTo = 0;
  }

  public void play() {
    audioSource.Play();
  }

  public void stop() {
    audioSource.Stop();
  }

  public void pause() {
    audioSource.Pause();
  }
}
