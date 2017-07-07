using System.Collections;
using UnityEngine;
using SeaOfGreed;
public class damage : MonoBehaviour {
	//on enable (which happens from playercombatobject), it checks if there's an enemy touching it, and if so, damages it. Then, 0.2s later, it hides itself.
	IEnumerator TemporarilyDeactivate() {
        yield return new WaitForSeconds(.2f);
        gameObject.SetActive(false);
    }
	void OnEnable(){
		StartCoroutine("TemporarilyDeactivate");
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector2(0f, 0f));
		Debug.Log(hits.Length);
		for(int i = 0; i<hits.Length; i++){
			if(hits[i].collider.gameObject.tag == "enemy")
			hits[i].collider.gameObject.GetComponent<CombatObject>().damage(10);
		}
	}
}
