using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace OverEarth
{
    public abstract class PanelUI<T> : Singleton<T> where T : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _panelGroup;

        private Sequence _animationSequence;

        private float _animationTime = 0.5f;

        protected override void Awake()
        {
            base.Awake();

            Init();
        }

        private void Init()
        {
            _panelGroup.alpha = 0;
            _panelGroup.interactable = false;
            _panelGroup.blocksRaycasts = false;
        }

        private protected void OpenPanel()
        {
            KillSequence();
            _animationSequence = DOTween.Sequence();

            _animationSequence.Insert(0, _panelGroup.DOFade(1, _animationTime).SetEase(Ease.Linear));
            _panelGroup.interactable = true;
            _panelGroup.blocksRaycasts = true;
        }

        private protected void ClosePanel()
        {
            KillSequence();
            _animationSequence = DOTween.Sequence();

            _animationSequence.Insert(0, _panelGroup.DOFade(0, _animationTime).SetEase(Ease.Linear));
            _panelGroup.interactable = false;
            _panelGroup.blocksRaycasts = false;
        }

        private void KillSequence()
        {
            if (_animationSequence != null)
            {
                _animationSequence.Kill();
            }
        }
    }
}
