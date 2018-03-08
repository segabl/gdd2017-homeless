using UnityEngine;

public class Spawner : MonoBehaviour {

  public GameObject civilianPrefab;
  public float spawnRate = 2f;
  float nextSpawn = 0f;

  // Use this for initialization
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    if (Time.time > nextSpawn) {

      Instantiate(civilianPrefab, transform.position, Quaternion.identity);
      nextSpawn = Time.time + spawnRate;
    }

  }
}
