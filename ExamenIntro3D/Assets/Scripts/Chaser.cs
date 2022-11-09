using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Chaser : MonoBehaviour
{
    public Transform[] Goals;
    public float MovementSpeed = 1.0f;
    public float AcceptanceRadius = 1.0f;
    public float RotationSpeed = 30.0f;

    private int CurrentGoalIdx = 0;
    private float MovementSpeedModifier = 1.0f;

    void Update()
    {
        if (CurrentGoalIdx < 0 || Goals.Length <= CurrentGoalIdx)
        {
            return;
        }
        Transform CurrentGoal = Goals[CurrentGoalIdx];

        Vector3 goalPositionAtSelfHeight = new Vector3(CurrentGoal.position.x, transform.position.y, CurrentGoal.position.z);
        if (Vector3.Distance(transform.position, goalPositionAtSelfHeight) > AcceptanceRadius)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(goalPositionAtSelfHeight - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * RotationSpeed);
            transform.Translate(Vector3.forward * MovementSpeed * MovementSpeedModifier * Time.deltaTime);
        }
        else
        {
            CurrentGoalIdx = (CurrentGoalIdx + 1) % Goals.Length;
        }
    }

    public void ApplyMovementSpeedModifier(float modifier)
    {
        MovementSpeedModifier *= modifier;
    }
}
