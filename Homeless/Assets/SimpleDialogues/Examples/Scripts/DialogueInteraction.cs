using UnityEngine;
using UnityEngine.UI;

public class DialogueInteraction : MonoBehaviour {

    [SerializeField]
    Dialogues npc;

    [SerializeField]
    Text dialogueText;
    [SerializeField]
    Text leftText;
    [SerializeField]
    Text rightText;
    [SerializeField]
    Text middleText;
    [SerializeField]
    GameObject backPanel;
    [SerializeField]
    GameObject nextTreeButton;

    bool nextEnd = false;

    // Simple Dialogues //
    // This is a basic example of how you can use the dialogue system. //

    
	void Start() {
        npc.SetTree("FirstMeeting"); //This sets the current tree to be used. Resets to the first node when called.
        Display();
	}

    public void Choice(int index)
    {
        if (index == 2 && npc.GetCurrentTree() == "TalkAgain") index = 1;
        if (npc.GetChoices().Length != 0)
        {
            npc.NextChoice(npc.GetChoices()[index]); //We make a choice out of the available choices based on the passed index.
            Display();                               //We actually call this function on the left and right button's onclick functions
        }
        else
        {
            Progress();
        }
    }

    public void TalkAgain()
    {
        npc.SetTree("TalkAgain");
        nextEnd = false;
        Display();
    }

    public void Progress()
    {
        npc.Next(); //This function returns the number of choices it has, in my case I'm checking that in the Display() function though.
        Display();
    }

    public void Display()
    {
        if (nextEnd == true)
        {
            backPanel.SetActive(false);
            nextTreeButton.SetActive(true);
        }
        else
        {
            backPanel.SetActive(true);
            nextTreeButton.SetActive(false);
        }

        //Sets our text to the current text
        dialogueText.text = npc.GetCurrentDialogue();
        //Just debug log our triggers for example purposes
        if (npc.HasTrigger())
            Debug.Log("Triggered: "+npc.GetTrigger());
        //This checks if there are any choices to be made
        if (npc.GetChoices().Length != 0)
        {
            //Setting the text's of the buttons to the choices text, in my case I know I'll always have a max of three choices for this example.
            leftText.text = npc.GetChoices()[0];
            middleText.text = npc.GetChoices()[1];
            //If we only have two choices, adjust accordingly
            if (npc.GetChoices().Length > 2)
                rightText.text = npc.GetChoices()[2];
            else
                rightText.text = npc.GetChoices()[1];
            //Setting the appropriate buttons visability
            leftText.transform.parent.gameObject.SetActive(true);
            rightText.transform.parent.gameObject.SetActive(true);
            if(npc.GetChoices().Length > 2)
                middleText.transform.parent.gameObject.SetActive(true);
            else
                middleText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            middleText.text = "Continue";
            //Setting the appropriate buttons visability
            leftText.transform.parent.gameObject.SetActive(false);
            rightText.transform.parent.gameObject.SetActive(false);
            middleText.transform.parent.gameObject.SetActive(true);
        }
        
        if (npc.End()) //If this is the last dialogue, set it so the next time we hit "Continue" it will hide the panel
            nextEnd = true;
    }
}
