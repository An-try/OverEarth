using UnityEngine;

namespace OverEarth
{
    /// <summary>
    /// Base class for any ammo in the game.
    /// </summary>
    public abstract class Ammo : MonoBehaviour
    {
        [SerializeField] private protected AmmoItem _ammoItem;
        
        private protected GameObject _hitSparksPrefab; // Sparks when hit something
        private GameObject _hitHolePrefab; // Hit hole from the impact will be in the place of impact of the object
        private protected float _damage;

        private void Start()
        {
            _hitSparksPrefab = _ammoItem.HitSparksPrefab;
            _hitHolePrefab = _ammoItem.HitHolePrefab;
            _damage = _ammoItem.Damage;
        }

        private protected void DoDamage(float damage, Collider collider)
        {
            Damageable hittedObject = collider.GetComponent<Damageable>();
            if (hittedObject)
            {
                hittedObject.DoDamage(damage);
            }
            else
            {
                hittedObject = collider.GetComponentInParent<Damageable>();
                if (hittedObject)
                {
                    hittedObject.DoDamage(damage);
                }
            }
        }

        private protected void DoDamage(float damage, Collision collision)
        {
            Damageable hittedObject = collision.collider.GetComponent<Damageable>();
            if (hittedObject)
            {
                hittedObject.DoDamage(damage);
            }
            else
            {
                hittedObject = collision.collider.GetComponentInParent<Damageable>();
                if (hittedObject)
                {
                    hittedObject.DoDamage(damage);
                }
            }
        }
    }
}
