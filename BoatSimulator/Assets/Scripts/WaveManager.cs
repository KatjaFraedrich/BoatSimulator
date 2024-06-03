using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Material waveMat;

    [SerializeField] private float speed = 0;
    [SerializeField] private float offSet = 0;
    [SerializeField] private float amplitude = 0;

    
    private void Update()
    {
        if (waveMat != null)
        {
            waveMat.SetFloat("_Speed",speed);
            waveMat.SetFloat("_OffSet", offSet);
            waveMat.SetFloat("_Amplitude", amplitude);
            waveMat.SetFloat("_WaveTime", Time.time);
        }

    }
    
        
    public float GetWaveHeightAt(float xPos,float yPos)
    {
        float waveHeight = Mathf.Sin(Time.time * speed + xPos * offSet) * amplitude;
        
        return waveHeight;
        
    }
}
