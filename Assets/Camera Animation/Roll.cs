using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Roll : MonoBehaviour
{
    public CinemachineFreeLook camera;
    public float speed;

    void Update()
    {
        camera.m_XAxis.Value += speed * Time.deltaTime;
    }
}
