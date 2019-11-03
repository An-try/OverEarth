using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class WarpAnimation : MonoBehaviour
    {
        //private ParticleSystem _particleSystem;
        //private ParticleSystemRenderer _particleSystemRenderer;
        private Animator _animator;

        //private const float INITIAL_PARTICLES_SPEED = 0;
        //private const float INITIAL_PARTICLES_LENGTH_SCALE = 15;

        private const string _prepareToWarpTriggerName = "PrepareToWarp";
        private const string _doWarpTriggerName = "DoWarp";

        public bool IsPrepareForWarpAnimationCompleted => _animator.GetCurrentAnimatorStateInfo(0).IsName(_prepareToWarpTriggerName) &&
                                                          _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f;
        public bool CanWarp => _animator.GetCurrentAnimatorStateInfo(0).IsName(_doWarpTriggerName) &&
                               _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f;

        //private float _particlesSpeed
        //{
        //    get => _particleSystem.main.startSpeed.constant;
        //    set
        //    {
        //        var main = _particleSystem.main;
        //        main.startSpeed = value;
        //    }
        //}

        //private float _particlesLengthScale
        //{
        //    get => _particleSystemRenderer.lengthScale;
        //    set => _particleSystemRenderer.lengthScale = value;
        //}

        private void Awake()
        {
            //_particleSystem = GetComponent<ParticleSystem>();
            //_particleSystemRenderer = GetComponent<ParticleSystemRenderer>();
            _animator = GetComponent<Animator>();
        }

        //private void Start()
        //{
        //    Init();
        //}

        //private void Init()
        //{
        //    _particlesSpeed = INITIAL_PARTICLES_SPEED;
        //    _particlesLengthScale = INITIAL_PARTICLES_LENGTH_SCALE;

        //    _animator.SetTrigger(0);
        //}

        public void PrepareToWarp()
        {
            _animator.SetTrigger(_prepareToWarpTriggerName);
        }

        public void DoWarp()
        {
            _animator.SetTrigger(_doWarpTriggerName);
        }
    }
}
