using SeaOfGreed;
using UnityEngine;

public class slightlybetter_AI : MonoBehaviour {
    private CharacterDriver driver;
    [SerializeField] private GameObject childProjectile;
    [SerializeField] private GameObject player;
    public damage weapon;
    private Random rand = new Random();
    private float timePassed = 3;
    private Vector3 output = Vector3.zero;
    private bool left;
    public int sightRange;

    public PauseMenu pauseMenu;

    private void Start() {
        driver = gameObject.GetComponent<CharacterDriver>();
        weapon = childProjectile.GetComponent<damage>();
        left = Random.value >= .5;
    }

    private float timeSinceLastAttack = 0;

    private void Update() {
        if (!pauseMenu.paused) {
            var range = (player.transform.position - transform.position).magnitude;
            if (range <= sightRange) {
                if (range >= weapon.range - 1) {
                    driver.walkInDirection(player.transform.position - transform.position);
                }
                driver.lookInDirection(player.transform.position - transform.position);

                if (timeSinceLastAttack >= 1.5f) {
                    timeSinceLastAttack = 0;
                    childProjectile.SetActive(true);
                }
            }
            if (range <= weapon.range) {
                if (range > weapon.range / 2) {
                    // Thanks mr. Robinson!
                    //    ___ Self
                    //  /
                    // |
                    // V      C
                    //(B)------------(A)  <---Player
                    // |             /
                    // |           /
                    // |         /
                    // A       /  B
                    // |     /
                    // |   /
                    // | /
                    // /
                    //(C)
                    //

                    if (timePassed >= Random.Range(.3f, 2f)) {
                        left = !left;
                        Vector3 offset = new Vector3(0, 0);
                        var angle = left ? -25 : 25;
                        var sideC = (player.transform.position - transform.position);
                        //              +-1.5
                        var sideB = (Random.value * 3) + weapon.range - 1.5;
                        var sideA = (Random.Range(3, 6));
                        var angleA = (Random.Range(20, 40));

                        var angleToTurn = Mathf.Rad2Deg * Mathf.Asin((float)(sideB * ((Mathf.Sin(angleA)) / sideB)));
                        output = new Vector3(((sideC.x * Mathf.Cos(angleToTurn)) + sideC.y * Mathf.Sin(angleToTurn)), ((-sideC.x * Mathf.Sin(angleToTurn)) + sideC.y * Mathf.Cos(angleToTurn)));
                        output.Scale(new Vector3(20, 20));
                        timePassed = 0;
                    }
                } else if (range <= weapon.range / 2) {
                    Vector3 pathToTarget = player.transform.position - transform.position;
                    output = Vector3.Scale(pathToTarget, new Vector3(-5, -5));
                }
                driver.walkInDirection(output);
                timePassed += Time.deltaTime;

                timeSinceLastAttack += Time.deltaTime;
            }
        }
    }
}