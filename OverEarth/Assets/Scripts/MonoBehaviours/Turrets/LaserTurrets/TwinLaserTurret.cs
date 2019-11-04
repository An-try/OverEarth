using UnityEngine;

namespace OverEarth
{
    public class TwinLaserTurret : Turret
    {
        public GameObject RightLaserBeam;
        public GameObject LeftLaserBeam;

        public float hitDuration;

        public override void SetTurretParameters()
        {
            base.SetTurretParameters();

            hitDuration = 0.1f;
        }

        public override void Shoot()
        {
            RightLaserBeam.GetComponent<Laser>().LaserLength = _turretRange; // Set the laset lenght to the right laser beam
            RightLaserBeam.GetComponent<Laser>().SetHitDuration(hitDuration); // Set the hit duration to the right laser beam

            LeftLaserBeam.GetComponent<Laser>().LaserLength = _turretRange; // Set the laset lenght to the left laser beam
            LeftLaserBeam.GetComponent<Laser>().SetHitDuration(hitDuration); // Set the hit duration to the left laser beam

            _currentCooldown = _maxCooldown + hitDuration; // Add a cooldown to this turret

            GetComponent<AudioSource>().Play(); // Play an shoot sound
        }
    }
}
