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

        private protected override IEnumerator PlayDestroyAnimation()
        {
            yield return new WaitForSeconds(1);

            DestroyGameObject();

            yield break;
        }
    }
}
