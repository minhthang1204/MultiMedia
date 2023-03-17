using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main
{
    public class SceneController : MonoSingleton<SceneController>
    {
        #region Variables

        private Action onLoaderCallback;
        private AsyncOperation loadingAsyncOperation = null;
        [SerializeField] private TransitionScreen transitionScreen;
        public bool isFromCustomMap = false;
        #endregion Variables

        #region Data from scene

        public LevelData selectedLevelData;

        #endregion

        #region Singleton Methods

        protected override void InternalInit()
        {
        }

        protected override void InternalOnDestroy()
        {
        }

        protected override void InternalOnDisable()
        {
        }

        protected override void InternalOnEnable()
        {
        }

        #endregion Singleton Methods

        #region Methods

        public void Load(string scene, Action action = null)
        {
            StartCoroutine(LoadSceneAsync(scene,action));
        }

        private IEnumerator LoadSceneAsync(string scene, Action action = null)
        {
            // trigger the transition screen
            transitionScreen.Intro(() =>
            {
                action?.Invoke();
            });
            yield return new WaitForSecondsRealtime(transitionScreen.GetTweenTime()+1f);
            

            loadingAsyncOperation = SceneManager.LoadSceneAsync(scene);

            loadingAsyncOperation.allowSceneActivation = false;
            
            while (!loadingAsyncOperation.isDone)
            {
                if (loadingAsyncOperation.progress >= 0.9f)
                {
                    loadingAsyncOperation.allowSceneActivation = true;
                    transitionScreen.Outro();
                }
                
                yield return null;
            }
        }

        public float GetLoadingProgress()
        {
            if (loadingAsyncOperation != null)
            {
                return loadingAsyncOperation.progress;
            }
            else
            {
                return 1f;
            }
        }

        // TODO: copy from the internet ???
        public void SceneLoaderCallback()
        {
            if (onLoaderCallback != null)
            {
                onLoaderCallback();
                onLoaderCallback = null;
            }
        }

        #endregion Methods
    }
}