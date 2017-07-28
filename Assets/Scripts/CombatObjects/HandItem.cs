using System;
using UnityEngine;

namespace SeaOfGreed {

    public abstract class HandItem : MonoBehaviour {
        public LineRenderer sprite; //Line Renderer for temporary sprite usage
        public string name;
        public float range;
        public float damage;
        public float fireRate;
        public float rounds;

        public bool ranged;
        public bool projectile;

        public LayerMask shootableRaycastMask;
        public LayerMask uncrossableRaycastMask;

        public float timeLeftForFire;
        public float TimeBetweenFire { get { return 60 / fireRate; } }

        public abstract void Fire(HandItem weapon, Vector3 target);
    }
}
