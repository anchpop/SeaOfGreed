using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Ink.Runtime;
using SeaOfGreed;

public class BasicInkExample : MonoBehaviour {
	[SerializeField]
	private Story story;
	[SerializeField] private GameObject textRef;
	[SerializeField]private Text textCompRef;
	private ArrayList queue;
	private int currentIndex;
	private bool isChoosing;
	private int selectedChoiceIndex;
	[SerializeField] private Canvas canvas;
	//pretty obvious - initialization of stuff
	void Awake () {
		textCompRef = textRef.GetComponent<Text>();
		queue = new ArrayList();
	}
	//it uses the text object's active value to determine whether or not it actually is running
	//if so, it runs one of two scripts - one which makes E go to the next page of text, the other of which renders all choices + an arrow with the currently selected one
	//selectedChoiceIndex is both the value to be given to the story and the current line with an arrow on it
	void Update(){
		if(textRef.activeInHierarchy){
		if(!isChoosing){
		if(Input.GetKeyDown(KeyCode.E))
			NextInQueue();
		} else {
			string tempText = "";
			for(int i = 0; i < story.currentChoices.Count; i++){
				if(selectedChoiceIndex == i)
					tempText += "▶";
				tempText += story.currentChoices[i].text;
				tempText += "\n"; 
			}
			textCompRef.text = tempText;
			if(Input.GetKeyDown(KeyCode.UpArrow) && selectedChoiceIndex != 0)
				selectedChoiceIndex--;
			if(Input.GetKeyDown(KeyCode.DownArrow) && selectedChoiceIndex < story.currentChoices.Count - 1)
				selectedChoiceIndex++;
			Debug.Log("selectedchoiceindex = " + selectedChoiceIndex);
			if(Input.GetKeyDown(KeyCode.E)){
				story.ChooseChoiceIndex(selectedChoiceIndex);
				RefreshView();
			}
		}
		}
	}
	public void StartStory (string input) {
		textRef.SetActive(true);
		story = new Story (input);
		RefreshView();
	}
	//clears the queue of items, replaces it with the dialogue from story.Continue, which has changed because we gave it a choice(above)
	void RefreshView () {
		queue.Clear();
		currentIndex = 0;
		while (story.canContinue) {
			queue.Add(story.Continue ().Trim());
		}
		NextInQueue();
	}
	//shows the next line in the queue; if the queue is empty, it displays choices or closes the dialogue box
	void NextInQueue () {
		if(currentIndex < queue.Count)
      	{
     		textCompRef.text = (string)queue[currentIndex];
        	currentIndex++;
        	isChoosing = false;
        }
        else
        {
			if(story.currentChoices.Count > 0){
				Debug.Log("isChoosing = true");
				isChoosing = true;
				selectedChoiceIndex = 0;
			} else{
				textRef.SetActive(false);
				PlayerController.setMove(true);
				//TODO: put shit here to do when the dialogue is complete
			}
		}

	}
}