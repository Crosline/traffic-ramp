using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Utilities
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private Image _logoImage;
        [SerializeField] private string _levelName;
        [SerializeField] private float _delaySeconds;

        private void Start()
        {
            StartCoroutine(StartLevelRoutine());
        }

        private IEnumerator StartLevelRoutine()
        {
            var loadSceneOp = SceneManager.LoadSceneAsync(_levelName);
            loadSceneOp.allowSceneActivation = false;

            yield return new WaitForSeconds(_delaySeconds);

            if (loadSceneOp.isDone || loadSceneOp.progress >= 0.9f)
            {
                loadSceneOp.allowSceneActivation = true;
            }
        }
    }
}