using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OverEarth
{
    public class DamageableUI : MonoBehaviour
    {
        [SerializeField] private DamageablePartTypes _damageablePartType;

        private Damageable _damageablePart;
        private Image _partImage;

        private void Awake()
        {
            _partImage = GetComponent<Image>();
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _damageablePart.TakeDamageEvent += UpdateImageColor;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            _damageablePart.TakeDamageEvent -= UpdateImageColor;
        }

        public void AssignDamageablePart(Damageable damageable)
        {
            _damageablePart = damageable.DamageableParts.Find(shipPart => shipPart.DamageablePartType.Equals(_damageablePartType));
        }

        private void UpdateImageColor(float _maxDurability, float _currentDurability)
        {
            float r = 255 - (_currentDurability / _maxDurability * 255);
            float g = _currentDurability / _maxDurability * 255;

            byte R = (byte)Mathf.Clamp(r, 0, 255);
            byte G = (byte)Mathf.Clamp(g, 0, 255);

            _partImage.color = new Color32(R, G, 0, 255);
        }
    }
}
