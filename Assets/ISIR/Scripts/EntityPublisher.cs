using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityRoboticsDemo;

public class EntityPublisher 
    : MonoBehaviour
{

    private ROSConnection ROS_connection = null;
    public string ROS_topicName = "ONERA_to_ISIR_Downstream";
    public float ROS_publishMessageFrequency = 0.5f;
    private float ROS_timeElapsed = 0.0f;

    public float anim_updatePeriod = 2;
    private float anim_elapsedTime = 0.0f;
    private Vector3 
        anim_translationObjective = Vector3.zero,
        anim_translationInitial   = Vector3.zero;
    private Quaternion 
        anim_rotationObjective = Quaternion.identity,
        anim_rotationInitial   = Quaternion.identity;
    
    private void Start()
    {

        this.anim_translationInitial   = this.transform.localPosition;
        this.anim_rotationInitial      = this.transform.localRotation;
        this.anim_translationObjective = Random.insideUnitSphere * 5.0f;
        this.anim_rotationObjective    = Random.rotation;

        // start the ROS connection
        this.ROS_connection = ROSConnection.GetOrCreateInstance();
        this.ROS_connection.RegisterPublisher<PosRotMsg>(ROS_topicName);
            
    } /* Start() */


    private void Update()
    {

        // Animating 

        this.anim_elapsedTime += Time.deltaTime;
        if (this.anim_elapsedTime > this.anim_updatePeriod)
        {

            this.anim_translationInitial   = this.transform.localPosition;
            this.anim_rotationInitial      = this.transform.localRotation;
            this.anim_translationObjective = Random.insideUnitSphere * 5.0f;
            this.anim_rotationObjective    = Random.rotation;
            this.anim_elapsedTime          = 0.0f;

        } /* if() */

        this.transform.localPosition 
            = Vector3.Slerp(
                this.anim_translationInitial, 
                this.anim_translationObjective, 
                Mathf.Clamp01(this.anim_elapsedTime / this.anim_updatePeriod)
            );
        this.transform.localRotation 
            = Quaternion.Slerp(
                this.anim_rotationInitial, 
                this.anim_rotationObjective, 
                Mathf.Clamp01(this.anim_elapsedTime / this.anim_updatePeriod)
            );

        // ROS publishing
        ROS_timeElapsed += Time.deltaTime;

        if (ROS_timeElapsed > ROS_publishMessageFrequency)
        {

            PosRotMsg cubePos = new PosRotMsg(
                this.transform.position.x,
                this.transform.position.y,
                this.transform.position.z,
                this.transform.rotation.x,
                this.transform.rotation.y,
                this.transform.rotation.z,
                this.transform.rotation.w
            );

            // Finally send the message to server_endpoint.py running in ROS
            ROS_connection.Publish(ROS_topicName, cubePos);

            ROS_timeElapsed = 0;

        } /* if() */

    } /* Update() */

} /* class EntityPublisher */
