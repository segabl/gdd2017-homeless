using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using KarmaSystem;

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

  public KarmaController karmaController = null;

  public BackgroundAudioLoop backgroundAudioLoop { get; private set; }

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
      BackgroundAudioLoop loop = gameObject.GetComponent<BackgroundAudioLoop>();
      controllerInstance.backgroundAudioLoop.fadeToAudioClip(loop.audioClip, 1);
      controllerInstance.backgroundAudioLoop.loopStart = loop.loopStart;
      controllerInstance.backgroundAudioLoop.loopEnd = loop.loopEnd;
      Destroy(gameObject);
    } else {
      Debug.Log("Controller created");
      controllerInstance = this;
      backgroundAudioLoop = gameObject.GetComponent<BackgroundAudioLoop>();
      karmaController = new KarmaController();
      day = 0;
      accumulatedDelta = 0;
    }
  }

  private void Start() {
    if (backgroundAudioLoop) {
      backgroundAudioLoop.play();
    }
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
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Open("savefile.dat", FileMode.Create);
    bf.Serialize(file, new SaveData(this));
    file.Close();
    Debug.Log("Progress saved");
  }

  public void loadGame() {
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Open("savefile.dat", FileMode.Open);
    SaveData data = (SaveData) bf.Deserialize(file);
    data.apply(this);
    file.Close();
    Debug.Log("Loaded game");
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

  /* 
  Serializeable save data class that contains data to be saved
  */
  [System.Serializable]
  class SaveData {
    public int day;
    public float dayTime;
    public float playerX;
    public float playerY;
    public float playerZ;

    public SaveData(GameController controller) {
      day = controller.day;
      dayTime = controller.dayTime;
      playerX = controller.player.transform.position.x;
      playerY = controller.player.transform.position.y;
      playerZ = controller.player.transform.position.z;
    }

    public void apply(GameController controller) {
      controller.day = day;
      controller.dayTime = dayTime;
      controller.player.transform.position = new Vector3(playerX, playerY, playerZ);
    }
  }
}