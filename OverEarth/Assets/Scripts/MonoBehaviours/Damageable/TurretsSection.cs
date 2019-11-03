using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class TurretsSection : Damageable
    {
        [SerializeField] private List<TurretPlace> _turretPlaces;

        private void Awake()
        {
            _maxDurability = 14000;
            _currentDurability = _maxDurability;
            _maxArmor = 5000;
            _currentArmor = _maxArmor;
        }

        private protected override void DestroyObject()
        {
            base.DestroyObject();

            for (int i = 0; i < _turretPlaces.Count; i++)
            {
                _turretPlaces[i].DoDamage(MinMaxValuesConstants.MAX_DAMAGE);
            }
        }
    }
}
