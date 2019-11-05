using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public abstract class AmmoItem : Item
    {
        [SerializeField] private GameObject _hitSparksPrefab;
        [SerializeField] private GameObject _hitHolePrefab;
        [SerializeField] private float _damage = 0;
        [SerializeField] private float _lifeTime = 0;

        public GameObject HitSparksPrefab => _hitSparksPrefab;
        public GameObject HitHolePrefab => _hitHolePrefab;
        public float Damage => _damage;
        public float LifeTime => _lifeTime;
    }
}
