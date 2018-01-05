using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour {

  public Button continueButton;

  public BackgroundAudioLoop backgroundAudio;

	// Use this for initialization
	void Start () {
    if (continueButton && System.IO.File.Exists("savefile.txt")) {
      continueButton.interactable = true;
    }
  }
	
	// Update is called once per frame
	void Update () {

	}

  public void gameStart() {
    UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    if (backgroundAudio) {
      backgroundAudio.fadeToVolume(0, 2);
    }
    Debug.Log("Start game");
  }

  public void gameContinue() {
    Debug.Log("Continue game");
  }

  public void gameEnd() {
    Debug.Log("End game");
    #if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
    #endif
    Application.Quit();
  }

  public void gameMainMenu() {
    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    Debug.Log("Start game");
  }

  public void gameResume() {
    this.gameObject.SetActive(false);
    Debug.Log("Resume game");
  }
}
