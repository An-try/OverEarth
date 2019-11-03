﻿using UnityEngine;

namespace OverEarth
{
    /// <summary>
    /// Class for any projectile.
    /// </summary>
    public abstract class Projectile : Ammo
    {
        private float _lifeTime = 60f;

        private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
        {
            _kineticDamage = 300f;

            Destroy(gameObject, _lifeTime); // Destroy plasma projectile after a while
        }

        private void OnCollisionEnter(Collision collision) // Called when this collider/rigidbody has begun touching another rigidbody/collider
        {
            DoDamage(_kineticDamage, collision);
            DestroyProjectile(); // Destroy this projectile
        }

        private void DestroyProjectile()
        {
            GameObject spark = Instantiate(SparksPrefab); // Instantiate sparks
            spark.transform.position = transform.position; // Set sparks position
            Destroy(spark, 0.3f); // Destroy sparks after some time
            Destroy(gameObject); // Destroy this projectile
        }
    }
}