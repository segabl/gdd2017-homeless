using KarmaSystem;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameController : MonoBehaviour {

  public GameObject player;
  public GameObject menuCanvas;
  public GameObject panelInGameMenu;
  public GameObject panelInventory;
  public GameObject panelDead;
  public float dayLength;

  public bool paused { get; private set; }
  public int day { get; private set; }
  public float dayTime { get; private set; }
  public float inGameHour { get; private set; }
  private float hoursToWait;
  private float pauseUntil;
  private bool gamePaused;
  private PauseReason pauseReason;
  public bool trainInteraction { get; private set; }
  public enum PauseReason { SLEEPING, TRAIN, RUNAWAY, ARRESTED }

  public KarmaController karmaController = null;

  public BackgroundAudioLoop backgroundAudioLoop { get; private set; }
  public ModalPanel modalPanel { get; private set; }

  private static GameController controllerInstance;
  private bool trainHit;
  private bool arrested = false;

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
      controllerInstance.inGameHour = inGameHour;
      controllerInstance.hoursToWait = hoursToWait;
      controllerInstance.paused = paused;
      controllerInstance.menuCanvas = menuCanvas;
      controllerInstance.panelInGameMenu = panelInGameMenu;
      controllerInstance.panelInventory = panelInventory;
      controllerInstance.panelDead = panelDead;
      BackgroundAudioLoop loop = gameObject.GetComponent<BackgroundAudioLoop>();
      controllerInstance.backgroundAudioLoop.fadeToAudioClip(loop.audioClip, 1);
      controllerInstance.backgroundAudioLoop.loopStart = loop.loopStart;
      controllerInstance.backgroundAudioLoop.loopEnd = loop.loopEnd;
      ModalPanel panel = gameObject.GetComponent<ModalPanel>();
      controllerInstance.modalPanel.Question = panel.Question;
      controllerInstance.modalPanel.buttons = panel.buttons;
      controllerInstance.modalPanel.ModalPanelObject = panel.ModalPanelObject;
      Destroy(gameObject);
    }
    else {
      Debug.Log("Controller created");
      controllerInstance = this;
      backgroundAudioLoop = gameObject.GetComponent<BackgroundAudioLoop>();
      modalPanel = gameObject.GetComponent<ModalPanel>();
      //karmaController = new KarmaController();
      inGameHour = dayLength / 24.0f;
      hoursToWait = 6;
      paused = false;
    }
    day = 0;
    dayTime = 0.5f;
    if (player) {
      Vector3 targetPosition = player.transform.position;
      targetPosition.z = Camera.main.transform.position.z;
      Camera.main.transform.position = targetPosition;
    }
  }

  void Start() {
    if (backgroundAudioLoop) {
      backgroundAudioLoop.play();
    }
  }

  void Update() {
    if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "GameScene") {
      if (karmaController != null)
      {
        karmaController.initialized = false;
      }
      return;
    }
    if (karmaController == null || !karmaController.initialized)
      karmaController = new KarmaController();
    float deltaTime = Time.deltaTime;
    bool asleep = player.GetComponent<Character>().asleep;
    dayTime += deltaTime / dayLength;
    if (dayTime > 1) {
      dayTime = 0;
      day++;
      Debug.Log("Day " + day + " started");
    }
    if (gamePaused) {
      if (Time.time >= pauseUntil) {
        unpauseGame();
      }
    }
  }

  public void unpauseGame() {
    switch (pauseReason) {
      case PauseReason.SLEEPING: { 
          dayTime += 0.25f;
          player.GetComponent<Character>().asleep = false;
          player.GetComponent<CharacterAnimation>().playOnce("standup_sleep", "idle");
          break;
        }
      case PauseReason.TRAIN: {
          var clara = GameObject.Find("Clara");
          if (trainHit) {
            Destroy(clara);
          }
          else {
            clara.transform.position = new Vector3(clara.transform.position.x, -25.6f, clara.transform.position.z);
            clara.GetComponent<Dialogues>().SetTree("ClaraAfterTrain");
          }

          player.transform.position = new Vector3(player.transform.position.x, -25.6f, player.transform.position.z);
          player.GetComponent<MainCharacterMovement>().up = true;
          break;
        }
      case PauseReason.ARRESTED: {
          arrested = false;
          player.transform.position = new Vector3(0, 0);
          player.GetComponent<MainCharacterMovement>().stopMovement();
          break;
        }
      case PauseReason.RUNAWAY: {
          var clara = GameObject.Find("Clara");
          clara.transform.position = new Vector3(77.0f, -20.0f, clara.transform.position.z);
          player.GetComponent<MainCharacterMovement>().up = true;
          break;
        }
    }
    unpauseAll();
    gamePaused = false;
    Camera.main.GetComponent<PostProcessing>().paused = false;
  }

  public void pauseGameAndBlend(PauseReason reason, bool flag1 = false, GameObject other = null) {
    player.GetComponent<CharacterAnimation>().ignoreNextOnPause();
    pauseAll();
    pauseReason = reason;
    Character character = player.GetComponent<Character>();

    if (reason == PauseReason.SLEEPING) {
      character.asleep = true;
      //NOTE: Sleeping for 6h. Intox loss per hour = 0.1 -> -0.6 intox
      //                       Repletion loss per hour while asleep = 3.0 -> -18.0 repletion
      SleepingSpot spot = other.GetComponent<SleepingSpot>();
      character.adjustStats(-18.0f, spot.healthGain, spot.sanityGain, -0.6f);
      player.GetComponent<CharacterAnimation>().playOnce("liedown");
    } else if (reason == PauseReason.TRAIN) {
      character.adjustStats(0.0f, 0.0f, -20.0f, 0.0f);
      trainHit = flag1;
      AudioSource audioSource = gameObject.AddComponent<AudioSource>();
      AudioClip train = Resources.Load("sfx/train") as AudioClip;
      audioSource.clip = train;
      audioSource.Play();
    } else if (reason == PauseReason.RUNAWAY) {
      //Momentarilly no behaviour needed
    } else if (reason == PauseReason.ARRESTED) {
      character.adjustStats(-15.0f, 0.0f, -30.0f, 0.0f);
      other.GetComponent<PoliceBehavior>().stopChasing();
      pauseUntil = Time.time + 2;
    }


    pauseUntil = Time.time + 2;
    gamePaused = true;
    Camera.main.GetComponent<PostProcessing>().paused = true;
    Camera.main.GetComponent<PostProcessing>().pauseReason = reason;
    player.GetComponent<MainCharacterMovement>().stopMovement();
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
    SaveData data = (SaveData)bf.Deserialize(file);
    data.apply(this);
    file.Close();
    Debug.Log("Loaded game");
  }

  public void pauseAll() {
    paused = true;
    Object[] objects = FindObjectsOfType(typeof(PausableObject));
    List<string> alreadyPausedObjects = new List<string>();

    foreach (PausableObject pausableObject in objects) {
      if (!alreadyPausedObjects.Contains(pausableObject.name)) {
        pausableObject.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
        alreadyPausedObjects.Add(pausableObject.name);
      }
    }
  }

  public void unpauseAll() {
    if (!player.GetComponent<Character>().asleep) {
      paused = false;
      Debug.Log("Unpause all PausableObjects");
      Object[] objects = FindObjectsOfType(typeof(PausableObject));
      List<string> alreadyUnPausedObjects = new List<string>();

      foreach (PausableObject pausableObject in objects) {
        if (!alreadyUnPausedObjects.Contains(pausableObject.name)) {
          pausableObject.SendMessage("OnUnpauseGame", SendMessageOptions.DontRequireReceiver);
          alreadyUnPausedObjects.Add(pausableObject.name);
        }
      }
    }
  }

  public void deactivateChildsOfMenuPanels(GameObject panel) {
    foreach (GameObject itPanel in GameObject.FindGameObjectsWithTag("MenuPanel")) {
      if (itPanel != panel) {
        Debug.Log("MenuPanel " + itPanel.name + " deactivated");
        itPanel.SetActive(false);
      }
    }
  }
  public static GameObject createItemInstance(GameObject prefab)
  {
    GameObject item = Instantiate(prefab);
    item.name = item.name.Replace("(Clone)", "");

    item.GetComponent<Collectible>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
    return item;
    
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