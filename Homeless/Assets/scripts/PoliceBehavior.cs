using UnityEngine;
using System.Collections;
using System;

public class PoliceBehavior : NPCMovement
{
  
  protected float catchRange = 0.5f;
  public GameObject target;
  
  protected bool chasing = false;
  protected string reason = "stealing";
  protected float chasingSpeed;

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
  }
  protected override void updatePausable()
  {
    if (!chasing)
    {
      base.updatePausable();
      return;
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
    Debug.Log(Vector3.Distance(targetPosition, transform.position));
    if (Vector3.Distance(targetPosition, transform.position) > 12)
    {
      movementSpeed = 3f;
      chasing = false;
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
    target.GetComponent<Character>().arrest("Stealing");
  }

  public void startChasing(GameObject target_, string reason_, float speed)
  {
    chasing = true;
    target = target_;
    reason = reason_;
    chasingSpeed = speed;    
  }
}