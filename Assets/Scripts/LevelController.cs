using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private BallsHolder _ballsHolder;

    private void OnEnable()
    {
        _ballsHolder.BallsEnded += ReloadLevel;
    }

    private void OnDisable()
    {
        _ballsHolder.BallsEnded -= ReloadLevel;
    }

    private void ReloadLevel()
    {
        SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single);
    }
}
