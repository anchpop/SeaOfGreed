using System;
using System.Linq;
using UnityEngine;

namespace SeaOfGreed {

    public class MeleeWeaponHandItem : HandItem {
    }

    public class RangedWeaponHandItem : HandItem {
    }

    public class HandItem : MonoBehaviour {
        public GameObject sprite;
        public string name;
        public float range;
        public float damage;
        public float fireRate;
        public float rounds;

        public bool ranged;
        public bool projectile;

        public float timeLeftForFire;
        public float TimeBetweenFire { get { return 1 / fireRate; } }

        private void Start() {
            timeLeftForFire = 0;
        }
    }
}
