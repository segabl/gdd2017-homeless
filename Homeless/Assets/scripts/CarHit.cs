using UnityEngine;

public class CarHit : MonoBehaviour {

  public bool hit { get; private set; }
  public float immobilityTime;
  private float immobilityCount;
  private float hitDirection;
  private float hitSpeed;

  void Start() {
    hit = false;
    immobilityCount = immobilityTime;
  }

  void Update() {
    if (!hit) {
      return;
    }
    if (immobilityCount <= 0.0f) {
      immobilityCount = immobilityTime;
      hit = false;
      if (GetComponent<Character>() && GetComponent<Character>().alive) {
        GetComponent<CharacterAnimation>().playOnce("standup_ground", "idle");
      }
      return;
    }
    // 0.5 of immobilityTime is gliding, 0.5 is lying 
    if (immobilityCount >= 0.5f * immobilityTime) {
      float step = hitSpeed * Time.deltaTime * ((immobilityCount - 0.5f * immobilityTime) / (0.5f * immobilityTime));
      Vector3 directionVector = new Vector3();
      directionVector.x = Mathf.Sin(hitDirection);
      directionVector.y = Mathf.Cos(hitDirection);
      this.transform.position += directionVector * step;
    }
    immobilityCount -= Time.deltaTime;
  }

  void OnTriggerEnter2D(Collider2D col) {
    Car car = col.gameObject.GetComponent<Car>();
    if (car != null && Mathf.Abs(car.speed) > 0.1f && !hit) {
      Debug.Log("Collision with: " + col.gameObject.name);
      hitSpeed = Mathf.Abs(car.speed) * 0.75f;
      car.stopMoving(gameObject, true);
      if (name.Equals("MainCharacter")) {
        GetComponent<Character>().adjustStats(0.0f, -hitSpeed * 5.0f, 0.0f, 0.0f);
      }
      hitDirection = Mathf.Atan2(this.transform.position.x - car.transform.position.x, this.transform.position.y - car.transform.position.y);
      if (hitDirection > 0 && hitDirection <= Mathf.PI) {
        this.transform.localScale = new Vector3(this.transform.localScale.x * -1.0f, this.transform.localScale.y, this.transform.localScale.z);
      }
      GetComponent<CharacterAnimation>().playOnce("carhit");
      hit = true;
    }
  }
}
