using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

  private static GameController controllerInstance;

  public float dayLength;

  private float lastDayTime;

  private int day = 0;
  private float dayTime = 0;

  public float Daytime {
    get {
      return dayTime;
    }
  }

  public static GameController Instance {
    get {
      return controllerInstance;
    }
  }

  // Use this for initialization
  void Start () {
    controllerInstance = this;
    lastDayTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
    dayTime = (Time.time - lastDayTime) / dayLength;
    if (dayTime > 1) {
      dayTime = 0;
      day++;
      lastDayTime = Time.time;
      Debug.Log("Day " + day + " started");
    }
	}
  
}
