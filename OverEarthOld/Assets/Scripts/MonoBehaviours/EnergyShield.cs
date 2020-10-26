using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class EnergyShield : Damageable
    {
        private Material _material;
        private Coroutine _hitAnimation;

        private float _animationTime = 0.04f;
        private bool _canAnimate = true;

        private protected override void Start()
        {
            base.Start();

            _material = GetComponent<MeshRenderer>().material;
            _canShakeCamera = false;
        }

        public override void DoDamage(float damage)
        {
            base.DoDamage(damage);

            if (gameObject.activeSelf && _canAnimate)
            {
                _canAnimate = false;
                KillCoroutine();
                _hitAnimation = StartCoroutine(HitAnimation());
            }
        }

        private IEnumerator HitAnimation()
        {
            float time = _animationTime;
            float amount = 1;

            while (time >= 0)
            {
                amount += 2 / _animationTime * Time.fixedDeltaTime;
                _material.SetFloat("_Fresnel", amount * 100);
                _material.SetFloat("_FresnelWidth", amount);
                time -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            time = _animationTime;

            while (time >= 0)
            {
                amount -= 2 / _animationTime * Time.fixedDeltaTime;
                _material.SetFloat("_Fresnel", amount * 100);
                _material.SetFloat("_FresnelWidth", amount);
                time -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            _canAnimate = true;

            yield break;
        }

        private void KillCoroutine()
        {
            if (_hitAnimation != null)
            {
                StopCoroutine(_hitAnimation);
                _hitAnimation = null;
            }
        }
    }
}
