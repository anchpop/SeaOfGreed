using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Ink.Runtime;
using SeaOfGreed;

public class BasicInkExample : MonoBehaviour {
	public static BasicInkExample inkHolder;
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
		inkHolder = this;
		textCompRef = textRef.GetComponent<Text>();
		queue = new ArrayList();
	}
	//it uses the text object's active value to determine whether or not it actually is running
	//if so, it runs one of two scripts - one which makes E go to the next page of text, the other of which renders all choices + an arrow with the currently selected one
	//selectedChoiceIndex is both the value to be given to the story and the current line with an arrow on it
	void Update(){
		if(getTextBoxActive()){
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
		setTextBoxActive(true);
		story = new Story (input);
		RefreshView();
	}
	public void StartStory (string input, string knot) {
		setTextBoxActive(true);
		story = new Story (input);
		if(knot!=null)story.ChoosePathString(knot);
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
	public void NextInQueue () {
		if(currentIndex < queue.Count)
      	{
			//if not command (ie first character isnt &) it just displays. Else - complicated shit
			//% = consecutive commands. tempArgs.args = the string after the "=", args[] = the command name and the string after "="
			//if the function doesn't plan on having a coroutine run NextInQueue after it runs, just run NextInQueue immediately
			if(((string)queue[currentIndex]).ToCharArray()[0] != '&' && ((string)queue[currentIndex]).ToCharArray()[0] != '$'){
				if(!getTextBoxActive())setTextBoxActive(true);
     			textCompRef.text = (string)queue[currentIndex];
        		currentIndex++;
        		isChoosing = false;
			} else{
				string temp = (string) queue[currentIndex];
				CommandArgs tempArgs;
				tempArgs.isSequential = (((string)queue[currentIndex]).ToCharArray()[0] == '$') ? true : false;
				if(tempArgs.isSequential && ((string)queue[currentIndex]).ToCharArray()[1] == '!'){ 
					temp = temp.Remove(0,1);
					setTextBoxActive(false);
				}
				temp = temp.Remove(0,1);
				string[] args = temp.Split('=');
				foreach(string qq in args){
					Debug.Log(qq + "Q");
				}
				tempArgs.args = args[1];
				currentIndex++;
				CommandController.runCommand(args[0], tempArgs);
				if(!tempArgs.isSequential) NextInQueue();
			}
        }
        else
        {
			if(story.currentChoices.Count > 0){
				if(!getTextBoxActive())setTextBoxActive(true);
				Debug.Log("isChoosing = true");
				isChoosing = true;
				selectedChoiceIndex = 0;
			} else{
				setTextBoxActive(false);
				PlayerController.setMove(true);
			}
		}
	}
	public void endOfCommand(CommandArgs arg){
		if(arg.isSequential) NextInQueue();
	}
	public void setTextBoxActive(bool b){
		textRef.transform.parent.gameObject.SetActive(b);
	}
	public bool getTextBoxActive(){
		return textRef.transform.parent.gameObject.activeInHierarchy;
	}
}