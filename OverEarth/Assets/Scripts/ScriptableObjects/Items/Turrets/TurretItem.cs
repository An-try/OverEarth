using UnityEngine;

namespace OverEarth
{
    public abstract class TurretItem : Item
    {
        [SerializeField] private float _maxCooldown = 0;
        [SerializeField] private float _range = 0;
        [SerializeField] private float _turnRate = 0; // Turret turning speed

        [Range(0.0f, 180.0f)] [SerializeField] private float _rightTraverse = 0; // Maximum right turn in degrees
        [Range(0.0f, 180.0f)] [SerializeField] private float _leftTraverse = 0; // Maximum left turn in degrees
        [Range(0.0f, 90.0f)] [SerializeField] private float _elevation = 0; // Maximum turn up in degrees
        [Range(0.0f, 90.0f)] [SerializeField] private float _depression = 0; // Maximum turn down in degrees

        public float MaxCooldown => _maxCooldown;
        public float Range => _range;
        public float TurnRate => _turnRate;

        public float RightTraverse => _rightTraverse;
        public float LeftTraverse => _leftTraverse;
        public float Elevation => _elevation;
        public float Depression => _depression;

        public override bool IsTurret => true;

        /// <summary>
        /// Use in laser turrets.
        /// </summary>
        public virtual float HitDuration
        {
            get
            {
                Debug.LogError("This turret has no hit duration");
                return 0;
            }
        }

        /// <summary>
        /// Use in projectile turrets.
        /// </summary>
        public virtual float BulletForce
        {
            get
            {
                Debug.LogError("This turret has no bullet force");
                return 0;
            }
        }

        /// <summary>
        /// Use in projectile turrets.
        /// </summary>
        public virtual float TurretScatter
        {
            get
            {
                Debug.LogError("This turret has no bullet force");
                return 0;
            }
        }
    }
}
