using UnityEngine;

namespace OverEarth
{
    [CreateAssetMenu(fileName = "New laser turret", menuName = "OverEarth/Items/Laser Turret")]
    public class LaserTurretEquipment : TurretEquipment
    {
        [SerializeField] private float _hitDuration = 0;
        
        public override float HitDuration => _hitDuration;
    }
}
