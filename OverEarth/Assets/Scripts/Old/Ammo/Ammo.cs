using UnityEngine;

namespace OverEarth
{
    /// <summary>
    /// Base class for any ammo in the game.
    /// </summary>
    public abstract class Ammo : MonoBehaviour
    {
        private protected float _kineticDamage;

        public GameObject SparksPrefab; // Sparks when hit something

        // TODO: Make an hit hole effect
        public GameObject HitHolePrefab; // Hit hole from the impact will be in the place of impact of the object

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
            Damageable hittedObject = collision.transform.GetComponent<Damageable>();

            if (hittedObject)
            {
                hittedObject.DoDamage(damage);
            }
            else
            {
                hittedObject = collision.transform.GetComponentInParent<Damageable>();
                if (hittedObject)
                {
                    hittedObject.DoDamage(damage);
                }
            }
        }
    }
}
