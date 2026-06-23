using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    private bool movementFoldout = true;
    private bool crouchFoldout = true;
    private bool jumpFoldout = true;
    private bool staminaFoldout = true;
    private bool featuresFoldout = true;
    private bool referencesFoldout = true;
    private bool collisionFoldout = true;
    private bool debugFoldout = true;
    private bool UseSprint => serializedObject.FindProperty("useSprint").boolValue;
    private bool UseStamina => serializedObject.FindProperty("useStamina").boolValue;
    private bool UseJump => serializedObject.FindProperty("useJump").boolValue;
    private bool UseCrouch => serializedObject.FindProperty("useCrouch").boolValue;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawMovement();
        DrawCrouch();
        DrawJumpGravity();
        DrawStamina();
        DrawFeatures();
        DrawReferences();
        DrawCollision();
        if (Application.isPlaying) DrawDebug();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawMovement()
    {
        movementFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(movementFoldout, "Movement");
        if (movementFoldout)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("walkSpeed"));
            if (UseSprint) EditorGUILayout.PropertyField(serializedObject.FindProperty("sprintSpeed"));
            if (UseCrouch) EditorGUILayout.PropertyField(serializedObject.FindProperty("crouchSpeed"));
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void DrawCrouch()
    {
        if (!UseCrouch) return;
        crouchFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(crouchFoldout, "Crouch");
        if (crouchFoldout)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("standingHeight"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("crouchHeight"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("standingEyeHeight"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("crouchEyeHeight"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("crouchTransitionSpeed"));
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void DrawJumpGravity()
    {
        jumpFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(jumpFoldout, "Jump & Gravity");
        if (jumpFoldout)
        {
            if (UseJump) EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpHeight"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("gravity"));
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void DrawStamina()
    {
        if (!UseSprint || !UseStamina) return;
        staminaFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(staminaFoldout, "Stamina");
        if (staminaFoldout)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxStamina"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("staminaDrainSpeed"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("staminaRegenSpeed"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("staminaCooldown"));
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void DrawFeatures()
    {
        featuresFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(featuresFoldout, "Features");
        if (featuresFoldout)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useSprint"));
            if (UseSprint) EditorGUILayout.PropertyField(serializedObject.FindProperty("useStamina"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useJump"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useCrouch"));
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void DrawReferences()
    {
        referencesFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(referencesFoldout, "References");
        if (referencesFoldout)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("cameraPivot"));
            if (UseSprint && UseStamina)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("staminaSlider"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("staminaCanvasGroup"));
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void DrawCollision()
    {
        collisionFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(collisionFoldout, "Collision");
        if (collisionFoldout) EditorGUILayout.PropertyField(serializedObject.FindProperty("obstacleMask"));
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void DrawDebug()
    {
        PlayerController player = (PlayerController)target;
        debugFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(debugFoldout, "Debug");
        if (debugFoldout)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Toggle("Is Crouching", player.IsCrouching);
            EditorGUILayout.FloatField("Speed 0-1", player.CurrentSpeed01);
            EditorGUILayout.Vector3Field("Velocity", player.Velocity);
            EditorGUI.EndDisabledGroup();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
}