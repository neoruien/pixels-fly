using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.SceneManagement;

public class PlayerControllerAI : PlayerController
{
    private Events events;

    public override void Initialize()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        events = GameObject.Find("Events").GetComponent<Events>();
    }

    public override void CollectObservations(VectorSensor sensor) // observing the env
    {
        // Agent and Obstacle positions
        sensor.AddObservation(transform.position);
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            if (obs.name == "sideSignboard") sensor.AddObservation(obs.transform.position);
            else if (obs.name == "Mrt") sensor.AddObservation(obs.transform.position);
            else if (obs.name == "Train tracks") sensor.AddObservation(obs.transform.position);
            else if (obs.name == "centerTrafficlight") sensor.AddObservation(obs.transform);
            else if (obs.name == "centerRailing") sensor.AddObservation(obs.transform);
            else if (obs.name == "overheadSignboard") sensor.AddObservation(obs.transform);
            else if (obs.name == "car") sensor.AddObservation(obs.transform);
            else if (obs.name == "centerSignboard1") sensor.AddObservation(obs.transform);
            else if (obs.name == "carTaxi") sensor.AddObservation(obs.transform);
        }

        // Coin positions
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Coin")) sensor.AddObservation(obs.transform.position);

        // Agent velocity
        sensor.AddObservation(forwardSpeed);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if (Mathf.FloorToInt(vectorAction[0]) == 1 && transform.position.y < maxHeight - 1f)
        {
            AddReward(-10f);
            Jump();
        }

        if (Mathf.FloorToInt(vectorAction[1]) == 2 && desiredLane < 2)
        {
            moveRight();
            AddReward(-3f);
        }
        else if (Mathf.FloorToInt(vectorAction[1]) == 0 && desiredLane > 0)
        {
            moveLeft();
            AddReward(-3f);
        }
    }

    public override void OnEpisodeBegin()
    {
        print("restarted");
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) &&
            transform.position.y < maxHeight) actionsOut[0] = 1f; // can only jump with y < maxHeight

        // gather the inputs on which lane we should be
        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && desiredLane < 2) actionsOut[1] = 2f;
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && desiredLane > 0) actionsOut[1] = 0f;
    }

    protected new void Update()
    {
        if (transform.position.y >= maxHeight) maxHeightPrompt.SetActive(true);
        else maxHeightPrompt.SetActive(false);

        direction.z = forwardSpeed;
        if (forwardSpeed < maxSpeed) forwardSpeed += acceleration;

        if (!controller.isGrounded)
        {
            direction.y += gravity * Time.deltaTime;
            if (wasOnGround) walkToFly();
        }
        else if (controller.isGrounded)
        {
            wasOnGround = true;
            transform.GetChild(0).GetComponent<Animation>().Play("Bird Walk");
        }

        // calculate where we should be in the future
        Vector3 targetPosition = transform.position.z * transform.forward +
                                 transform.position.y * transform.up;
        if (desiredLane == 0) targetPosition += Vector3.left * tm.laneDistance;
        else if (desiredLane == 2) targetPosition += Vector3.right * tm.laneDistance;

        // transform.position = Vector3.Lerp(transform.position, targetPosition, 500 * Time.deltaTime);
        if (transform.position == targetPosition) return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude) controller.Move(moveDir);
        else controller.Move(diff);
    }

    protected new void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            AddReward(-10000f);
            Debug.Log(GetCumulativeReward());
            EndEpisode();
            SceneManager.LoadScene("Level (Training)");
            // forwardSpeed = defaultForwardSpeed;
        }
    }
}
