using UnityEngine;

public class MobileUI : MonoBehaviour
{
    [SerializeField] private GameObject mobileUI = null;

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
    private void Awake() => mobileUI.SetActive(true);
#else
    private void Awake() => mobileUI.SetActive(false);
#endif
}