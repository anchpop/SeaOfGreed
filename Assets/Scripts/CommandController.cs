using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandController : MonoBehaviour {
	private static Dictionary<string, Action<CommandArgs>> functionEmbedDict = new Dictionary<string, Action<CommandArgs>>();
	void Start () {
		functionEmbedDict.Add("test", testMethod);
		functionEmbedDict.Add("move", GameObject.Find("squarey").GetComponent<ObeyCommand>().move);
		functionEmbedDict.Add("wait", GameObject.Find("squarey").GetComponent<ObeyCommand>().wait);
	}
	static void testMethod(CommandArgs s){
		Debug.Log(s.args);
		s.commandCaller.endOfCommand(s);
	}

	public static void runCommand(String commandName, CommandArgs argument){
		functionEmbedDict[commandName](argument);
	}
}
public struct CommandArgs{
		public bool isSequential;
		public string args;
		public BasicInkExample commandCaller;
}