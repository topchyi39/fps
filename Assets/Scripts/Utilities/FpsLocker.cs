using System;
using UnityEngine;

public class FpsLocker : MonoBehaviour
{
    [SerializeField] private int targetFrameRate;
    
    
    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}