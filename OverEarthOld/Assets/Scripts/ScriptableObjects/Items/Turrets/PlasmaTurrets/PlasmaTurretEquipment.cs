using UnityEngine;

namespace OverEarth
{
    [CreateAssetMenu(fileName = "New plasma turret", menuName = "OverEarth/Items/Turrets/Plasma Turret")]
    public class PlasmaTurretEquipment : TurretItem
    {
        [SerializeField] private float _bulletForce = 0;
        [SerializeField] private float _turretScatter = 0;
        
        public override float BulletForce => _bulletForce;
        public override float TurretScatter => _turretScatter;
    }
}
