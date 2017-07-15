using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeaOfGreed;

public class NPCPassiveController : MonoBehaviour {
	[SerializeField] private GameObject inkHolder;
	[SerializeField] private TextAsset inkJSONAsset;
	[SerializeField] private string knot;
	private BasicInkExample inkScriptRef;
	void Start(){
		inkScriptRef = inkHolder.GetComponent<BasicInkExample>();
	}
	public void OnInteract(){
		PlayerController.setMove(false);
		inkScriptRef.StartStory(inkJSONAsset.text);
	}
}
