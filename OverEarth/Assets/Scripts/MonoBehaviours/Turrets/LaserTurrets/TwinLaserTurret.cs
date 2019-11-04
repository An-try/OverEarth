using UnityEngine;

namespace OverEarth
{
    public class TwinLaserTurret : Turret
    {
        [SerializeField] private GameObject _rightLaserBeam;
        [SerializeField] private GameObject _leftLaserBeam;

        public float _hitDuration;

        public override void SetTurretParameters()
        {
            base.SetTurretParameters();

            _hitDuration = _turretItem.HitDuration;
        }

        public override void Shoot()
        {
            _rightLaserBeam.GetComponent<Laser>().LaserLength = _turretRange; // Set the laset lenght to the right laser beam
            _rightLaserBeam.GetComponent<Laser>().SetHitDuration(_hitDuration); // Set the hit duration to the right laser beam

            _leftLaserBeam.GetComponent<Laser>().LaserLength = _turretRange; // Set the laset lenght to the left laser beam
            _leftLaserBeam.GetComponent<Laser>().SetHitDuration(_hitDuration); // Set the hit duration to the left laser beam

            _currentCooldown = _maxCooldown + _hitDuration; // Add a cooldown to this turret

            _audioSource.Play(); // Play an shoot sound
        }
    }
}
