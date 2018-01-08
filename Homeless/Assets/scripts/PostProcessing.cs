using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing : MonoBehaviour {

  public Shader daylightCycleShader;

  private Material daylightCycleMaterial;

  void Awake() {
    daylightCycleMaterial = new Material(daylightCycleShader);
  }

  // Postprocess the image
  void OnRenderImage(RenderTexture source, RenderTexture destination) {
    daylightCycleMaterial.SetFloat("_time", GameController.instance.dayTime);
    Graphics.Blit(source, destination, daylightCycleMaterial);
  }
}
