using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PausableObject : MonoBehaviour {

  public bool paused { get; private set; }

  void Update() {
    if (paused) {
      return;
    }
    updatePausable();
  }

  public void OnPauseGame() {
    Debug.Log("Pausing object: " + this.name);
    paused = true;
  }
  public void OnUnpauseGame() {
    Debug.Log("Unpausing object: " + this.name);
    paused = false;
  }

  /// <summary>
  /// Implement this for Pausable Objects to imitate Update behaviour of MonoBehaviour.
  /// Update Pausable is called by PausableObject if the PausableObject is not paused
  /// </summary>
  protected abstract void updatePausable();

}
