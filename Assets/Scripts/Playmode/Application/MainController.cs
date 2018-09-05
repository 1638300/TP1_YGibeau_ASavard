using Playmode.Util.Values;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Playmode.Application
{
    public class MainController : MonoBehaviour
    {
        private void Start()
        {
            LoadGameScene();
        }

        private void LoadGameScene()
        {
            StartCoroutine(LoadGameSceneRoutine());
        }

        public void ReloadGameScene()
        {
            StartCoroutine(ReloadGameSceneRoutine());
        }

        private static IEnumerator LoadGameSceneRoutine()
        {
            if (!SceneManager.GetSceneByName(Scenes.GAME).isLoaded)
                yield return SceneManager.LoadSceneAsync(Scenes.GAME, LoadSceneMode.Additive);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(Scenes.GAME));
        }

        private static IEnumerator UnloadGameSceneRoutine()
        {
            if (SceneManager.GetSceneByName(Scenes.GAME).isLoaded)
                yield return SceneManager.UnloadSceneAsync(Scenes.GAME);
        }

        private static IEnumerator ReloadGameSceneRoutine()
        {
            yield return UnloadGameSceneRoutine();
            yield return LoadGameSceneRoutine();
        }
    }
}