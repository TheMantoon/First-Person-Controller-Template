using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationControl : MonoBehaviour
{
    public static ApplicationControl Instance;

    private void Awake() => Instance = this;

    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}