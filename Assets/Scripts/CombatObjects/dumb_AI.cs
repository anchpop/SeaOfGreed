using SeaOfGreed;
using UnityEngine;

public class dumb_AI : MonoBehaviour {
    private CharacterDriver driver;
    [SerializeField] private GameObject childProjectile;
    [SerializeField] private GameObject player;

    private void Start() {
        driver = gameObject.GetComponent<CharacterDriver>();
    }

    private float timeSinceLastAttack = 0;

    private void Update() {
        if ((player.transform.position - transform.position).magnitude <= 5) {
            driver.lookInDirection(player.transform.position - transform.position);
            driver.walkInDirection(player.transform.position - transform.position);
            if (timeSinceLastAttack >= 0.5f) {
                timeSinceLastAttack = 0;
                childProjectile.SetActive(true);
            }
        }
        timeSinceLastAttack += Time.deltaTime;
    }
}