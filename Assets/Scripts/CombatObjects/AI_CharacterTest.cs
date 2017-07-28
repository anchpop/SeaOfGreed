using System;
using SeaOfGreed;
using UnityEngine;

public class AI_CharacterTest : MonoBehaviour {
    private CharacterDriverNoAnimTest driver;

    [SerializeField] private GameObject childProjectile;
    [SerializeField] private GameObject player;
    private float timePassed = 3;
    private bool left;
    public int sightRange;
    private int timeToNext = 3;
    private Vector3 direction = Vector3.zero;
    private float rangeToPlayer;
    private HandItem weapon;

    public float escapeAtHealthPercent;

    private void Start() {
        //driver = gameObject.GetComponent<CharacterDriverNoAnimTest>(); //gameObject.GetComponent<CharacterDriver>()
        player = GameObject.FindGameObjectWithTag("Player"); //GameManager.gameManager.player;
        weapon = driver.hands[0];
        left = UnityEngine.Random.value >= .5;
    }

    private float timeSinceLastAttack = 0;

    private void Update() {
        rangeToPlayer = (player.transform.position - transform.position).magnitude;

        if (rangeToPlayer > sightRange) {
            // idle
        } else {
            if (driver.Health <= driver.MaxHealth * escapeAtHealthPercent) {
                //Escape
                direction = (player.transform.position - transform.position);
                direction.Scale(new Vector3(-5, -5));
                driver.lookInDirection(direction);
                driver.walkInDirection(direction);
            } else {
                if (rangeToPlayer <= weapon.range) {
                    driver.lookInDirection(player.transform.position - transform.position);
                    if (rangeToPlayer <= weapon.range / 4) {
                        direction = (player.transform.position - transform.position);
                        direction.Scale(new Vector3(-5, -5));
                    } else if (timePassed >= timeToNext) {
                        direction = GetNewVector(rangeToPlayer);
                        timePassed = 0;
                        timeToNext = UnityEngine.Random.Range(1, 3);
                    }
                    driver.walkInDirection(direction);
                    TryFire();

                    timePassed += Time.deltaTime;
                    timeSinceLastAttack += Time.deltaTime;
                } else if (rangeToPlayer <= sightRange) {
                    driver.lookInDirection(player.transform.position - transform.position);

                    driver.walkInDirection(player.transform.position - transform.position);
                }
            }
        }
    }

    private Vector3 GetNewVector(float range) {
        Vector3 output = Vector3.zero;
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

            left = !left;
            Vector3 offset = new Vector3(0, 0);
            var leftA = left ? -1 : 1;
            var angleA = UnityEngine.Random.Range(20, 40) * leftA;
            var sideC = (player.transform.position - transform.position);
            //              +-1.5
            var sideB = (UnityEngine.Random.value * 3) + weapon.range - 1.5;
            var sideA = (UnityEngine.Random.Range(3, 6));
            //var angleA = (Random.Range(20, 40));

            var angleToTurn = Mathf.Rad2Deg * Mathf.Asin((float)(sideB * ((Mathf.Sin(angleA)) / sideB)));
            output = new Vector3(((sideC.x * Mathf.Cos(angleToTurn)) + sideC.y * Mathf.Sin(angleToTurn)), ((-sideC.x * Mathf.Sin(angleToTurn)) + sideC.y * Mathf.Cos(angleToTurn)));
            output.Scale(new Vector3(20, 20));
        } else if (range <= weapon.range / 2) {
            Vector3 pathToTarget = player.transform.position - transform.position;
            output = Vector3.Scale(pathToTarget, new Vector3(-5, -5));
        }

        return output;
    }

    private void TryFire() {
        //driver.TryFire
    }
}
