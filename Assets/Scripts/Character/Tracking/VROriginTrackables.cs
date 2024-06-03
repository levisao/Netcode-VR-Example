using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class VROriginTrackables : MonoBehaviour
{
    public static VROriginTrackables Singleton;

    public Transform XRRig;
    public Transform XRHead;
    public Transform XRLeftHand;
    public Transform XRRightHand;



    private void Start()
    {
        if (Singleton == null)
        {
            DontDestroyOnLoad(gameObject);
            Singleton = this;
        }
    }
}

