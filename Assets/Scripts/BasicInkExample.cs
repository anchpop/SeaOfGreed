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
	void Awake () {
		textCompRef = textRef.GetComponent<Text>();
		queue = new ArrayList();
	}
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

	void RefreshView () {
		queue.Clear();
		currentIndex = 0;
		while (story.canContinue) {
			queue.Add(story.Continue ().Trim());
			Debug.Log("HEY!");
		}

		/*if(story.currentChoices.Count > 0) {
			for (int i = 0; i < story.currentChoices.Count; i++) {
				Choice choice = story.currentChoices [i];
				Button button = CreateChoiceView (choice.text.Trim ());
				button.onClick.AddListener (delegate {
					OnClickChoiceButton (choice);
				});
			}
		} else {
			Button choice = CreateChoiceView("End of story.\nRestart?");
			choice.onClick.AddListener(delegate{
				StartStory();
			});
		}*/
		NextInQueue();
	}

	void OnClickChoiceButton (Choice choice) {
		story.ChooseChoiceIndex (choice.index);
		RefreshView();
	}
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
				//put shit here to do when the dialogue is complete
			}
		}

	}
}