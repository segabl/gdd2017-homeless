using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PausableObject : MonoBehaviour {

  public bool paused { get; protected set; }
  protected float time = 0;

  void Start() {
    time = Time.time;
  }

  void Update() {
    if (paused) {
      return;
    }
    time += Time.deltaTime;
    updatePausable();
  }

  public virtual void OnPauseGame() {
    paused = true;
  }
  public virtual void OnUnpauseGame() {
    paused = false;
  }

  /// <summary>
  /// Implement this for Pausable Objects to imitate Update behaviour of MonoBehaviour.
  /// Update Pausable is called by PausableObject if the PausableObject is not paused
  /// </summary>
  protected abstract void updatePausable();

}
