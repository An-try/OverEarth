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

        private void SubscribeEvents()
        {
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J) && !_warpJumpStarted)
            {
                print('a');
                _warpJumpStarted = true;
                StartCoroutine(PrepareToWarp());
            }
        }

        private IEnumerator PrepareToWarp()
        {
            print('b');
            SceneLoading = SceneManager.LoadSceneAsync("Game");
            SceneLoading.allowSceneActivation = false;

            PlayerController.Instance.Ship.PrepareToWarp();

            //yield return new WaitUntil(() => SceneLoading.isDone);
            yield return new WaitUntil(() => _sceneLoaded);
            print('c');
            yield return new WaitUntil(() => PlayerController.Instance.Ship.IsPrepareForWarpAnimationCompleted);
            print('d');

            PlayerController.Instance.Ship.DoWarp();

            yield return new WaitUntil(() => PlayerController.Instance.Ship.CanWarp);

            PlayerController.Instance.Ship.AnimateShipWarping();

            yield break;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            _sceneLoaded = true;
        }
    }
}
