using UnityEngine;

namespace OverEarth
{
    public class PlasmaTurret : Turret
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private GameObject _rightShootPlace;
        [SerializeField] private GameObject _leftShootPlace;

        private float _bulletForce;
        private float _turretScatter;

        public override void SetTurretParameters()
        {
            base.SetTurretParameters();
            
            _bulletForce = _turretEquipment.BulletForce;
            _turretScatter = _turretEquipment.TurretScatter;

            _shootPlace = _rightShootPlace;
        }

        public override void Shoot()
        {
            // Change shoot place(right or left in turn)
            _shootPlace = _shootPlace == _rightShootPlace ? _leftShootPlace : _rightShootPlace;

            // Scatter while firing
            Vector3 scatter = new Vector3(Random.Range(-_turretScatter, _turretScatter),
                                          Random.Range(-_turretScatter, _turretScatter),
                                          Random.Range(-_turretScatter, _turretScatter));

            // Creating shoot animation with position and rotation of the shoot place. Also parent shoot animation to the shoot place
            GameObject shootAnimation = Instantiate(_shootAnimationPrefab, _shootPlace.transform.position, _shootPlace.transform.rotation, _shootPlace.transform);
            Destroy(shootAnimation.gameObject, shootAnimation.GetComponent<ParticleSystem>().main.duration);

            // Creating bullet with position and rotation of the shoot place
            GameObject bullet = Instantiate(_projectilePrefab, _shootPlace.transform.position, _shootPlace.transform.rotation);
            // Add force to the bullet so it will fly directly
            bullet.GetComponent<Rigidbody>().AddForce((_shootPlace.transform.forward + scatter) * _bulletForce);

            _currentCooldown = _maxCooldown; // Add a cooldown to this turret

            _audioSource.Play(); // Play an shoot sound
        }
    }
}
