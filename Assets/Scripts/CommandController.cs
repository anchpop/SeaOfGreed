using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandController : MonoBehaviour {
	private static Dictionary<string, Action<string>> functionEmbedDict = new Dictionary<string, Action<string>>();
	void Start () {
		functionEmbedDict.Add("test", testMethod);
	}
	
	static void testMethod(string s){
		Debug.Log(s);
	}

	public static void runCommand(String commandName, string argument){
		functionEmbedDict[commandName](argument);
		BasicInkExample.inkHolder.NextInQueue();
	}
}
