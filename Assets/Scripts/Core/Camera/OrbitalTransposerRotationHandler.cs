using UnityEngine;
using Cinemachine;
using System;

public class OrbitalTransposerRotationHandler : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera virtualCamera;
    CinemachineOrbitalTransposer transposer;

    [SerializeField]
    private float rotationSpeed;

    private bool selfRotated = false;
    private bool rotationDisabled = false;

    private enum RotationDirection
    {
        Clockwise,
        CounterClockwise
    }

    private RotationDirection rotationDirection;

    private void Awake()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        rotationDirection = RotationDirection.Clockwise;
    }

    private void RemoveAxis()
    {
        transposer.m_XAxis.m_InputAxisName = "";
    }

    private void SetMouseAxis()
    {
        transposer.m_XAxis.m_InputAxisName = "Mouse X";
    }

    public void SetRotationAngle(float yRotation)
    {
        if (yRotation <= 0)
        {
            yRotation = 360f;
        }

        if (yRotation >= 360f)
        {
            yRotation = 0f;
        }

        transposer.m_XAxis.Value = yRotation;
    }

    private float GetRotationValue()
    {
        return transposer.m_XAxis.Value;
    }

    public void SetRotatedByPlayer()
    {
        SetMouseAxis();

        selfRotated = false;
        rotationDisabled = false;
    }

    public void SetSelfRotated()
    {
        RemoveAxis();

        transposer.m_XAxis.m_InputAxisValue = 0f;
        selfRotated = true;
        rotationDisabled = false;
    }

    public void SetNoRotation()
    {
        rotationDisabled = true;
    }

    private void SetRotationDirection()
    {
        if (transposer.m_XAxis.m_InputAxisValue > 0)
        {
            rotationDirection = RotationDirection.CounterClockwise;
        }
        else if (transposer.m_XAxis.m_InputAxisValue < 0)
        {
            rotationDirection = RotationDirection.Clockwise;
        }
    }

    void Update()
    {
        if (!rotationDisabled)
        {
            SetRotationDirection();

            if (selfRotated)
            {
                float currentValue = GetRotationValue();

                int direction = rotationDirection == RotationDirection.Clockwise ? -1 : 1;
                SetRotationAngle(currentValue + (direction * rotationSpeed * Time.deltaTime));
            }
        }
    }
}
