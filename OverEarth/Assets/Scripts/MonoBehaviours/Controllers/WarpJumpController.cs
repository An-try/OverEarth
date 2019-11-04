using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OverEarth
{
    public class WarpJumpController : Singleton<WarpJumpController>
    {
        public AsyncOperation SceneLoading { get; private set; }

        private bool _warpJumpStarted = false;
        private bool _sceneLoaded = false;

        private List<string> _scenesNames = new List<string>();
        private int _nextSceneIndex = 1;

        private protected override void Awake()
        {
            base.Awake();

            _scenesNames.Add("BaseDefence");
            _scenesNames.Add("Game");
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            SceneManager.sceneLoaded -= SceneLoaded;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J) && !_warpJumpStarted)
            {
                _warpJumpStarted = true;
                StartCoroutine(PrepareToWarp());
            }
        }

        private IEnumerator PrepareToWarp()
        {
            SceneLoading = SceneManager.LoadSceneAsync(_scenesNames[_nextSceneIndex]);
            SceneLoading.allowSceneActivation = false;

            _nextSceneIndex++;
            if (_nextSceneIndex >= _scenesNames.Count)
            {
                _nextSceneIndex = 0;
            }

            PlayerController.Instance.Ship.PrepareToWarp(); print("Preparation for warp started");

            yield return new WaitUntil(() => _sceneLoaded); print("Scene loaded");
            yield return new WaitUntil(() => PlayerController.Instance.Ship.IsPrepareForWarpAnimationCompleted); print("Preparation for warp completed");

            PlayerController.Instance.Ship.DoWarp();

            yield return new WaitUntil(() => PlayerController.Instance.Ship.CanWarp); print("Ship warped");
            SceneLoading.allowSceneActivation = true;

            PlayerController.Instance.Ship.AnimateShipWarping();

            yield return new WaitUntil(() => PlayerController.Instance.Ship.IsWarpEnded);

            //_sceneLoaded = false;
            _warpJumpStarted = false;
            SceneLoading = null;

            StopAllCoroutines();

            yield break;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            _sceneLoaded = true;
        }
    }
}
