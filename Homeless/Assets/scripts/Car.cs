using SpriterDotNetUnity;
using UnityEngine;

public class Car : PausableAnimatedObject {

  public Color color;
  public bool randomColor;
  public float speed;
  private float oldSpeed;
  private bool emergencyStop;
  private AudioSource audioSource;
  private GameObject stopForObject;
  public bool spawned;

  void Start() {
    Vector3 scale = gameObject.transform.localScale;
    scale.x = speed > 0 ? -1 : 1;
    gameObject.transform.localScale = scale;
    if (randomColor) {
      color = Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 0.8f), Random.Range(0.5f, 1.0f));
    }
    foreach (SpriteRenderer renderer in gameObject.GetComponentsInChildren<SpriteRenderer>()) {
      if (renderer.sprite.name.Equals("color")) {
        renderer.color = color;
      }
    }
    oldSpeed = speed;
  }

  protected override void updatePausable() {
    if (spriterAnimator == null) {
      spriterAnimator = gameObject.GetComponentInChildren<SpriterDotNetBehaviour>().Animator;
      spriterAnimator.AnimationFinished += onAnimationFinished;
      spriterAnimator.Play("drive");
    }
    if (audioSource == null) {
      audioSource = gameObject.GetComponent<AudioSource>();
    }
    float deltaTime = Time.deltaTime;

    if (Mathf.Abs(speed) > 0 || Mathf.Abs(oldSpeed) > 0) {
      if (stopForObject) {
        if (Mathf.Abs(speed) >= deltaTime * 20.0f) {
          speed -= Mathf.Sign(speed) * deltaTime * 20.0f;
        }
        else {
          speed = 0.0f;
          if (Vector3.Distance(gameObject.transform.position, stopForObject.transform.position) > 7) {
            startMoving();
          }
        }
      }
      else {
        if (Mathf.Abs(speed) <= Mathf.Abs(oldSpeed) - deltaTime * 5.0f) {
          speed += Mathf.Sign(oldSpeed) * deltaTime * 5.0f;
        }
        else {
          speed = oldSpeed;
        }
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position + new Vector3(Mathf.Sign(speed) * 4, 0, 0), new Vector3(Mathf.Sign(speed), 0, 0), 2);
        if (hit.collider) {
          stopMoving(hit.collider.gameObject);
        }
      }
      gameObject.transform.position += new Vector3(speed * deltaTime, 0, 0);
    }

    if (!emergencyStop) {
      spriterAnimator.Speed = Mathf.Abs(speed) / 20;
    }

    if (Mathf.Abs(speed) > 0) {
      if (!audioSource.isPlaying) {
        audioSource.Play();
      }
      audioSource.pitch = 0.75f + Mathf.Abs(speed) * 0.05f;
      audioSource.volume = Mathf.Abs(speed) * 0.1f;
    }
    else if (audioSource.isPlaying) {
      audioSource.Stop();
    }
    if (spawned && Vector3.Distance(gameObject.transform.position, GameController.instance.player.transform.position) > 400) {
      Destroy(gameObject);
    }
  }

  public void setSpeed(float s) {
    speed = s;
    oldSpeed = s;
  }

  private void onAnimationFinished(string animation) {
    if (animation == "break") {
      if (spriterAnimator.Speed > 0) {
        spriterAnimator.Speed = -spriterAnimator.Speed * 0.5f;
        spriterAnimator.Play("break");
      } else {
        spriterAnimator.Speed = 0;
      }
    }
  }

  public void stopMoving(GameObject stopFor, bool emergency = false) {
    if (!stopForObject) {
      emergencyStop = emergency;
      stopForObject = stopFor;
      if (emergency) {
        spriterAnimator.Speed = 0.25f;
        spriterAnimator.Play("break");
      }
    }
  }

  public void startMoving() {
    emergencyStop = false;
    stopForObject = null;
    spriterAnimator.Play("drive");
  }
}
