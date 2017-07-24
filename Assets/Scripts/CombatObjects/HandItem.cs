using System;
using System.Linq;

namespace SeaOfGreed {

    public struct MeleeWeaponHandItem {
        public string name;
        public float range;
        public float damage;
        public float refireRate;
    }

    public struct RangedWeaponHandItem {
        public string name;
        public float range;
        public float damage;
        public float roundsPerMinute;
        public float roundsPerMagazine;
    }

    public struct WeaponHandItem {
        public string name;
        public float range;
        public float damage;
        public float fireRate;
        public float rounds;

        public bool ranged;
        public bool projectile;
    }
}
