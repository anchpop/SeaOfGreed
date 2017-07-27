using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeaOfGreed;

public class NPCPassiveController : MonoBehaviour {
	[SerializeField] private GameObject inkHolder;
	[SerializeField] private TextAsset inkJSONAsset;
	[SerializeField] private string knot;
	[SerializeField] private bool isRunningWorldScript;
	private InkProcessor inkScriptRef;
	void Start(){
		inkScriptRef = inkHolder.GetComponent<InkProcessor>();
		if(isRunningWorldScript){
			gameObject.GetComponent<InkProcessor>().isCommandSlave = true;
			gameObject.GetComponent<InkProcessor>().StartStory(inkJSONAsset.text, "WorldScript");
		}
	}
	public void OnInteract(){
		PlayerController.setMove(false);
		inkScriptRef.StartStory(inkJSONAsset.text);
		inkScriptRef.setEndAction(this.onInteractEnd);
		if(isRunningWorldScript)
			gameObject.GetComponent<InkProcessor>().pauseCoroutines = true;
	}
	public void onInteractEnd(){
		if(isRunningWorldScript)
			gameObject.GetComponent<InkProcessor>().pauseCoroutines = false;
	}
	void Update(){
	}
}
