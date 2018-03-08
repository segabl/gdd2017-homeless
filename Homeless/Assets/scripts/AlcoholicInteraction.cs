using UnityEngine;

namespace Assets.scripts {
  public class AlcoholicInteraction : CharacterInteraction {
    public float promile;
    public float intoxication;
    public AudioClip useSound;
    private bool storyTold = false;
    public void drink() {

      AudioSource audioSource = GameController.instance.player.GetComponent<Character>().gameObject.GetComponent<AudioSource>();
      GameController.instance.player.GetComponent<Character>().adjustStats(0, 0, 0, intoxication);
      if (audioSource) {
        audioSource.clip = useSound;
        audioSource.Play();
      }
      promile += intoxication;
      if (promile > 1 && !storyTold) {
        SetNextTree("PersonalStory");
        storyTold = true;
      }
      else if (promile > 1.5) {
        SetNextTree("Dying");
      }
      else {
        SetNextTree("OneMore");
      }

    }
    public void die() {
      GameController.instance.player.GetComponent<Character>().adjustStats(0, 0, -90, 0);
      SetNextTree("Dead");
    }


  }
}


