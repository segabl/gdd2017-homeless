using SpriterDotNetUnity;

public abstract class PausableAnimatedObject : PausableObject {

  public bool ignoreOnPause { get; protected set; }
  protected UnityAnimator spriterAnimator;
  float oldAnimationSpeed = 0;

  public override void OnPauseGame() {
    if (ignoreOnPause)
      return;
    base.OnPauseGame();
    oldAnimationSpeed = spriterAnimator.Speed != 0 ? spriterAnimator.Speed : oldAnimationSpeed;
    spriterAnimator.Speed = 0;
  }
  public override void OnUnpauseGame() {
    if (ignoreOnPause) {
      ignoreOnPause = false;
      return;
    }
    base.OnUnpauseGame();
    spriterAnimator.Speed = oldAnimationSpeed;
  }
  public void ignoreNextOnPause() {
    ignoreOnPause = true;
  }

}
