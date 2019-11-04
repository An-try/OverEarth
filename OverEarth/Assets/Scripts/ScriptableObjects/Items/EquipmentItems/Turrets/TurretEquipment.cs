using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    [CreateAssetMenu(fileName = "New turret", menuName = "OverEarth/Items/Turret")]
    public class TurretEquipment : EquipmentItem
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
    }
}
