using System;
using UnityEngine;

[Serializable]
public class BobTarget
{
    public Transform transform;
    public float multiplier = 1f;
}

public class CameraBob : MonoBehaviour
{
    [SerializeField] private BobTarget[] bobCameras = null;
    [SerializeField] private PlayerController player = null;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobAmount = 0.1f;
    [SerializeField] private float crouchBobAmount = 0.03f;
    [SerializeField] private float minSpeedToBob = 0.05f;
    private float timer = 0;
    private Vector3[] startPositions;
    private bool footstepTriggered = false;
    [SerializeField] private FootstepSystem footstepSystem = null;

    private void Start()
    {
        startPositions = new Vector3[bobCameras.Length];
        for (int i = 0; i < bobCameras.Length; i++) startPositions[i] = bobCameras[i].transform.localPosition;
    }

    private void Update()
    {
        float speed01 = player.CurrentSpeed01;
        if (speed01 < minSpeedToBob)
        {
            timer = 0;
            for (int i = 0; i < bobCameras.Length; i++)
            {
                bobCameras[i].transform.localPosition = Vector3.Lerp(bobCameras[i].transform.localPosition,
                    startPositions[i], Time.deltaTime * 10f);
            }
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
        float footstepPhase = Mathf.Sin(timer * 2f);
        if (player.OnGround && footstepPhase < -0.95f)
        {
            if (!footstepTriggered)
            {
                footstepTriggered = true;
                footstepSystem.PlayFootstep();
            }
        }
        else
        {
            footstepTriggered = false;
        }
        float bobX = Mathf.Cos(timer) * bobAmount;
        float bobY = Mathf.Sin(timer * 2f) * bobAmount;
        for (int i = 0; i < bobCameras.Length; i++)
        {
            Vector3 target = startPositions[i] + new Vector3(bobX * bobCameras[i].multiplier, bobY * bobCameras[i].multiplier, 0);
            bobCameras[i].transform.localPosition = Vector3.Lerp(bobCameras[i].transform.localPosition, target, Time.deltaTime * 15f);
        }
    }
}