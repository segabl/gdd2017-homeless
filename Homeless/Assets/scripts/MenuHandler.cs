using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour {

  public Button b_start;
  public Button b_continue;
  public Button b_end;

	// Use this for initialization
	void Start () {
    if (System.IO.File.Exists("savefile.txt")) {
      b_continue.interactable = true;
    }
  }
	
	// Update is called once per frame
	void Update () {

	}

  public void gameStart() {
    UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
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
}
