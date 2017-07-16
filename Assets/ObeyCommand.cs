﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObeyCommand : MonoBehaviour {
	public void move(CommandArgs args){
		StartCoroutine(moveCorout(args));
	}
	private IEnumerator moveCorout(CommandArgs args){
		string[] argSplit = args.args.Split(',');
		int[] argSplitInt = new int[3];
		for(int i = 0; i<3; i++){
			int.TryParse(argSplit[i], out argSplitInt[i]);
		}
		Vector3 diff = new Vector3(argSplitInt[0], argSplitInt[1], argSplitInt[2]);
		while(diff.magnitude > Time.deltaTime*3){
			diff -= diff.normalized*Time.deltaTime*3;
			transform.position += diff.normalized*Time.deltaTime*3;
			yield return new WaitForFixedUpdate();
		}
		transform.position += diff;
		BasicInkExample.inkHolder.endOfCommand(args);
	}
}
