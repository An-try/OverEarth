using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class Station : Damageable
    {
        [SerializeField] private List<TurretsSection> _turretsSections;

        private void Awake()
        {
            _maxDurability = 50000;
            _currentDurability = _maxDurability;
            _maxArmor = 30000;
            _currentArmor = _maxArmor;
        }

        private protected override void DestroyObject()
        {
            base.DestroyObject();

            for (int i = 0; i < _turretsSections.Count; i++)
            {
                _turretsSections[i].DoDamage(int.MaxValue);
            }
        }
    }
}
