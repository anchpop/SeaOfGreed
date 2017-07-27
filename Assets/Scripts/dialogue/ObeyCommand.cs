using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObeyCommand : MonoBehaviour {
	public void move(CommandArgs args){
		StartCoroutine(moveCorout(args));
	}
	public void wait(CommandArgs args){
		StartCoroutine(waitCorout(args));
	}
	//YARRRRRR
	private IEnumerator moveCorout(CommandArgs arrgs){
		string[] argSplit = arrgs.args.Split(',');
		int[] argSplitInt = new int[3];
		for(int i = 0; i<3; i++){
			int.TryParse(argSplit[i], out argSplitInt[i]);
		}
		Vector3 diff = new Vector3(argSplitInt[0], argSplitInt[1], argSplitInt[2]);
		while(diff.magnitude > Time.deltaTime*3){
			diff -= diff.normalized*Time.deltaTime*3;
			transform.position += diff.normalized*Time.deltaTime*3;
			yield return new WaitForFixedUpdateInterruptable(arrgs.commandCaller);
		}
		transform.position += diff;
		arrgs.commandCaller.endOfCommand(arrgs);
	}
	private IEnumerator waitCorout(CommandArgs args){
		float x;
		float.TryParse(args.args, out x);
		yield return new WaitForSecondsInterruptable(x, args.commandCaller);
		args.commandCaller.endOfCommand(args);
	}
}
public class WaitForSecondsInterruptable : CustomYieldInstruction{
	private float waitCountDown;
	private InkProcessor commandCaller;
	public override bool keepWaiting{
		get {
			if(!commandCaller.pauseCoroutines){
				waitCountDown -= Time.deltaTime;
				if(waitCountDown<=0)return false;
			}
			return true;
		}
	}
	public WaitForSecondsInterruptable(float cd, InkProcessor caller){
		waitCountDown = cd;
		commandCaller = caller;
	}
}
public class WaitForFixedUpdateInterruptable : CustomYieldInstruction{
	private InkProcessor commandCaller;
	private bool hasRunForAFrame = false;
	public override bool keepWaiting{
		get {
			if(!commandCaller.pauseCoroutines){
				if(hasRunForAFrame)return false;
				hasRunForAFrame = true;
			}
			return true;
		}
	}
	public WaitForFixedUpdateInterruptable(InkProcessor caller){
		commandCaller = caller;
	}
}