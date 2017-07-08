using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CombatObject : MonoBehaviour {
	//outlines the basic properties a combat object must have.
	[SerializeField] private int maxHP;
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
	public void damage(int amount){
		if(amount>defense){
			HP -= (amount - defense);
			onHPChange();
		}
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
}
