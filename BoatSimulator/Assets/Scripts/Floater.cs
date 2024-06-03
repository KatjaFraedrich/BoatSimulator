using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField] private BoatController boatController;
    


    // Physics Computations in fixed update to not be depended on framerate (called 50 times per second)
    private void FixedUpdate()
    {
        BuoyancySettings buoyancySettings = boatController.BuoyancySettings;
        
        Vector3 floaterPos = this.transform.position;
        boatController.Physicsbody.AddForceAtPosition(Physics.gravity/ buoyancySettings.FloaterCount, floaterPos, ForceMode.Acceleration); //Apply Gravity to each floater (even if not submerged)
        
        float waterHeight = boatController.WaveManager.GetWaveHeightAt(floaterPos.x, floaterPos.z);
        if (floaterPos.y < waterHeight) //floater is under water
        {
            float percentUnderWater = Mathf.Clamp01((waterHeight - floaterPos.y) / buoyancySettings.ObjectHeight);
            float displacementMultiplier = percentUnderWater * buoyancySettings.FloaterForceStrength;
            Vector3 upwardForce = new Vector3(0, Mathf.Abs(Physics.gravity.y) * displacementMultiplier , 0);
            
            //upwards force
            boatController.Physicsbody.AddForceAtPosition(upwardForce, floaterPos,ForceMode.Acceleration);

            //drag (smoothes the boat as the force is partly canceled out (hence the *-velocity))
            boatController.Physicsbody.AddForce(displacementMultiplier * -boatController.Physicsbody.velocity * buoyancySettings.WaterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            boatController.Physicsbody.AddTorque(displacementMultiplier * -boatController.Physicsbody.angularVelocity * buoyancySettings.WaterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);

        }
    }
}
