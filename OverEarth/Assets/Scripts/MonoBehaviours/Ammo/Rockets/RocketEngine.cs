using UnityEngine;

namespace OverEarth
{
    /// <summary>
    /// Engines for any rocket.
    /// </summary>
    [CreateAssetMenu(fileName = "New rocket engine", menuName = "OverEarth/Items/Rocket Parts/Rocket Engine")]
    public class RocketEngine : Item
    {
        [SerializeField] private float _maxDurability = 0;
        [SerializeField] private float _maxArmor = 0;
        [SerializeField] private float _mass = 0;
        [SerializeField] private float _maxVelocity = 0;
        [SerializeField] private float _turnRate = 0;

        public float MaxDurability => _maxDurability;
        public float MaxArmor => _maxArmor;
        public float Mass => _mass;
        public float MaxVelocity => _maxVelocity;
        public float TurnRate => _turnRate;
    }
}
