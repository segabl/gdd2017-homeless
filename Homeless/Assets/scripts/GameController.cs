using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameController : PausableObject {

  public GameObject player;
  public GameObject menuCanvas;
  /*
  Menu Canvas contains panels which need to be independently activated/deactivated.
  It is somehow not possible to access a Panel from the canvas without using its name.
  Using GameObject.Find(<Game objects name>) is a bad idea, because if the GameObjects
  name changes in Unity this wouldn't be recognized in the code.
  To avoid this effect we store a reference to the Panels as a GameObject right here
  in the game handler. 
  TODO: think of a better solution (Possibly get inspired by other peoples experiences.)
  */
  public GameObject panelInGameMenu;
  public GameObject panelInventory;
  public float dayLength;

  private float accumulatedDelta;
  public int day { get; private set; }
  public float dayTime { get; private set; }

  private static GameController controllerInstance;

  public static GameController instance {
    get {
      if (!controllerInstance) {
        GameObject obj = new GameObject("GameController");
        return obj.AddComponent<GameController>();
      }
      return controllerInstance;
    }
  }

  void Awake() {
    DontDestroyOnLoad(gameObject);
    if (controllerInstance) {
      Debug.Log("Controller exists, only taking new editor params");
      controllerInstance.player = player;
      controllerInstance.dayLength = dayLength;
      controllerInstance.menuCanvas = menuCanvas;
      controllerInstance.panelInGameMenu = panelInGameMenu;
      controllerInstance.panelInventory = panelInventory;
      Destroy(gameObject);
    } else {
      Debug.Log("Controller created");
      controllerInstance = this;
    }
    day = 0;
    accumulatedDelta = 0;
  }

  protected override void updatePausable() {
    if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "GameScene") {
      return;
    }
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
    Debug.Log("Start loading from savefile");
    using (BinaryReader reader = new BinaryReader(File.Open("savefile.dat", FileMode.Open))) {
      player.transform.position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }
    Debug.Log("Finished loading");
  }

  public void pauseAll() {
    Object[] objects = FindObjectsOfType(typeof(PausableObject));
    foreach (PausableObject pausableObject in objects) {
      pausableObject.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
    }
  }

  public void unpauseAll() {
    Debug.Log("Unpause all PausableObjects");
    Object[] objects = FindObjectsOfType(typeof(PausableObject));
    foreach (PausableObject pausableObject in objects) {
      pausableObject.SendMessage("OnUnpauseGame", SendMessageOptions.DontRequireReceiver);
    }
  }

  public void deactivateChildsOfMenuPanels(GameObject panel) {
    foreach(GameObject itPanel in GameObject.FindGameObjectsWithTag("MenuPanel")) {
      if(itPanel != panel) {
        Debug.Log("MenuPanel " + itPanel.name + " deactivated");
        itPanel.SetActive(false);
      }
    }
  }
}
