using UnityEngine;
using UnityEngine.UI;

public class SetDropdownCrouchMode : MonoBehaviour
{
    private void Start() => gameObject.GetComponent<Dropdown>().SetValueWithoutNotify((int)ApplicationControl.Instance.GetCrouchMode());
}