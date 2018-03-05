using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PausableObject : MonoBehaviour {

  public bool paused { get; protected set; }
  public bool ignoreOnPause { get; protected set; }

  void Update() {
    if (paused) {
      return;
    }
    updatePausable();
  }

  public virtual void OnPauseGame() {
    paused = true;
  }
  public virtual void OnUnpauseGame() {
    paused = false;
  }
  public void ignoreNextOnPause()
  {
    ignoreOnPause = true;
  }

  /// <summary>
  /// Implement this for Pausable Objects to imitate Update behaviour of MonoBehaviour.
  /// Update Pausable is called by PausableObject if the PausableObject is not paused
  /// </summary>
  protected abstract void updatePausable();

}
