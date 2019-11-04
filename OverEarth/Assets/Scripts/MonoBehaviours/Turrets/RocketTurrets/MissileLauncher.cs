using UnityEngine;

namespace OverEarth
{
    public class MissileLauncher : Turret
    {
        public GameObject MissilePrefab;

        public override bool AimedAtEnemy()
        {
            // Missile launcher always "aimed on enemy" if there is the nearest target
            if (_target && _targetPart)
            {
                return true;
            }
            return false;
        }

        public override void Shoot()
        {
            GameObject missile = Instantiate(MissilePrefab, TurretCannons.transform.position, TurretCannons.transform.rotation); // Create a new missile
            missile.GetComponent<Rocket>().TargetTags = TargetTags;

            _currentCooldown = _maxCooldown; // Add a cooldown to this turret

            GetComponent<AudioSource>().Play(); // Play an shoot sound
        }
    }
}
