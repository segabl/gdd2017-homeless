using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

  public Button continueButton;
  public Button exitButton;

	// Use this for initialization
	void Start () {
    if (continueButton && System.IO.File.Exists("savefile.dat")) {
      continueButton.interactable = true;
    }
    if (exitButton && Application.platform == RuntimePlatform.WebGLPlayer) {
      exitButton.interactable = false;
    }
  }
	
	// Update is called once per frame
	void Update () {

	}

  public void gameStart() {
    SceneManager.LoadScene("GameScene");
    Debug.Log("Start game");
  }

  void OnGameSceneLoaded(Scene scene , LoadSceneMode mode) { 
    GameController.instance.loadGame();
    SceneManager.sceneLoaded -= OnGameSceneLoaded;
  }

  public void gameContinue() {
    Debug.Log("Continue game");
    SceneManager.sceneLoaded += OnGameSceneLoaded;
    gameStart();
  }

  public void gameEnd() {
    Debug.Log("End game");
    #if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
    #endif
    Application.Quit();
  }

  public void gameMainMenu() {
    GameController.instance.unpauseAll();
    SceneManager.LoadScene("MainMenu");
    Debug.Log("Back to Main menu");
  }

  public void gameResume() {
    this.gameObject.SetActive(false);
    GameController.instance.unpauseAll();
    Debug.Log("Resume game");
  }

}
