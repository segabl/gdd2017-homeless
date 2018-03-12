using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class TheftHandler : PausableObject {

  public float triggerDistance = 5;
  public float stealDistance = 3;
  [Range(0.9f, 1.3f)]
  public float detectionDistance = 0.9f;
  public bool targetWasAlreadyRobbed = false;
  public bool playerWasCaught = false;
  public GameObject reward;
  public GameObject policePrefab;

  public static TheftHandler theftObject;
  public static bool playerCanSteal;
  public static bool playerIsStealing;


  protected AudioClip interactClip;

  private float radius;
  private int numSegments = 128;
  private float theftStart;
  private float theftDeltaTime;
  private float width;
  private float theftDuration = 3.0f;
  private Text stealingText;
  private static float caughtTime;
  private bool secondTry = false;
  private bool displayedSecondTry = false;
  private bool pushAway = false;
  private float pushStart = 0f;
  private float pushDirection;




  void Start() {
    playerWasCaught = false;
    interactClip = (AudioClip)Resources.Load("sfx/grab-item");
    //CreatePoints();
  }



  private void CreatePoints() {
    LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();

    Color c1 = new Color(0.5f, 0.5f, 0.5f, 1);
    //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
    lineRenderer.startColor = c1;
    lineRenderer.endColor = c1;
    lineRenderer.startWidth = width;
    lineRenderer.endWidth = width;
    lineRenderer.positionCount = numSegments + 1;
    lineRenderer.useWorldSpace = false;

    float deltaTheta = (float)(2.0 * Mathf.PI) / numSegments;
    float theta = 0f;
    //radius = 1.5f;
    for (int i = 0; i < numSegments + 1; i++) {
      float x = radius * Mathf.Cos(theta);
      float y = radius * Mathf.Sin(theta);
      Vector3 pos = new Vector3(x, y, 0);
      lineRenderer.SetPosition(i, pos);
      theta += deltaTheta;
    }
    lineRenderer.enabled = true;
  }

  protected override void updatePausable() {

    if (!stealingText) {
      GameObject canvas = GameController.instance.menuCanvas;
      Text[] texts = canvas.GetComponentsInChildren<Text>();
      foreach (Text t in texts) {
        if (t.name.Equals("StealingText")) {
          stealingText = t;
          break;
        }
      }
      if (!stealingText) {
        Debug.Log("stealingText is null");
        return;
      }
    }
    if (playerWasCaught) {
      if (stealingText.enabled) {

        if (((GameController.instance.dayTime - caughtTime) > (1.5f) / GameController.instance.dayLength) && caughtTime != 0f) {

          caughtTime = 0;
          stealingText.enabled = false;
          secondTry = true;
        }
      }
      if (pushAway) {
        float delta = (GameController.instance.dayTime - pushStart);
        if (delta < 0.15f / GameController.instance.dayLength) {
          GameController.instance.player.transform.position += new Vector3(-Mathf.Sin(pushDirection), Mathf.Cos(pushDirection)) * (0.05f - delta);
        }
        else {
          pushAway = false;
          pushDirection = 0;
          pushStart = 0;
        }
      }
    }

    if (targetWasAlreadyRobbed) {
      return;
    }
    if (playerWasCaught && !secondTry) {
      return;
    }

    if (Vector3.Distance(this.transform.position, GameController.instance.player.transform.position) < triggerDistance) {

      if (theftObject != this && playerCanSteal) {
        return;
      }

      theftObject = this;
      if (secondTry) {
        if (Vector3.Distance(this.transform.position, GameController.instance.player.transform.position) < stealDistance) {
          stealingText.enabled = true;
          if (caughtTime == 0f)
            caughtTime = GameController.instance.dayTime;
          if (GameController.instance.dayTime - caughtTime < 1f / GameController.instance.dayLength) {
            displayedSecondTry = true;
            stealingText.enabled = true;
            stealingText.text = "Piss off, thief!";
          }
          else if (!displayedSecondTry) {
            stealingText.enabled = false;
            displayedSecondTry = true;
          }
        }
        return;
      }
      if (true)//!playerWasCaught)
      {
        playerCanSteal = true;
      }

      if (playerIsStealing) {
        drawTheftCircle();

        if (Vector3.Distance(this.transform.position, GameController.instance.player.transform.position) < stealDistance) {
          if (theftDeltaTime < 0.001f && Vector3.Distance(this.transform.position, GameController.instance.player.transform.position) < detectionDistance) {
            playerCaught();
            endTheft();
          }

          updateTheftTimer();

          if (theftDeltaTime > 0.00115f) {
            if (Vector3.Distance(this.transform.position, GameController.instance.player.transform.position) <= detectionDistance)
              theftSuccess();
            else {
              theftDeltaTime = 0;
              theftStart = 0;
            }
          }
        }
        else {
          theftDeltaTime = 0;
          theftStart = 0;
          //gameObject.GetComponent<LineRenderer>().enabled = false;
        }
      }
      else {
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
  private void updateTheftTimer() {
    if (theftStart == 0) {
      theftStart = Time.fixedTime;
    }
    theftDeltaTime = (Time.fixedTime - theftStart) / 1440f;
  }

  private void drawTheftCircle() {
    if (theftDeltaTime < 0.0005f)
      radius = Mathf.Max(stealDistance - theftDuration * theftDeltaTime * 500f, 0);
    else
      radius = Mathf.Max(stealDistance - theftDuration * theftDeltaTime * 500f - (theftDeltaTime - 0.0005f) * 2000, 0);
    width = Mathf.Max(stealDistance / 30f - theftDuration * theftDeltaTime * 18f, 0);
    CreatePoints();
  }
  private void playerCaught() {
    Debug.Log("Player was caught!");
    playerWasCaught = true;
    playerIsStealing = false;

    GameController.instance.karmaController.SocialAction(GameController.instance.player, KarmaSystem.SocialConstants.gettingCaughtStealing);
    Vector3 relativeTargetPosition = Camera.main.WorldToScreenPoint(GameController.instance.player.transform.position);
    Vector3 relativeThisPosition = Camera.main.WorldToScreenPoint(transform.position);
    Vector3 direction = relativeThisPosition - relativeTargetPosition;
    pushDirection = direction.x;

    pushStart = GameController.instance.dayTime;
    pushAway = true;
    transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.y);
    if ((direction.x < 0f)) {
      GetComponent<CharacterAnimation>().playOnce("push_away_right", "idle");
    }
    else {
      GetComponent<CharacterAnimation>().playOnce("push_away_left", "idle");

    }
    GameController.instance.player.GetComponent<CharacterAnimation>().playOnce("idle", "idle");

    FindObjectOfType<InputHandler>().disableMovementFor(0.5f);

    stealingText.text = "What the fuck?!";
    stealingText.enabled = true;
    stealingText.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    caughtTime = GameController.instance.dayTime;
    //GameController.instance.karmaController.DebugKarmaList();
    GameObject[] officers = GameObject.FindGameObjectsWithTag("Police");
    foreach (GameObject officer in officers) {
      if (Vector3.Distance(GameController.instance.player.transform.position, officer.transform.position) < 8f) {
        officer.GetComponent<PoliceBehavior>().startChasing(GameController.instance.player, "stealing", 4.5f);
      }
    }

    if (GameController.instance.karmaController.isCriminal(GameController.instance.player)) {
      GameObject police1 = Instantiate(policePrefab);
      GameObject police2 = Instantiate(policePrefab);

      police1.transform.position = GameController.instance.player.transform.position + new Vector3(-10f, 0f);
      police2.transform.position = GameController.instance.player.transform.position + new Vector3(10f, 0f);
      police1.GetComponent<PoliceBehavior>().startChasing(GameController.instance.player, "stealing", 6.5f);
      police2.GetComponent<PoliceBehavior>().startChasing(GameController.instance.player, "stealing", 6.5f);

    }
  }
  private void theftSuccess() {
    Debug.Log("Player has successfully stolen!");
    targetWasAlreadyRobbed = true;

    AudioSource audioSource = GameController.instance.player.gameObject.GetComponent<AudioSource>();
    GameController.instance.player.GetComponent<CharacterAnimation>().playOnce("give_front", "idle");
    if (audioSource) {
      audioSource.clip = interactClip;
      audioSource.Play();
    }

    giveReward();
    endTheft();
  }
  private void giveReward() {
    GameObject rewardInstance = Instantiate(reward);
    rewardInstance.name = rewardInstance.name.Replace("(Clone)", "");

    rewardInstance.GetComponent<Collectible>().sprite = reward.GetComponent<SpriteRenderer>().sprite;
    GameController.instance.player.GetComponent<Inventory>().addItem(rewardInstance.GetComponent<Collectible>());
    rewardInstance.SetActive(false);
  }

}
