using UnityEngine;

namespace OverEarth
{
    public class MissileLauncher : Turret
    {
        [SerializeField] private GameObject _missilePrefab;

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
            GameObject missile = Instantiate(_missilePrefab, _turretCannons.transform.position, _turretCannons.transform.rotation); // Create a new missile
            missile.GetComponent<Rocket>().TargetTags = _targetTags;

            _currentCooldown = _maxCooldown; // Add a cooldown to this turret

            _audioSource.Play(); // Play an shoot sound
        }
    }
}
