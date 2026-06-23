using UnityEngine;

public class CameraBob : MonoBehaviour
{
    [SerializeField] private PlayerController player = null;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobAmount = 0.1f;
    [SerializeField] private float crouchBobAmount = 0.03f;
    [SerializeField] private float minSpeedToBob = 0.05f;
    private float timer;
    private Vector3 startLocalPos;

    private void Start() => startLocalPos = transform.localPosition;

    private void Update()
    {
        float speed01 = player.CurrentSpeed01;

        if (speed01 < minSpeedToBob)
        {
            timer = 0;
            transform.localPosition = Vector3.Lerp(transform.localPosition, startLocalPos, Time.deltaTime * 10f);
            return;
        }
        float bobSpeed = player.walkSpeed;
        float bobAmount = walkBobAmount;
        bobSpeed = Mathf.Lerp(player.walkSpeed, player.sprintSpeed, speed01);
        bobAmount = Mathf.Lerp(walkBobAmount, sprintBobAmount, speed01);
        if (player.IsCrouching)
        {
            bobSpeed = player.crouchSpeed;
            bobAmount = crouchBobAmount;
        }
        timer += Time.deltaTime * bobSpeed;
        float bobX = Mathf.Cos(timer) * bobAmount;
        float bobY = Mathf.Sin(timer * 2f) * bobAmount;
        Vector3 target = startLocalPos + new Vector3(bobX, bobY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * 15f);
    }
}