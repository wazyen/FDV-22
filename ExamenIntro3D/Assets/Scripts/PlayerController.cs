using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
  public float MovementSpeed = 3.0f;
  public float RotationSpeed = 135.0f;
  
  public float SprintMultiplier = 2.0f;
  
  private float MovementSpeedModifier = 1.0f;

  void Update()
  {
    float currentMovementSpeed = MovementSpeed * MovementSpeedModifier;
    Vector3 movementDirection = Input.GetAxis("Vertical") * Vector3.forward;
    Vector3 rotationDirection = Input.GetAxis("Horizontal") * Vector3.up;
    transform.Translate(movementDirection * currentMovementSpeed * Time.deltaTime);
    transform.Rotate( rotationDirection * RotationSpeed * Time.deltaTime);
  }

  public void ToggleSprint(Toggle toggle)
  {
    if (toggle.isOn)
    {
      ApplyMovementSpeedModifier(SprintMultiplier);
    }
    else
    {
      ApplyMovementSpeedModifier(1.0f / SprintMultiplier);
    }
  }

  public void ApplyMovementSpeedModifier(float modifier)
  {
    MovementSpeedModifier *= modifier;
  }
}
