using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTrigger : MonoBehaviour
{

    public Vector3 newCamPos, newPlayerPos;
    CamController camControl;

    // Start is called before the first frame update
    void Start()
    {
        camControl = Camera.main.GetComponent<CamController>();
    }

    public void CameraTransition()
    {
            camControl.minPos += newCamPos;
            camControl.maxPos += newCamPos;

    }
}
