using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public static PlayerLook Instance;
    [SerializeField] private Transform cameraPivot = null;
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private GameObject gameUI = null, pauseUI = null;
    private float pitch = 0;
    public bool paused = false;

#if UNITY_STANDALONE || UNITY_EDITOR
    private void Start() => Unpause();
#endif

    private void Awake() => Instance = this;

    private void Update()
    {
        if (!paused)
        {
            float mouseX = InputManager.GetAxis("LookX");
            float mouseY = InputManager.GetAxis("LookY");
            transform.Rotate(Vector3.up * mouseX * sensitivity);
            pitch -= mouseY * sensitivity;
            pitch = Mathf.Clamp(pitch, -90, 90);
            cameraPivot.localEulerAngles = new Vector3(pitch, 0, 0);
        }
        if (InputManager.GetButtonDown("Pause")) Pause();
    }

    public void Pause()
    {
        paused = true;
        gameUI.SetActive(false);
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
#if UNITY_STANDALONE || UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
#endif
    }

    public void Unpause()
    {
        paused = false;
        gameUI.SetActive(true);
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
#if UNITY_STANDALONE || UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
#endif
    }
}