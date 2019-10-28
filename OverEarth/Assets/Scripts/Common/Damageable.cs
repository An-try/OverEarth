using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private List<MonoBehaviour> _functionalСomponents;

        public DamageablePartTypes DamageablePartType;

        public event Action TakeDamageEvent;
        public event Action DestroyedEvent;

        private protected float _maxDurability;
        private protected float _currentDurability;
        private protected float _maxArmor;
        private protected float _currentArmor;

        public float MaxDurability => _maxDurability;
        public float CurrentDurability => _currentDurability;
        public float MaxArmor => _maxArmor;
        public float CurrentArmor => _currentArmor;

        public Vector3 Position => transform.position;

        /// <summary>
        /// For Debugging. MaxDurability = 10000; CurrentDurability = 10000; MaxArmor = 5000; CurrentArmor = 5000.
        /// </summary>
        public void SetDefaultParameters()
        {
            _maxDurability = 10000;
            _currentDurability = _maxDurability;
            _maxArmor = 5000;
            _currentArmor = _maxArmor;
        }

        public void InvokeTakingDamageEvent()
        {
            TakeDamageEvent?.Invoke();
        }

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

            TakeDamageEvent?.Invoke();

            if (_currentDurability <= 0)
            {
                DestroyObject();
            }
        }

        public virtual void DestroyObject()
        {
            StartCoroutine(PlayDestroyAnimation());
            DestroyedEvent?.Invoke();
        }

        private IEnumerator PlayDestroyAnimation()
        {
            SetActiveFunctionalComponents(false);
            //transform.SetParent(null);
            //AddExplosionForce();
            
            yield return new WaitForSeconds(1);

            Destroy(gameObject, 3600);

            yield break;
        }

        private void SetActiveFunctionalComponents(bool value)
        {
            for (int i = 0; i < _functionalСomponents.Count; i++)
            {
                _functionalСomponents[i].enabled = value;
            }
        }

        private void AddExplosionForce()
        {
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();

            if (rigidbody)
            {
                rigidbody.AddExplosionForce(100f, transform.position, 10f);
            }
            else
            {
                rigidbody = gameObject.AddComponent<Rigidbody>();
                rigidbody.angularDrag = 0;
                rigidbody.useGravity = false;
                rigidbody.AddExplosionForce(100f, transform.position, 10f);
            }
        }
    }
}
