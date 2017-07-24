using System.Collections;
using UnityEngine;

namespace SeaOfGreed {

    public class damage : MonoBehaviour {
        [SerializeField] private int damageAmount;
        [SerializeField] private string hitTag;
        public int range;

        //on enable (which happens from playercombatobject), it checks if there's an enemy touching it, and if so, damages it. Then, 0.2s later, it hides itself.
        private IEnumerator TemporarilyDeactivate() {
            yield return new WaitForSeconds(.2f);
            gameObject.SetActive(false);
        }

        private void OnEnable() {
            StartCoroutine("TemporarilyDeactivate");
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector2(0f, 0f));
            for (int i = 0; i < hits.Length; i++) {
                if (hits[i].collider.gameObject.tag == hitTag)
                    hits[i].collider.gameObject.GetComponent<CombatObject>().damage(damageAmount, ((transform.parent.position) - (hits[i].collider.gameObject.transform.position)).normalized * 1.5f);
            }
        }
    }
}
