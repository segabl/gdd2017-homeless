﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class TheftHandler : PausableObject {

  public float triggerDistance = 5;
  public float stealDistance = 3;
  public float detectionDistance = 1;
  public bool targetWasAlreadyRobbed = false;
  
  public static TheftHandler theftObject;
  public static bool playerCanSteal;
  public static bool playerIsStealing;
  public static bool playerWasCaught;

  protected AudioClip interactClip;

  private float radius;
  private int numSegments = 128;
  private float theftStart;
  private float theftDeltaTime;
  private float width;
  private float theftDuration = 3.0f;

  private List<Collectible> rewards;


  void Start() {
    playerWasCaught = false;
    //CreatePoints();
  }

  

  private void CreatePoints()
  {
    LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
    
    Color c1 = new Color(0.5f, 0.5f, 0.5f, 1);
    lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
    lineRenderer.startColor = c1;
    lineRenderer.endColor = c1;
    lineRenderer.startWidth = width;
    lineRenderer.endWidth = width;
    lineRenderer.positionCount = numSegments + 1;
    lineRenderer.useWorldSpace = false;
    
    float deltaTheta = (float)(2.0 * Mathf.PI) / numSegments;
    float theta = 0f;
    //radius = 1.5f;
    for (int i = 0; i < numSegments + 1; i++)
    {
      float x = radius * Mathf.Cos(theta);
      float y = radius * Mathf.Sin(theta);
      Vector3 pos = new Vector3(x, y, 0);
      lineRenderer.SetPosition(i, pos);
      theta += deltaTheta;
    }
    lineRenderer.enabled = true;
  }

  protected override void updatePausable() {


    if (targetWasAlreadyRobbed)
    {
      return;
    }

    if (Vector3.Distance(this.transform.position, GameController.instance.player.transform.position) < triggerDistance) {
     
      if (theftObject != this) {
      }
     
      theftObject = this;
      if (true)//!playerWasCaught)
      {
        playerCanSteal = true;
      }

      if (playerIsStealing)
      {
        drawTheftCircle();

        if (Vector3.Distance(this.transform.position, GameController.instance.player.transform.position) < stealDistance)
        {
          if (Vector3.Distance(this.transform.position, GameController.instance.player.transform.position) < detectionDistance)
          {
            playerCaught();
            endTheft();
          }

          updateTheftTimer();

          if (theftDeltaTime > 0.002)
          {
            theftSuccess();
          }
        }
        else
        {
          theftDeltaTime = 0;
          theftStart = 0;
        }
      }
      else
      {
        theftStart = 0;
        theftDeltaTime = 0;
        gameObject.GetComponent<LineRenderer>().enabled = false;
      }
    }
    else if (theftObject == this) {
      endTheft();
    }
  }

  protected void endTheft() {
    
    theftObject = null;
    playerCanSteal = false;
    playerIsStealing = false;
    theftStart = 0;
    theftDeltaTime = 0;
    gameObject.GetComponent<LineRenderer>().enabled = false;
  }
  /*public void pickPocket()
  {
    
  }
  */
  private void updateTheftTimer()
  {
    if (theftStart == 0)
    {
      theftStart = GameController.instance.dayTime;
    }
    theftDeltaTime = GameController.instance.dayTime - theftStart;
  }

  private void drawTheftCircle()
  {
    radius = Mathf.Max(stealDistance - theftDuration  * theftDeltaTime * 500f,0);
    width = Mathf.Max(stealDistance / 30f - theftDuration * theftDeltaTime * 18f, 0);
    
    CreatePoints();
  }
  private void playerCaught()
  {
    Debug.Log("Player was caught!");
    playerWasCaught = true;
  }
  private void theftSuccess()
  {
    Debug.Log("Player has successfully stolen!");
    targetWasAlreadyRobbed = true;
    reward();
    endTheft();
  }
  private void reward()
  {

  }

}