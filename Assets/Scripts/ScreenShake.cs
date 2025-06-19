using System;
using Unity.Cinemachine;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
  public static ScreenShake Instance {get; private set;}
  
  private CinemachineImpulseSource cinemachineImpulseSource;
  

  private void Awake()
  {
    cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
    }
    else
    {
      Instance = this;
    }
    
  }

  public void Shake(float intensity = 1f)
  {
    cinemachineImpulseSource.GenerateImpulse(intensity);
  }
}
