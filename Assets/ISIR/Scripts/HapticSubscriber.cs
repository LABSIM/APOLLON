using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityRoboticsDemo;

public class HapticSubscriber 
    : MonoBehaviour
{
    private ROSConnection ROS_connection = null;
    public string ROS_topicName = "ISIR_to_ONERA_Upstream";

    private void Start()
    {

        // start the ROS connection
        this.ROS_connection = ROSConnection.GetOrCreateInstance();
        this.ROS_connection.Subscribe<PosRotMsg>(ROS_topicName, this.HandleUpstreamData);
            
    } /* Start() */

    private void HandleUpstreamData(PosRotMsg msg)
    {

        this.transform.localPosition 
            = new(
                msg.pos_x,
                msg.pos_y,
                msg.pos_z
            );
        this.transform.localRotation
            = new(
                msg.rot_x,
                msg.rot_y,
                msg.rot_z,
                msg.rot_w
            );

    } /* HandleUpstreamData() */ 

} /* HapticSubscriber() */
