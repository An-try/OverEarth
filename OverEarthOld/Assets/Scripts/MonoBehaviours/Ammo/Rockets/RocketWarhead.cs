using UnityEngine;

namespace OverEarth
{
    /// <summary>
    /// Warheads for any rocket.
    /// </summary>
    [CreateAssetMenu(fileName = "New rocket warhead", menuName = "OverEarth/Items/Rocket Parts/Rocket Warhead")]
    public class RocketWarhead : Item
    {
        [SerializeField] private float _maxDurability = 0;
        [SerializeField] private float _maxArmor = 0;
        [SerializeField] private float _damage = 0;
        [SerializeField] private float _mass = 0;

        public float MaxDurability => _maxDurability;
        public float MaxArmor => _maxArmor;
        public float Damage => _damage;
        public float Mass => _mass;
    }
}
