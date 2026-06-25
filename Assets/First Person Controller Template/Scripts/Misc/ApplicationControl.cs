using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationControl : MonoBehaviour
{
    public static ApplicationControl Instance;

    private void Awake() => Instance = this;

    public void SetCrouchMode(int set) => PlayerPrefs.SetInt("CrouchMode", set);

    public CrouchMode GetCrouchMode() => (CrouchMode)PlayerPrefs.GetInt("CrouchMode", 0);

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}