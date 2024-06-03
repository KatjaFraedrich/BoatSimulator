using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BoatController : MonoBehaviour
{
    
    [SerializeField] private Rigidbody physicsbody;
    [SerializeField] private WaveManager waveManager;

    [SerializeField] private Transform wheelTransform;
    [SerializeField] private Transform speedLeverTransform;

    [SerializeField] private TMP_Text motorText;

    [Tooltip("Settings controlling how the boat behaves on the waves.")]
    [SerializeField] private BuoyancySettings buoyancySettings;

    [Tooltip("Movement speed multiplier for the boats motor forward force.")]
    [SerializeField] private float boatSpeed = 1;
    [Tooltip("Steering sensitivity of the steering wheel, higher values will make the boat turn faster.")]
    [SerializeField] private float steeringSensitivity = 1;

    public BuoyancySettings BuoyancySettings => buoyancySettings;
    public Rigidbody Physicsbody => physicsbody;
    public WaveManager WaveManager => waveManager;


    private float lastWheelZRot=0;
    private bool motorOn = true;


    private void FixedUpdate()
    {
        RecenterWavePlane();
        ApplyForwardMovement();
        OnSteeringWheel();
    }

    public void OnSteeringWheel()
    {
        float currentEulerAngles = wheelTransform.localEulerAngles.z;
        float deltaRot;
        if (Mathf.Abs(currentEulerAngles - lastWheelZRot) < (360 - Mathf.Max(currentEulerAngles, lastWheelZRot)) + Mathf.Min(currentEulerAngles, lastWheelZRot))
        {
            deltaRot = currentEulerAngles - lastWheelZRot;
        }
        else //360 skip (e.g. for angles 350° and 10° difference is only 20°)
        {
            if (lastWheelZRot > currentEulerAngles)//positive rotation
            {
                deltaRot = 360 - lastWheelZRot + currentEulerAngles;
            }
            else
            {
                deltaRot = -(360 - currentEulerAngles + lastWheelZRot);
            }
        }
        lastWheelZRot = currentEulerAngles;
        SteerBoat(deltaRot);
    }

    private void SteerBoat(float angle)
    {
        // Calculate the torque to apply
        float torque = angle * steeringSensitivity;
        // Apply torque to the boat
        physicsbody.AddTorque(Vector3.up * torque);
    }

    private void RecenterWavePlane()
    {
        //as the boat drives forward, move the wave shader to the boats center to make sure the boat never reaches the edge
        //(this works due to shader computations using world position of vertices, therefore even if the mesh is moved it does not look like it has moved)
        waveManager.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
    private void ApplyForwardMovement()
    {
        //speedLeverTransform.localEulerAngles = new Vector3(0, 0, Mathf.Clamp(speedLeverTransform.localEulerAngles.z, 0f, 120f));
        float leverAngle = speedLeverTransform.localEulerAngles.z-20;
        SetMotorStatus(leverAngle);

        if (motorOn)
        {
            physicsbody.AddForce(transform.forward * boatSpeed*leverAngle);//
        }
    }
    private void SetMotorStatus(float leverAngle)
    {
        motorOn = Mathf.Abs(leverAngle) > 20;
        if (motorOn)
        {
            motorText.text = "ON";
            motorText.color = new Color(0, 1, 0);
        }
        else
        {
            motorText.text = "OFF";
            motorText.color = new Color(1, 0, 0);
        }
    }
}
