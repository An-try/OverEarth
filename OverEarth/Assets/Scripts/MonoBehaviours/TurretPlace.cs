using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class TurretPlace : Damageable
    {
        private Turret _turret;

        private void Awake()
        {
            _maxDurability = 2000;
            _currentDurability = _maxDurability;
            _maxArmor = 300;
            _currentArmor = _maxArmor;
        }

        private protected override void Start()
        {
            base.Start();

            _turret = GetComponentInChildren<Turret>();
        }

        private protected override IEnumerator PlayDestroyAnimation()
        {
            yield return new WaitForSeconds(1);

            DestroyGameObject();

            yield break;
        }
    }
}
