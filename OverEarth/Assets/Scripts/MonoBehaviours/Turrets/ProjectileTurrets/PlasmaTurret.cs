using UnityEngine;

namespace OverEarth
{
    public class PlasmaTurret : Turret
    {
        public GameObject ProjectilePrefab;
        public GameObject RightShootPlace;
        public GameObject LeftShootPlace;

        private float bulletForce;
        private float turretScatter;

        public override void SetTurretParameters()
        {
            base.SetTurretParameters();

            turnRate = 30f;
            _turretRange = 50000f;
            cooldown = 2f;
            currentCooldown = cooldown;

            _rightTraverse = 180f;
            _leftTraverse = 180f;
            _elevation = 60f;
            _depression = 5f;

            bulletForce = 50000f;
            turretScatter = 0.001f;

            ShootPlace = RightShootPlace;
        }

        public override void Shoot()
        {
            // Change shoot place(right or left in turn)
            if (ShootPlace == RightShootPlace)
            {
                ShootPlace = LeftShootPlace;
            }
            else
            {
                ShootPlace = RightShootPlace;
            }

            // Scatter while firing
            Vector3 scatter = new Vector3(Random.Range(-turretScatter, turretScatter),
                                          Random.Range(-turretScatter, turretScatter),
                                          Random.Range(-turretScatter, turretScatter));

            // Creating shoot animation with position and rotation of the shoot place. Also parent shoot animation to the shoot place
            GameObject shootAnimation = Instantiate(ShootAnimationPrefab, ShootPlace.transform.position, ShootPlace.transform.rotation, ShootPlace.transform);
            Destroy(shootAnimation.gameObject, shootAnimation.GetComponent<ParticleSystem>().main.duration);

            // Creating bullet with position and rotation of the shoot place
            GameObject bullet = Instantiate(ProjectilePrefab, ShootPlace.transform.position, ShootPlace.transform.rotation);
            // Add force to the bullet so it will fly directly
            bullet.GetComponent<Rigidbody>().AddForce((ShootPlace.transform.forward + scatter) * bulletForce);

            currentCooldown = cooldown; // Add a cooldown to this turret

            GetComponent<AudioSource>().Play(); // Play an shoot sound
        }
    }
}
