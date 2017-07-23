using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandController : MonoBehaviour {
    private static Dictionary<string, Action<CommandArgs>> functionEmbedDict = new Dictionary<string, Action<CommandArgs>>();
	void Awake () {
        if (GameObject.FindGameObjectsWithTag("GameController").Length > 1)
        {
            Destroy(this);
            return;
        }
		functionEmbedDict.Add("test", testMethod);
		functionEmbedDict.Add("move", moveObject);
		functionEmbedDict.Add("wait", waitObject);
	}
	static void testMethod(CommandArgs s){
		Debug.Log(s.args);
		s.commandCaller.endOfCommand(s);
	}

	public static void runCommand(String commandName, CommandArgs argument){
        Debug.Log("Running command " + commandName);
		functionEmbedDict[commandName](argument);
	}
	private static void moveObject(CommandArgs s){
		firstArgGO(s, 3, out s).GetComponent<ObeyCommand>().move(s);
	}
	private static void waitObject(CommandArgs s){
		firstArgGO(s, 1, out s).GetComponent<ObeyCommand>().wait(s);
	}
	private static GameObject firstArgGO(CommandArgs s, int expectedLengthExclGOName, out CommandArgs outs){
		string[] args = s.args.Split(',');
		GameObject toRun;
		if(args.Length == expectedLengthExclGOName + 1){
		toRun = GameObject.Find(args[0]);
		//removes first argument, the gameobject name
		s.args = "";
		for(int i = 1; i<args.Length; i++){
			s.args += args[i];
			s.args += ",";
		}
		} else{
			toRun = s.commandCaller.gameObject;
		}
		outs = s;
		return toRun;
	}
}
public struct CommandArgs{
		public bool isSequential;
		public string args;
		public BasicInkExample commandCaller;
}