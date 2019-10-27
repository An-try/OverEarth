using UnityEngine;

namespace OverEarth
{
    public class MissileLauncher : Turret
    {
        public GameObject MissilePrefab;

        public override void SetTurretParameters()
        {
            base.SetTurretParameters();

            turnRate = 10f;
            _turretRange = 50000f;
            cooldown = 5f;
            currentCooldown = cooldown;

            _rightTraverse = 180f;
            _leftTraverse = 180f;
            _elevation = 60f;
            _depression = 5f;
        }

        public override bool AimedAtEnemy()
        {
            // Missile launcher always "aimed on enemy" if there is the nearest target
            if (_target != null)
            {
                return true;
            }
            return false;
        }

        public override void Shoot()
        {
            GameObject missile = Instantiate(MissilePrefab, TurretCannons.transform.position, TurretCannons.transform.rotation); // Create a new missile
            missile.GetComponent<Rocket>().TargetTags = TargetTags;

            currentCooldown = cooldown; // Add a cooldown to this turret

            GetComponent<AudioSource>().Play(); // Play an shoot sound
        }
    }
}
