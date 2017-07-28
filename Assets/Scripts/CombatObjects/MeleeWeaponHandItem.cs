using System.Linq;
using UnityEngine;

namespace SeaOfGreed {

    public class MeleeWeaponHandItem : HandItem {
        public float coneOfFire;

        public void Awake() {
            timeLeftForFire = 0;
            ranged = false;
            projectile = false;
        }

        public override void Fire(HandItem weapon, Vector3 target) {
            Debug.Log("Fired" + weapon.name);

            weapon.sprite.SetPositions(new Vector3[] { transform.position, target });

            weapon.sprite.enabled = true;

            var direction = target - transform.position;
            RaycastHit2D[] cast = Physics2D.CircleCastAll(gameObject.transform.position, coneOfFire, direction, weapon.range, shootableRaycastMask);

            if (cast.Any()) {
                Debug.Log("Cast Hit by weapon" + weapon.name);
            }
        }
    }
}
