using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BuoyancySettings
{
    [Tooltip("Affects how high on the watersurface the boat sits; the higher the value the deeper the boat sinks.")]
    [SerializeField] private float objectHeight;
    [Tooltip("Affects the strength of the floaters; lower values will have the boat partially sink or get submerged, also indirectly affects how high boat sits on the water.")]
    [SerializeField] private float floaterForceStrength;
    [Tooltip("Affects how much the forces on the boat are smoothed/cancelded out. Low values might lead to jittering of the boat/jumpyness on the water surface.")]
    [SerializeField] private float waterDrag;
    [Tooltip("Affects how much the angular forces on the boat are smoothed/cancelded out. Low values might lead to boat rotating with the waves on the water surface.")]
    [SerializeField] private float waterAngularDrag;
    [Tooltip("Number of floaters attached to the boat. Used to divide the gravity on each floater so total gravity of the boat stays the same.")]
    [SerializeField] private int floaterCount;

    // Add more fields as necessary

    // Public getters for the fields
    public float ObjectHeight => objectHeight;
    public float FloaterForceStrength => floaterForceStrength;
    public float WaterDrag => waterDrag;
    public float WaterAngularDrag => waterAngularDrag;
    public int FloaterCount => floaterCount;
}
