using UnityEngine;

public class PostProcessing : MonoBehaviour {

  public Texture daylightTexture;
  public Shader daylightCycleShader;
  public Shader blurShader;
  public Shader redRadiationShader;
  public Shader swirlShader;
  public Shader pulseShader;
  public Shader blackFadeShader;
  public Shader strangeShader;

  public bool paused = false;
  public GameController.PauseReason pauseReason;

  private Material daylightCycleMaterial;
  private Material blurMaterial;
  private Material redRadiationMaterial;
  private Material swirlMaterial;
  private Material pulseMaterial;
  private Material blackFadeMaterial;
  private Material strangeMaterial;

  private float intoxicationTime;
  private float lowHealthTime;
  private float fadeTime;
  private float insanityTime;

  void Awake() {
    daylightCycleMaterial = new Material(daylightCycleShader);
    blurMaterial = new Material(blurShader);
    redRadiationMaterial = new Material(redRadiationShader);
    swirlMaterial = new Material(swirlShader);
    pulseMaterial = new Material(pulseShader);
    blackFadeMaterial = new Material(blackFadeShader);
    strangeMaterial = new Material(strangeShader);

    intoxicationTime = 0;
    lowHealthTime = 0;
    fadeTime = 0;
    insanityTime = 0;
  }

  // Postprocess the image
  void OnRenderImage(RenderTexture source, RenderTexture destination) {


    //GL.Clear(false, true, Color.black);




    ProcessDaylight(source, source);
    ProcessPlayerRepletion(source, source);
    ProcessPlayerHealth(source, source);
    ProcessPlayerSanity(source, source);
    ProcessPlayerIntoxication(source, source);
    ProcessSleep(source, source);


    Graphics.Blit(source, destination);




  }

  internal void ProcessPlayerRepletion(RenderTexture source, RenderTexture destination) {
    float repletion = GameController.instance.player.GetComponent<Character>().repletion;
    if (repletion <= 3f)
      repletion = 3f;
    if (repletion < 80.0f) {
      blurMaterial.SetFloat("hstep", 3.20f / repletion);
      blurMaterial.SetFloat("vstep", 1.80f / repletion);
      Graphics.Blit(source, destination, blurMaterial);
    }
  }

  internal void ProcessDaylight(RenderTexture source, RenderTexture destination) {
    daylightCycleMaterial.SetTexture("_daylight", daylightTexture);
    daylightCycleMaterial.SetFloat("_time", GameController.instance.dayTime);
    Graphics.Blit(source, destination, daylightCycleMaterial);
  }
  internal void ProcessPlayerHealth(RenderTexture source, RenderTexture destination) {
    float health = GameController.instance.player.GetComponent<Character>().health;
    if (health <= 5f)
      health = 5f;
    if (health < 80.0f) {
      redRadiationMaterial.SetFloat("_delta", (80 - health) / 160f);
      if (health < 50.0f) {
        if (lowHealthTime == 0)
          lowHealthTime = Time.fixedTime;
        float timeDiff = (Time.fixedTime - lowHealthTime) / 1440f;

        float strength, speed, repletion;
        repletion = GameController.instance.player.GetComponent<Character>().repletion;
        if (repletion <= 50) {
          speed = 0.9f;
          strength = 0.9f;
        }
        else {
          speed = (50.0f - health) / 100.0f + 0.5f;
          strength = speed;
        }

        pulseMaterial.SetFloat("_time", timeDiff);
        pulseMaterial.SetFloat("_strength", strength);
        pulseMaterial.SetFloat("_speed", speed);
        Graphics.Blit(source, destination, pulseMaterial);
      }
      else {
        lowHealthTime = 0;
      }

      Graphics.Blit(source, destination, redRadiationMaterial);
    }
  }
  internal void ProcessPlayerSanity(RenderTexture source, RenderTexture destination) {
    float sanity = GameController.instance.player.GetComponent<Character>().sanity;
    if (sanity <= 80f) {
      if (sanity <= 5f)
        sanity = 5f;
      if (insanityTime == 0f)
        insanityTime = Time.fixedTime;
      float timeDiff = (Time.fixedTime - insanityTime) / 1440f * 2000f;
      if (GameController.instance.player.GetComponent<Character>().repletion >= 50f)
        timeDiff *= (80f - sanity) / 80f;
      strangeMaterial.SetFloat("_time", timeDiff);
      strangeMaterial.SetFloat("_strength", (80f - sanity) / 80f);
      Graphics.Blit(source, destination, strangeMaterial);
      //strangeMaterial.SetFloat("")
    }
    else {
      insanityTime = 0;
    }

  }
  internal void ProcessPlayerIntoxication(RenderTexture source, RenderTexture destination) {
    float intoxication = GameController.instance.player.GetComponent<Character>().intoxication;
    if (intoxication > 0.3f) {
      if (intoxication > 2.5f)
        intoxication = 2.5f;
      if (intoxicationTime == 0)
        intoxicationTime = Time.fixedTime;
      float intoxicationDelta = (Time.fixedTime - intoxicationTime) / 1440f * 500f;
      if (intoxicationDelta > 400f)
        intoxicationDelta -= 400f;
      if (intoxicationDelta > 0) {
        swirlMaterial.SetFloat("_time", intoxicationDelta * (1 + intoxication));
        swirlMaterial.SetFloat("_strength", intoxication + (float)System.Math.Pow(4, intoxication >= 0.7 ? intoxication - 0.7 : 0) - 1);
        Graphics.Blit(source, destination, swirlMaterial);
      }
    }
    else {
      intoxicationTime = 0;
    }
  }

  internal void ProcessSleep(RenderTexture source, RenderTexture destination) {
    if (paused) {
      if (fadeTime == 0) {
        fadeTime = Time.time;
      }
      float deltaTime;
      if (pauseReason == GameController.PauseReason.TRAIN || pauseReason == GameController.PauseReason.RUNAWAY) {
        deltaTime = (Time.time - fadeTime) / 1.0f;
      } else {
        deltaTime = (Time.time - fadeTime) /2.0f;
      }
      if (deltaTime > 0) {
        blackFadeMaterial.SetFloat("_factor", System.Math.Min(deltaTime, 1.0f));
        Graphics.Blit(source, destination, blackFadeMaterial);
      }
    }
    else {
      fadeTime = 0;
    }
  }

}
