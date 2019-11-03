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
        [Header("Parts to display")]
        [Tooltip("Select one or several parts")]
        [SerializeField] private List<DamageablePartTypes> _damageablePartTypes;

        public List<Damageable> DamageableParts { get; private set; } = new List<Damageable>();

        private Image _partImage;

        private void Awake()
        {
            _partImage = GetComponent<Image>();
        }

        //private void OnEnable()
        //{
        //    SubscribeEvents();
        //}

        private void SubscribeEvents()
        {
            for (int i = 0; i < DamageableParts.Count; i++)
            {
                if (DamageableParts[i])
                {
                    DamageableParts[i].TakeDamageEvent += UpdateImageColor;
                    DamageableParts[i].DestroyedEvent += SetDestroyedImageColor;
                }
            }
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            for (int i = 0; i < DamageableParts.Count; i++)
            {
                DamageableParts[i].TakeDamageEvent -= UpdateImageColor;
                DamageableParts[i].DestroyedEvent -= SetDestroyedImageColor;
            }
        }

        public void AssignDamageablePart(Ship ship)
        {
            for (int i = 0; i < ship.DamageableParts.Count; i++)
            {
                for (int j = 0; j < _damageablePartTypes.Count; j++)
                {
                    Damageable damageablePart = ship.DamageableParts.Find(shipPart => shipPart.DamageablePartType.Equals(_damageablePartTypes[j]));

                    if (damageablePart && !DamageableParts.Contains(damageablePart))
                    {
                        DamageableParts.Add(damageablePart);
                    }
                }
            }

            SubscribeEvents();
        }

        private void UpdateImageColor()
        {
            float averageCurrentDurability = 0;
            float averageMaxDurability = 0;
            for (int i = 0; i < DamageableParts.Count; i++)
            {
                //if (_damageableParts[i] == null)
                //{
                //    _damageableParts.RemoveAt(i);
                //    i--;
                //    continue;
                //}
                averageCurrentDurability += DamageableParts[i].CurrentDurability;
                averageMaxDurability += DamageableParts[i].MaxDurability;
            }
            averageCurrentDurability /= DamageableParts.Count;
            averageMaxDurability /= DamageableParts.Count;

            float r = 255 - (averageCurrentDurability / averageMaxDurability * 255);
            float g = averageCurrentDurability / averageMaxDurability * 255;

            byte R = (byte)Mathf.Clamp(r, 0, 255);
            byte G = (byte)Mathf.Clamp(g, 0, 255);

            _partImage.color = new Color32(R, G, 0, 255);
        }

        private void SetDestroyedImageColor(Damageable damageable)
        {
            _partImage.color = Color.black;
        }
    }
}
