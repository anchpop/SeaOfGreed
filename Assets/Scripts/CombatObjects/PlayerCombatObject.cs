using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombatObject : CombatObject {
	//combat object; if you hit C it spawns a projectile (controlled by damage.cs)
	//TODO: move this control to something better when we flesh out attacking.
	[SerializeField] private GameObject childObject;
	private void onDeath(){
        SceneManager.LoadScene("testscene1");
	}
	void Update(){
		if(Input.GetKeyDown(KeyCode.C))
			childObject.SetActive(true);
	}
}
