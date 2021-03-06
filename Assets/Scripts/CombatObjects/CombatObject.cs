﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CombatObject : MonoBehaviour {
	//outlines the basic properties a combat object must have.
	[SerializeField] private int maxHP;
	private Vector3 currentKnockback;
	[SerializeField] private int defense;
	public  int HP;
	void Start () {
		HP = maxHP;
	}
	public int getMaxHP(){
		return maxHP;
	}
	public int getDefense(){
		return defense;
	}
	public int getHP(){
		return HP;
	}
	public void damage(int amount, Vector3 knockback){
		if(amount>defense){
			HP -= (amount - defense);
			onHPChange();
		}
		Debug.Log(knockback + "=kb");
		currentKnockback += knockback;
	}
	protected virtual void onHPChange(){
		Debug.Log("hpchange in object " + gameObject.name);
		if(HP<=0){
			onDeath();
		}
	}
	protected virtual void onDeath(){
		Destroy(gameObject);
	}
	//knockback is queued up, then applied at a speed of 8 per second.
	void Update(){
		if(currentKnockback.magnitude > 0){
			Vector3 knockbackThisFrame = currentKnockback.normalized * Time.deltaTime * 8;
			if(knockbackThisFrame.magnitude > currentKnockback.magnitude)
				knockbackThisFrame = currentKnockback;
			transform.position -= knockbackThisFrame;
			currentKnockback -= knockbackThisFrame;
		}
		childUpdate();
	}
	protected virtual void childUpdate(){}
}
