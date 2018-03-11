using UnityEngine;

public class CarSpawner : PausableObject {

  public float minWaitTime = 10;
  public float maxWaitTime = 20;
  public float carSpeed = 10;

  public GameObject carPrefab;

  private float nextSpawnTime;

  void Start() {
    nextSpawnTime = time + Random.Range(minWaitTime, maxWaitTime) * 0.5f;
  }

  protected override void updatePausable() {
    if (time > nextSpawnTime) {
      RaycastHit2D hit = Physics2D.Raycast(this.transform.position, new Vector3(Mathf.Sign(carSpeed), 0, 0), 5, (1 << 0) | (1 << 8));
      if (!hit.collider) {
        GameObject car = Instantiate(carPrefab, transform.position, Quaternion.identity);
        car.GetComponent<Car>().setSpeed(carSpeed);
        car.GetComponent<Car>().spawned = true;
        nextSpawnTime = time + Random.Range(minWaitTime, maxWaitTime);
      }
    }
  }
}
