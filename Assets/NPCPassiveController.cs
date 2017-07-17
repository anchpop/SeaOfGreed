using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeaOfGreed;

public class NPCPassiveController : MonoBehaviour {
	[SerializeField] private GameObject inkHolder;
	[SerializeField] private TextAsset inkJSONAsset;
	[SerializeField] private string knot;
	[SerializeField] private bool isRunningWorldScript;
	private BasicInkExample inkScriptRef;
	void Start(){
		inkScriptRef = inkHolder.GetComponent<BasicInkExample>();
		if(isRunningWorldScript){
			gameObject.GetComponent<BasicInkExample>().isCommandSlave = true;
			gameObject.GetComponent<BasicInkExample>().StartStory(inkJSONAsset.text, "WorldScript");
		}
	}
	public void OnInteract(){
		PlayerController.setMove(false);
		inkScriptRef.StartStory(inkJSONAsset.text);
	}
	void Update(){
	}
}
