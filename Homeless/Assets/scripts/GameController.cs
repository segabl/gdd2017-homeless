using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameController {
  
  public GameObject player { get; set; }
  public float dayLength { get; set; }
  private float accumulatedDelta = 0;
  private int day = 0;
  public float dayTime { get; set; }

  private static GameController controllerInstance;
  public static GameController instance {
    get {
      if (controllerInstance == null) {
        controllerInstance = new GameController();
      }
      return controllerInstance;
    }
  }

	public void update() {
    accumulatedDelta += Time.deltaTime;
    dayTime = accumulatedDelta / dayLength;
    if (dayTime > 1) {
      dayTime = 0;
      day++;
      accumulatedDelta = 0;
      Debug.Log("Day " + day + " started");
    }
	}

  public void saveGame() {
    // TODO: use a serializer or something
    using (BinaryWriter writer = new BinaryWriter(File.Open("savefile.dat", FileMode.Create))) {
      writer.Write(player.transform.position.x);
      writer.Write(player.transform.position.y);
      writer.Write(player.transform.position.z);
    }
    Debug.Log("Progress saved");
  }

  public void loadGame() {
    using (BinaryReader reader = new BinaryReader(File.Open("savefile.dat", FileMode.Open))) {
      player.transform.position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }
  }

}
