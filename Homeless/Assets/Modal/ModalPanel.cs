using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ModalPanel : MonoBehaviour
  {
	public Text   Question;
	public Button Button1;   
	public Button Button2;   
	public Button Button3;   
  public Button Button4;   

  public GameObject ModalPanelObject;
  private static ModalPanel MainModalPanel;
  private List<Button> buttons;
  private GameController controller;

  public static ModalPanel Instance()
	{
		if (!MainModalPanel) {
			MainModalPanel = FindObjectOfType(typeof(ModalPanel)) as ModalPanel;
			if (!MainModalPanel) {
			  Debug.LogError("There needs to be one active ModalPanel script on a GameObject in your scene.");
			} else {
        MainModalPanel.buttons = new List<Button> { MainModalPanel.Button1, MainModalPanel.Button2, MainModalPanel.Button3, MainModalPanel.Button4 };
        MainModalPanel.controller = GameController.instance;
      }
		}
		return MainModalPanel;
	}

  public void MessageBox(string Question, Action<string> Callback, string[] Options) {
    controller.pauseAll();
    ModalPanelObject.SetActive(true);
    this.Question.text = Question;

    if (Options.Length > buttons.Count) {
      Debug.LogWarning("Only " + buttons.Count + " Choices are supported as of now. Please add more buttons to the modal Dialog");
    }

    for (int index = 0; index < Options.Length && index < buttons.Count; index++) {
      String option = Options[index];
      buttons[index].gameObject.SetActive(true);
      buttons[index].onClick.AddListener(() => {
        Debug.Log("Selected Choice " + index.ToString());
        Callback(option);
        ClosePanel();
      });
      buttons[index].GetComponentInChildren<Text>().text = Options[index];
    }
  }

	void ClosePanel()
	{
		ModalPanelObject.SetActive(false); //Close the Modal Dialog
    foreach (Button button in buttons) {
      button.onClick.RemoveAllListeners();
      button.gameObject.SetActive(false);
    }
    controller.unpauseAll();
	}
}
