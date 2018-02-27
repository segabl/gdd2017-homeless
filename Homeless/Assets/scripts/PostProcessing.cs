using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing : MonoBehaviour {

  public Texture daylightTexture;
  public Shader daylightCycleShader;
  public Shader blurShader;
  public Shader redRadiationShader;
  public Shader swirlShader;

  private Material daylightCycleMaterial;
  private Material blurMaterial;
  private Material redRadiationMaterial;
  private Material swirlMaterial;
  private float intoxicationTime;

  void Awake() {
    daylightCycleMaterial = new Material(daylightCycleShader);
    blurMaterial = new Material(blurShader);
    redRadiationMaterial = new Material(redRadiationShader);
    swirlMaterial = new Material(swirlShader);
    intoxicationTime = 0;
  }

  // Postprocess the image
  void OnRenderImage(RenderTexture source, RenderTexture destination) {


    //GL.Clear(false, true, Color.black);


    

    ProcessDaylight(source, source);

    ProcessPlayerHealth(source, source);
    ProcessPlayerSanity(source, source);
    ProcessPlayerIntoxication(source, source);

    Graphics.Blit(source, destination);

    


  }

  internal void ProcessDaylight(RenderTexture source, RenderTexture destination)
  {
    daylightCycleMaterial.SetTexture("_daylight", daylightTexture);
    daylightCycleMaterial.SetFloat("_time", GameController.instance.dayTime);
    Graphics.Blit(source, destination, daylightCycleMaterial);
  }
  internal void ProcessPlayerHealth(RenderTexture source, RenderTexture destination)
  {
    float health = GameController.instance.player.GetComponent<Character>().health;
    if (health <= 5f)
      health = 5f;
    if (health < 80.0f)
    {
      redRadiationMaterial.SetFloat("_delta", (80 - health) / 160f);
      if (health < 50.0f)
      {
        blurMaterial.SetFloat("hstep", 1.3f / health);
        blurMaterial.SetFloat("vstep", 1.0f / health);
        Graphics.Blit(source, source, blurMaterial);
      }
      
      Graphics.Blit(source, destination, redRadiationMaterial);
    }
  }
  internal void ProcessPlayerSanity(RenderTexture source, RenderTexture destination)
  {
    float sanity = GameController.instance.player.GetComponent<Character>().sanity;
    if (sanity <= 3f)
      sanity = 3f;
    if (sanity < 80.0f)
    {
      blurMaterial.SetFloat("hstep", 3.20f / sanity);
      blurMaterial.SetFloat("vstep", 1.80f / sanity);
      Graphics.Blit(source, destination, blurMaterial);
    }
  }
  internal void ProcessPlayerIntoxication(RenderTexture source, RenderTexture destination)
  {
    float intoxication = GameController.instance.player.GetComponent<Character>().intoxication;
    if (intoxication > 0.2f)
    {
      if (intoxication > 2.5f)
        intoxication = 2.5f;
      if (intoxicationTime == 0)
        intoxicationTime = GameController.instance.dayTime;
      float intoxicationDelta = (GameController.instance.dayTime - intoxicationTime) * 500f;
      if (intoxicationDelta > 400f)
        intoxicationDelta -= 400f;
      if (intoxicationDelta > 0)
      {
        swirlMaterial.SetFloat("_time", intoxicationDelta * (1 + intoxication));
        swirlMaterial.SetFloat("_strength", intoxication + (float)System.Math.Pow(4, intoxication >= 0.7 ? intoxication - 0.7 : 0) - 1);
        Graphics.Blit(source, destination, swirlMaterial);
      }
    }
    else
    {
      intoxicationTime = 0;
    }
  }

}
