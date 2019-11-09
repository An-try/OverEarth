using UnityEngine;

namespace OverEarth
{
    [CreateAssetMenu(fileName = "New laser turret", menuName = "OverEarth/Items/Turrets/Laser Turret")]
    public class LaserTurretEquipment : TurretItem
    {
        [SerializeField] private float _hitDuration = 0;
        
        public override float HitDuration => _hitDuration;
    }
}
