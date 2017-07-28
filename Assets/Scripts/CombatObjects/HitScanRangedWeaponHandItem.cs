using UnityEngine;

namespace SeaOfGreed {

    public class HitScanRangedWeaponHandItem : HandItem {

        public void Awake() {
            timeLeftForFire = 0;
            ranged = true;
            projectile = false;
        }

        public override void Fire(HandItem weapon, Vector3 target) {
            Debug.Log("Fired" + weapon.name);

            weapon.sprite.SetPositions(new Vector3[] { transform.position, target });

            weapon.sprite.enabled = true;

            var direction = target - transform.position;
            RaycastHit2D cast = Physics2D.Raycast(gameObject.transform.position, direction, weapon.range, shootableRaycastMask);

            if (cast) {
                if (cast.transform.gameObject.layer == uncrossableRaycastMask) {
                    Debug.Log("Cast hit wall");
                } else {
                    Debug.Log("Cast hit " + cast.transform.gameObject.name + "in layer " + cast.transform.gameObject.layer + " by weapon" + weapon.name);
                }
            }
        }
    }
}
