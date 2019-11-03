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
            SceneLoading = SceneManager.LoadSceneAsync("Game");
            SceneLoading.allowSceneActivation = false;

            PlayerController.Instance.Ship.PrepareToWarp();

            yield return new WaitUntil(() => SceneLoading.isDone);
            yield return new WaitUntil(() => PlayerController.Instance.Ship.IsPrepareForWarpAnimationCompleted);

            PlayerController.Instance.Ship.DoWarp();

            yield return new WaitUntil(() => PlayerController.Instance.Ship.CanWarp);

            yield break;
        }
    }
}
