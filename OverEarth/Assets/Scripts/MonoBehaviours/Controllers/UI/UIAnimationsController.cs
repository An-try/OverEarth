using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace OverEarth
{
    public class UIAnimationsController : Singleton<UIAnimationsController>
    {
        [SerializeField] private CanvasGroup _takeDamageUIPanelGroup;

        private Sequence _cameraShakingAnimationSequence;
        private Sequence _UIAnimationSequence;

        private float _cameraShakingAnimationTime = 0.1f;
        private float _UIanimationTime = 0.025f;

        private bool _isCameraShakingComplete = true;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _takeDamageUIPanelGroup.alpha = 0;
        }

        public void TakeDamage()
        {
            CameraShaking();
            TakeDamageUIAnimation();
        }

        private void CameraShaking()
        {
            if (!_isCameraShakingComplete)
            {
                return;
            }
            _isCameraShakingComplete = false;

            Transform playerCamera = CameraController.Instance.Camera.transform;
            
            KillSequence(_cameraShakingAnimationSequence);
            _cameraShakingAnimationSequence = DOTween.Sequence();

            _cameraShakingAnimationSequence.Insert(0, playerCamera.DOShakePosition(_cameraShakingAnimationTime));
            _cameraShakingAnimationSequence.Insert(0, playerCamera.DOPunchRotation(Vector3.forward, _cameraShakingAnimationTime)).OnComplete(() =>
            {
                _isCameraShakingComplete = true;
            });
        }

        private void TakeDamageUIAnimation()
        {
            KillSequence(_UIAnimationSequence);
            _UIAnimationSequence = DOTween.Sequence();

            _UIAnimationSequence.Insert(0, _takeDamageUIPanelGroup.DOFade(0.9f, _UIanimationTime)).OnComplete(() =>
            {
                _UIAnimationSequence.Insert(0, _takeDamageUIPanelGroup.DOFade(0, _UIanimationTime));
            });
        }

        private void KillSequence(Sequence sequence)
        {
            if (sequence != null)
            {
                sequence.Kill();
            }
        }
    }
}
