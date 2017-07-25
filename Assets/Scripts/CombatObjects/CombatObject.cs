using TeamUtility.IO;
using UnityEngine;

namespace SeaOfGreed {

    public class CombatObject : MonoBehaviour {

        //outlines the basic properties a combat object must have.
        [SerializeField] private int maxHealth;

        private Vector3 currentKnockback;
        [SerializeField] private int defense;
        private int health;

        public CharacterDriver driver;
        public GameManager manager;

        public Camera mainCamera;
        public LayerMask playerMask;

        public GameObject sprite;

        private void Start() {
            health = maxHealth;
            driver = GetComponent<CharacterDriver>();
            manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            mainCamera = manager.mainCameraBlackout.GetComponent<Camera>();
        }

        public int MaxHealth { get { return maxHealth; } }
        public int Health { get { return health; } }
        public int Defense { get { return defense; } }

        private void Fire(HandItem weapon) {
            Debug.Log("Fired" + weapon.name);
            Vector2 mousePoint = gameObject.transform.position - mainCamera.ScreenToWorldPoint(InputManager.mousePosition).normalized;
            if (weapon.ranged) { //Guns
            } else { //Melee
                //Run melee animation
                sprite.SetActive(true);

                RaycastHit2D cast = Physics2D.Raycast(gameObject.transform.position, mousePoint, weapon.range, playerMask);
                if (cast) {
                    Debug.Log("Cast Hit by weapon" + weapon.name);
                }
            }
        }

        public void TryFire(HandItem weapon) {
            if (weapon.timeLeftForFire <= 0) {
                Fire(weapon);
                weapon.timeLeftForFire = weapon.TimeBetweenFire;
            }
        }

        public void damage(int amount, Vector3 knockback) {
            if (amount > defense) {
                health -= (amount - defense);
                onHPChange();
            }
            Debug.Log(knockback + "=kb");
            currentKnockback += knockback;
        }

        protected virtual void onHPChange() {
            Debug.Log("hpchange in object " + gameObject.name);
            if (health <= 0) {
                onDeath();
            }
        }

        protected virtual void onDeath() {
            Destroy(gameObject);
        }

        //knockback is queued up, then applied at a speed of 8 per second.
        private void Update() {
            if (currentKnockback.magnitude > 0) {
                Vector3 knockbackThisFrame = currentKnockback.normalized * Time.deltaTime * 8;
                if (knockbackThisFrame.magnitude > currentKnockback.magnitude)
                    knockbackThisFrame = currentKnockback;
                transform.position -= knockbackThisFrame;
                currentKnockback -= knockbackThisFrame;
            }
            childUpdate();
        }

        protected virtual void childUpdate() {
        }
    }
}
