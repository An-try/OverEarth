using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public abstract class Damageable : MonoBehaviour
    {
        public event Action<float, float> TakeDamageEvent;

        private protected float _maxDurability;
        private protected float _currentDurability;
        private protected float _maxArmor;
        private protected float _currentArmor;

        public float MaxDurability => _maxDurability;
        public float CurrentDurability => _currentDurability;
        public float MaxArmor => _maxArmor;
        public float CurrentArmor => _currentArmor;

        private protected virtual void Start()
        {
            TakeDamageEvent?.Invoke(_maxDurability, _currentDurability);
        }

        private protected abstract IEnumerator PlayDestroyAnimation();

        public void DoDamage(float damage)
        {
            if (_currentArmor <= 0)
            {
                _currentArmor = 0.00001f;
            }

            float damageToDurability = damage - (damage / (_maxDurability / _currentArmor));
            if (damageToDurability < 0)
            {
                damageToDurability = 0;
            }
            _currentDurability -= damageToDurability;


            float currentArmor = _currentArmor;
            _currentArmor -= damage;
            damage -= currentArmor;
            if (damage > 0)
            {
                _currentDurability -= damage;
            }

            TakeDamageEvent?.Invoke(_maxDurability, _currentDurability);

            if (_currentDurability <= 0)
            {
                StartCoroutine(PlayDestroyAnimation());
            }
        }

        private protected void DestroyGameObject()
        {

        }
    }
}
