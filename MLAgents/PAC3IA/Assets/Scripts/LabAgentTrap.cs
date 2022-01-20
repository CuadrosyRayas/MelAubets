using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class LabAgentTrap : Agent
{
    Rigidbody rBody;
    // Start is called before the first frame update
    public Transform target, targetPosition1, targetPosition2, targetPosition3;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        float rand1 = Random.Range(0.1f, 1f);
        if (rand1 < 0.33f)
        {
            Instantiate(target, targetPosition1);
        }
        else if (rand1 < 0.66f)
        {
            Instantiate(target, targetPosition2);
        }
        else
        {
            Instantiate(target, targetPosition3);
        }

    }

    public override void OnEpisodeBegin()
    {
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        float rand = Random.Range(-10f, 20f);
        this.transform.localPosition = new Vector3(rand, 1.5f, -20f);


    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);

        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actions.ContinuousActions[0];
        controlSignal.z = actions.ContinuousActions[1];
        float forceMultiplier = actions.ContinuousActions[2];
        if(forceMultiplier == 0 || controlSignal == Vector3.zero)
        {
            EndEpisode();
        }
        rBody.AddForce(controlSignal * forceMultiplier * 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Target")
        {
            SetReward(+1f);
            Destroy(other.gameObject);
            float rand3 = Random.Range(0.1f, 1f);
            if (rand3 < 0.33f)
            {
                Instantiate(target, targetPosition1);
            }
            else if (rand3 < 0.66f)
            {
                Instantiate(target, targetPosition2);
            }
            else
            {
                Instantiate(target, targetPosition3);
            }
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Trap")
        {
            SetReward(-1f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

}
