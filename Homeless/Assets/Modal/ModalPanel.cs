using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ModalPanel : MonoBehaviour
  {
	public Text   Question;
  public List<Button> buttons;

  public GameObject ModalPanelObject;
  private static ModalPanel MainModalPanel;

  public void MessageBox(string Question, Action<string> Callback, string[] Options) {
    GameController.instance.pauseAll();
    ModalPanelObject.SetActive(true);
    this.Question.text = Question;

    if (Options.Length > 4) {
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
    GameController.instance.unpauseAll();
	}
}
