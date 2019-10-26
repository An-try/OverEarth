using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class TurretsSection : Damageable
    {
        [SerializeField] private TurretPlace[] _turretPlaces;

        private void Awake()
        {
            _maxDurability = 14000;
            _currentDurability = _maxDurability;
            _maxArmor = 5000;
            _currentArmor = _maxArmor;
        }

        private protected override IEnumerator PlayDestroyAnimation()
        {
            yield return new WaitForSeconds(1);

            DestroyGameObject();

            yield break;
        }
    }
}
