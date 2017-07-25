using System;
using System.Linq;
using SeaOfGreed;
using UnityEngine;

public class AI_NewTest : MonoBehaviour {
    private CharacterDriverNoAnimTest driver;
    [SerializeField] private GameObject player;
    private bool left;
    public int sightRange;
    private float timePassed = 3;
    private float rangeToPlayer;
    private Vector3 dirToLook = Vector3.zero;
    private Vector3 dirToWalk = Vector3.zero;
    public bool runAwayIfTooClose;
    private float weaponRange;

    public float escapeAtHealthPercent;

    private void Start() {
        driver = gameObject.GetComponent<CharacterDriverNoAnimTest>(); //gameObject.GetComponent<CharacterDriver>()
        player = GameObject.FindGameObjectWithTag("Player"); //GameManager.gameManager.player;
        left = UnityEngine.Random.value >= .5;
        //Weapon with least range
        weaponRange = driver.hands.Select(x => x.range).OrderBy(x => x).First();
    }

    private float timeSinceLastAttack = 0;

    private void Update() {
        var dirToPlayer = player.transform.position - transform.position;
        rangeToPlayer = (dirToPlayer).magnitude;

        if (rangeToPlayer > sightRange) {
            Debug.Log("Idle");
            //idle
            timePassed = 3;
        } else { // in sight range
            if ((rangeToPlayer <= weaponRange / 4) && runAwayIfTooClose) { //Closer than quarter of weapon range
                //back up
                Debug.Log("Too Close");
                driver.lookInDirection(dirToPlayer);
                dirToPlayer.Scale(new Vector3(-1, -1));
                driver.walkInDirection(dirToPlayer);

                timePassed = 3;
            } else { // farther than quarter of weapon range
                dirToLook = dirToPlayer;

                if (rangeToPlayer > weaponRange) { //farther than weapon range
                    Debug.Log("In Sight");
                } else { // in weapon range
                    Debug.Log("In Weapon Range");
                }

                if (timePassed >= 3) {
                    if (rangeToPlayer > weaponRange) { //farther than weapon range
                        Debug.Log("New Vector: In Sight");
                        dirToWalk = dirToPlayer;
                    } else { // in weapon range
                        Debug.Log("New Vector: In Weapon Range");
                        dirToWalk = GetNewVector(rangeToPlayer);
                    }
                    timePassed = UnityEngine.Random.Range(0, 2);
                }
                driver.lookInDirection(dirToLook);
                driver.walkInDirection(dirToWalk);
                timePassed += Time.deltaTime;
            }
        }

        foreach (var weapon in driver.hands) {
            if (rangeToPlayer <= weapon.range) {
                driver.TryFire(weapon, dirToPlayer);
            }
        }
    }

    private Vector3 GetNewVector(float range) {
        Vector3 output = Vector3.zero;

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

        left = !left;
        Vector3 offset = new Vector3(0, 0);
        var leftA = left ? -1 : 1;
        var angleA = 25 * leftA;  //UnityEngine.Random.Range(20, 40) * leftA;
        var sideC = (player.transform.position - transform.position);
        //              +-1.5
        var sideB = weaponRange * .75;  //(UnityEngine.Random.value * 3) + weaponRange - 1.5;
        //var sideA = (UnityEngine.Random.Range(3, 6));
        //var angleA = (Random.Range(20, 40));

        var angleToTurn = Mathf.Rad2Deg * Mathf.Asin((float)(sideB * ((Mathf.Sin(angleA)) / sideB)));
        output = new Vector3(((sideC.x * Mathf.Cos(angleToTurn)) + sideC.y * Mathf.Sin(angleToTurn)), ((-sideC.x * Mathf.Sin(angleToTurn)) + sideC.y * Mathf.Cos(angleToTurn)));
        output.Scale(new Vector3(20, 20));

        return output;
    }
}
