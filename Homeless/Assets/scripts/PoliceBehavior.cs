using UnityEngine;
using System.Collections;
using System;

public class PoliceBehavior : NPCMovement
{
  
  protected float catchRange = 0.5f;
  public GameObject target;
  
  protected bool chasing = false;
  protected bool aiming = false;
  public bool shooting = false;
  protected string reason = "stealing";
  protected float chasingSpeed;
  protected AudioClip shootingClip;
  public static bool audioTriggered = false;
  private float shootIn;

  void Start()
  {
    Init();
  }

  protected override void Init()
  {
    base.Init();
    waitTime = () => UnityEngine.Random.Range(4.0f, 10.0f);
    movementSpeed = 3f;
    if (target == null)
    {
      target = GameController.instance.player;
    }
    shootingClip = (AudioClip)Resources.Load("sfx/gun-shot");
  }
  protected override void updatePausable()
  {
    if (!chasing && !shooting && !aiming)
    {
      base.updatePausable();
      return;
    }
    if (aiming) {
      Vector3 relativeTargetPosition = Camera.main.WorldToScreenPoint(target.transform.position);
      Vector3 relativeThisPosition = Camera.main.WorldToScreenPoint(transform.position);
      Vector3 direction = relativeTargetPosition - relativeThisPosition;
      float shootDirection = Mathf.Sign(direction.x);
      if (shootDirection >= 0f) {
        transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.y);
      }
      else {
        transform.localScale = new Vector3(-Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.y);
      }
      if (time > shootIn && !shooting) {
        shooting = true;
        Debug.Log("Shooting the player");
        gameObject.GetComponent<CharacterAnimation>().playOnce("shoot");

        AudioSource audioSource = GameController.instance.player.gameObject.GetComponent<AudioSource>();
        if (audioSource) {
          audioSource.clip = shootingClip;
          audioSource.Play();
        }
        //target.GetComponent<Character>().shoot("stealing");
        target.GetComponent<Character>().die("Police shooting you");
      }
    }

    if (shooting || aiming)
    {
      return;
    }
    if (GameController.instance.karmaController.isCriminal(target))
    {
      catchRange = 4f;
    }
    if (targetInRange())
    {
      targetCaptured();
      return;
    }
    chase();

  }
  
  protected void chase()
  {
    movementSpeed = chasingSpeed;
    Vector3 targetPosition = target.transform.position;
    Vector3 direction = targetPosition - transform.position;
    if (Vector3.Distance(targetPosition, transform.position) > 12)
    {
      stopChasing();
      return;
    }


    transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

    float walkingDirection = Mathf.Atan2(direction.x, direction.y);
    float corr = 0.01f;

    if (walkingDirection >= Mathf.PI * 0.25f + corr && walkingDirection < Mathf.PI * 0.75f)
    {
      //RIGHT
      if (this.transform.localScale.x < 0.0f)
      {
        this.transform.localScale = new Vector3(this.transform.localScale.x * -1.0f, this.transform.localScale.y, this.transform.localScale.y);
      }
      this.GetComponent<CharacterAnimation>().currentAnimation = "walking_side";
    }
    else if (walkingDirection < Mathf.PI * 0.25f && walkingDirection > Mathf.PI * -0.25f)
    {
      //UP
      if (this.transform.localScale.x < 0.0f)
      {
        this.transform.localScale = new Vector3(this.transform.localScale.x * -1.0f, this.transform.localScale.y, this.transform.localScale.y);
      }
      this.GetComponent<CharacterAnimation>().currentAnimation = "walking_back";
    }
    else if (walkingDirection <= Mathf.PI * -0.25f - corr && walkingDirection > Mathf.PI * -0.75)
    {
      //LEFT
      if (this.transform.localScale.x > 0.0f)
      {
        this.transform.localScale = new Vector3(this.transform.localScale.x * -1.0f, this.transform.localScale.y, this.transform.localScale.y);
      }
      this.GetComponent<CharacterAnimation>().currentAnimation = "walking_side";
    }
    else if (walkingDirection >= Mathf.PI * 0.75f + corr || walkingDirection <= Mathf.PI * -0.75 - corr)
    {
      //DOWN
      if (this.transform.localScale.x < 0.0f)
      {
        this.transform.localScale = new Vector3(this.transform.localScale.x * -1.0f, this.transform.localScale.y, this.transform.localScale.y);
      }
      this.GetComponent<CharacterAnimation>().currentAnimation = "walking_front";
    }

    

  }
  protected bool targetInRange()
  {
    if (Vector3.Distance(transform.position, target.transform.position) <= catchRange)
    {
      return true;
    }
    return false;
  }

  protected void targetCaptured()
  {
    gameObject.GetComponent<CharacterAnimation>().playOnce("idle", "idle");
    if (GameController.instance.karmaController.isCriminal(target) && !aiming)
    {
      aiming = true;
      gameObject.GetComponent<CharacterAnimation>().playOnce("draw_gun");
      shootIn = time + UnityEngine.Random.Range(0.5f, 1f);
      stopChasing();
    }
      //target.GetComponent<Character>().arrest("Stealing");
    else
    {
      GameController.instance.karmaController.SocialAction(target, KarmaSystem.SocialConstants.gettingCaughtByPolice);
      GameController.instance.arrestPlayer(gameObject);
    }
  }

  public void startChasing(GameObject target_, string reason_, float speed)
  {
    chasing = true;
    target = target_;
    reason = reason_;
    chasingSpeed = speed;    
  }
  public void stopChasing()
  {
    movementSpeed = 3f;
    chasing = false;
  }

}