using UnityEngine;

public class NPCMovement : PausableObject {

  public bool isWalking;
  public bool isWaiting;
  private System.Func<float> waitTime;
  private System.Func<float> walkDirectionChangeTime;
  private System.Func<float> randVel;
  private float walkCounter;
  private float waitCounter;
  private int walkDirection;
  private Vector3 targetPosition;
  public float movementSpeed;
  private Vector3 direction_vector;

  void Start() {
    waitTime = () => Random.Range(1.0f, 3.0f);
    walkDirectionChangeTime = () => Random.Range(3.0f, 7.0f);
    randVel = () => Random.Range(0.0f, 2.0f * Mathf.PI);
    direction_vector = Vector3.zero;
    waitCounter = waitTime();
    walkCounter = walkDirectionChangeTime();
  }

  protected override void updatePausable() {
    if (movementSpeed == 0) {
      return;
    }
    float step = movementSpeed * Time.deltaTime;
    if (GetComponent<CarHit>().hit) {
      waitCounter = waitTime();
      walkCounter = walkDirectionChangeTime();
      isWalking = false;
      targetPosition = this.transform.position;
      return;
    }
    if (walkCounter < 0) {
      waitCounter = waitTime();
      walkCounter = walkDirectionChangeTime();
      isWalking = false;
    }
    if (waitCounter < 0) {
      waitCounter = waitTime();
      walkCounter = walkDirectionChangeTime();
      float rand = randVel();
      direction_vector.x = -Mathf.Sin(rand);
      direction_vector.y = -Mathf.Cos(rand);
      isWalking = true;
    }

    if (isWalking) {
      RaycastHit2D hit_default = Physics2D.Raycast(this.transform.position, direction_vector, step, 1 << 0);
      RaycastHit2D hit_street = Physics2D.Raycast(this.transform.position, direction_vector, step, 1 << 9);
      isWalking = hit_default.collider == null && hit_street.collider == null;
    }

    if (!isWalking) {
      waitCounter -= Time.deltaTime;
      this.GetComponent<CharacterAnimation>().setAnimation = "idle";
      return;
    } else {
      walkCounter -= Time.deltaTime;
      this.transform.position += direction_vector * step;
    }
    

    float walkingDirection = Mathf.Atan2(direction_vector.x, direction_vector.y);
    float corr = 0.01f;
    if (walkingDirection >= Mathf.PI * 0.25f + corr && walkingDirection < Mathf.PI * 0.75f) {
      //RIGHT
      if (this.transform.localScale.x < 0.0f) {
        this.transform.localScale = new Vector3(this.transform.localScale.x * -1.0f, this.transform.localScale.y, this.transform.localScale.y);
      }
      this.GetComponent<CharacterAnimation>().setAnimation = "walking_side";
    }
    else if (walkingDirection < Mathf.PI * 0.25f && walkingDirection > Mathf.PI * -0.25f) {
      //UP
      if (this.transform.localScale.x < 0.0f) {
        this.transform.localScale = new Vector3(this.transform.localScale.x * -1.0f, this.transform.localScale.y, this.transform.localScale.y);
      }
      this.GetComponent<CharacterAnimation>().setAnimation = "walking_back";
    }
    else if (walkingDirection <= Mathf.PI * -0.25f - corr && walkingDirection > Mathf.PI * -0.75) {
      //LEFT
      if (this.transform.localScale.x > 0.0f) {
        this.transform.localScale = new Vector3(this.transform.localScale.x * -1.0f, this.transform.localScale.y, this.transform.localScale.y);
      }
      this.GetComponent<CharacterAnimation>().setAnimation = "walking_side";
    }
    else if (walkingDirection >= Mathf.PI * 0.75f + corr || walkingDirection <= Mathf.PI * -0.75 - corr) {
      //DOWN
      if (this.transform.localScale.x < 0.0f) {
        this.transform.localScale = new Vector3(this.transform.localScale.x * -1.0f, this.transform.localScale.y, this.transform.localScale.y);
      }
      this.GetComponent<CharacterAnimation>().setAnimation = "walking_front";
    }
  }

}

