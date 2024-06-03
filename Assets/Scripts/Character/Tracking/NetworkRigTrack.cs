using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkRigTrack : NetworkBehaviour
{

    [SerializeField] private Transform NetXRRig;
    [SerializeField] private Transform NetXRHead;
    [SerializeField] private Transform NetXRLeftHand;
    [SerializeField] private Transform NetXRRightHand;

    public override void OnNetworkSpawn()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("posiçcao xrorgin: " + VROriginTrackables.Singleton.XRRig.position);
        if (!IsOwner) return;

        NetXRRig.transform.position = VROriginTrackables.Singleton.XRRig.position;
        NetXRRig.transform.rotation = VROriginTrackables.Singleton.XRRig.rotation;

        NetXRHead.transform.position = VROriginTrackables.Singleton.XRHead.position;
        NetXRHead.transform.rotation = VROriginTrackables.Singleton.XRHead.rotation;

        NetXRLeftHand.transform.position = VROriginTrackables.Singleton.XRLeftHand.position;
        NetXRLeftHand.transform.rotation = VROriginTrackables.Singleton.XRLeftHand.rotation;

        NetXRRightHand.transform.position = VROriginTrackables.Singleton.XRRightHand.position;
        NetXRRightHand.transform.rotation = VROriginTrackables.Singleton.XRRightHand.rotation;
    }
}
