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

            Transform playerCamera = PlayerController.Instance.Camera.transform;
            
            KillSequence(_cameraShakingAnimationSequence);
            _cameraShakingAnimationSequence = DOTween.Sequence();

            int randomNumber = Random.Range(0, 2);
            if (randomNumber == 0)
            {
                randomNumber = -1;
            }
            Vector3 shakeDirection = new Vector3(0, 0, randomNumber);
            _cameraShakingAnimationSequence.Insert(0, playerCamera.DOShakePosition(_cameraShakingAnimationTime, 0.01f, 1));
            _cameraShakingAnimationSequence.Insert(0, playerCamera.DOPunchRotation(shakeDirection, _cameraShakingAnimationTime)).OnComplete(() =>
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
