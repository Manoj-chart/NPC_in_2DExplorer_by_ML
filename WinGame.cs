using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Gamekit2D;

public class WinGame : Agent
{
    [SerializeField] private Transform targetTransform;
    Rigidbody2D rigidbody;
    PlayerCharacter playerCharacter;
    GameObject chomperGameobject;
    Damageable damageable;

    public bool IsHeuristic = false;
    public bool IsInteracting = false;
    /// <summary>
    /// Initial setup, called when the agent is enabled
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        MaxStep = 25000;
        rigidbody = GetComponent<Rigidbody2D>();
        playerCharacter = GetComponent<PlayerCharacter>();
        damageable = GetComponent<Damageable>();
        chomperGameobject = GameObject.Find("Chomper");

    }
    public override void OnEpisodeBegin()
    {
        EpisodeEndLogic();
    }

    private void EpisodeEndLogic()
    {
        // For Simple env
        //transform.localPosition = new Vector3((float)29.24067, (float)-0.2573631, 0);
        // For complex env
        transform.position = new Vector3((float)30.1, (float)-0.32, 0);
        damageable.SetHealth(damageable.startingHealth);
        playerCharacter.UpdateFacing(true);
        //var childTransforms = GameObject.Find("DestructableColumn (2)").GetComponentInChildren<Transform>();
        //foreach (Transform ct in childTransforms)
        //{
        //    if (ct.gameObject.name.Contains("Whole"))
        //    {
        //        ct.gameObject.SetActive(true);
        //    }
        //    else if (ct.gameObject.name.Contains("Broken"))
        //    {
        //        ct.gameObject.SetActive(false);
        //    }
        //}
        chomperGameobject.SetActive(true);
        Animator m_Animator = chomperGameobject.GetComponent<Animator>();
        EnemyBehaviour enemyBehaviour = chomperGameobject.GetComponent<EnemyBehaviour>();
        SceneLinkedSMB<EnemyBehaviour>.Initialise(m_Animator, enemyBehaviour);
        enemyBehaviour.InitializeForMlAgent();
        //For Simple env
        //chomperGameobject.transform.localPosition = new Vector3((float)-24.09782, (float)-0.1943152, 0);
        // For Complex env
        //chomperGameobject.transform.position = new Vector3((float)-0.9322128, (float)7.898977, 0);
        
        //Chomper in 2nd floor
        chomperGameobject.transform.localPosition = new Vector3((float)-0.9322128, (float)3.79, 0);
        //enemyBehaviour.SetFacingData(-1);
        Damageable chomperDamageable = chomperGameobject.GetComponent<Damageable>();
        chomperDamageable.SetHealth(chomperDamageable.startingHealth);
        chomperDamageable.DisableInvulnerability();
        chomperGameobject.SetActive(true);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position.x);
        sensor.AddObservation(transform.position.y);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //Debug.Log(actions.DiscreteActions[0]);
        //float moveX = actions.ContinuousActions[0];
        //float moveZ = actions.ContinuousActions[1];
        float forwardAmount = 0f;
        float gun = actionBuffers.DiscreteActions[1];
        //forwardAmount = 1f;
        float moveSpeed = 5f;
        //transform.position += new Vector3(moveX, 0, 0) * Time.deltaTime * moveSpeed;

        if (actionBuffers.DiscreteActions[0] == 1f)
        {
            forwardAmount = -1f;
        }
        else if (actionBuffers.DiscreteActions[0] == 2f)
        {
            forwardAmount = 1f;
        }

        //Debug.Log("Movement: "+ forwardAmount);
        //rigidbody.MovePosition(transform.position + transform.forward * forwardAmount * moveSpeed * Time.fixedDeltaTime);
        playerCharacter.UpdateFacingForAgent(forwardAmount);
        playerCharacter.GroundedHorizontalMovementForAgent(forwardAmount);

        if (actionBuffers.DiscreteActions[0] == 3f)
        {
            playerCharacter.CheckForCrouchingForAgent(true);
        }
        else
        {
            playerCharacter.CheckForCrouchingForAgent(false);
        }

        if (actionBuffers.DiscreteActions[1] == 3f)
        {
            IsInteracting = true;
            //AddReward(-0.001f);
        }
        else
        {
            IsInteracting = false;
        }

        if (actionBuffers.DiscreteActions[1] == 2f)
        {
            playerCharacter.MeleeAttack();
            AddReward(-0.00001f);
        }
        else if(actionBuffers.DiscreteActions[1] == 1f)
        {
            AddReward(-0.00001f);
        }
        playerCharacter.CheckForHoldingGunForAgent(gun);
        playerCharacter.CheckAndFireGunForAgent(gun);
        //Debug.Log("Attack action: " + gun);

        if(GetCumulativeReward() < -200)
        {
            Debug.Log("Episode ended due to low cumulative reward of " + GetCumulativeReward());
            EndEpisode();
            //EpisodeEndLogic();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> actions = actionsOut.DiscreteActions;
        
        //IsHeuristic = true;
        actions[0] = 0;
        actions[1] = 0;
        if (Input.GetKey(KeyCode.S))  //?? ?? ????. 
        {

            actions[0] = 3;
        }
        if(Input.GetKey(KeyCode.A))
        {
            actions[0] = 1;
        }
        if(Input.GetKey(KeyCode.D))
        {
            actions[0] = 2;
        }
        if (Input.GetKey(KeyCode.O))
        {
            actions[1] = 1;
        }
        if (Input.GetKey(KeyCode.K))
        {
            actions[1] = 2;
        }
        if (Input.GetKey(KeyCode.E))
        {
            actions[1] = 3;
        }
    }

    public void AfterTeleporting()
    {
        //AddReward(0.1f);
        //Debug.Log("Cumulative reward after teleporting:" + GetCumulativeReward());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.name.Contains("InfoPost"))
        //{
        //    // Try to eat the fish
        //    AddReward(10f);
        //    EndEpisode();
        //    EpisodeEndLogic();
        //}
        if (collision.gameObject.name.Contains("Teleporter"))
        {
            // Try to eat the fish
            //AddReward(0.01f);
            //Debug.Log("Cumulative reward after teleporting:" + GetCumulativeReward());
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<InfoPost>(out InfoPost goal))
        {
            SetReward(+1f);
            EndEpisode();
        }
        if (other.TryGetComponent<DestructableColumn>(out DestructableColumn noGoal))
        {
            SetReward(-1f);
            EndEpisode();
        }

        if (other.gameObject.GetType().Equals("DestructableColumn"))
        {
            // Try to eat the fish
            SetReward(-1f);
        }
        else if (other.transform.CompareTag("Chomper"))
        {
            // Try to feed the baby
            SetReward(+1f);
            EndEpisode();
        }
        Debug.Log("Thorugh trigger" +GetCumulativeReward());
    }

    /// <summary>
    /// When the agent collides with something, take action
    /// </summary>
    /// <param name="collision">The collision info</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetType().Equals("DestructableColumn"))
        {
            // Try to eat the fish
            SetReward(-1f);
        }
        else if (collision.transform.CompareTag("Chomper"))
        {
            // Try to feed the baby
            SetReward(+1f);
            EndEpisode();
        }
        Debug.Log(GetCumulativeReward());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("DestructibleColumn") || collision.gameObject.name.Contains("DestructableColumn"))
        {
            // Try to eat the fish
            AddReward(-0.001f);
        }
        //else if (collision.gameObject.name.Contains("Chomper"))
        //{
        //    // Try to feed the baby
        //    SetReward(+1f);
        //    EndEpisode();
        //}
        Debug.Log("Cumulative reward:" +GetCumulativeReward());
    }

    public void OnHurt()
    {
        AddReward(-0.2f);
        Debug.Log("Cumulative reward after hurt:" + GetCumulativeReward());
        if(damageable.CurrentHealth < 3)
        {
            AddReward(-1f);
            Debug.Log("Cumulative reward after 3 hits die:" + GetCumulativeReward());
            EndEpisode();
            //EpisodeEndLogic();
        }
    }

    public void OnDie()
    {
        AddReward(-1f);
        Debug.Log("Cumulative reward after die:" + GetCumulativeReward());
        EndEpisode();
        //EpisodeEndLogic();
    }

    public void OnHit(Damager damager, Damageable damageable)
    {
        if(damageable.CurrentHealth > 0)
        {
            AddReward(1f);
        }
        else
        {
            AddReward(5f);
        }
        Debug.Log("Cumulative reward after hit:" + GetCumulativeReward());
    }

    public void OnHitEnemies(Damageable chomperDamageable)
    {
        AddReward(0.1f);
        Debug.Log("Cumulative reward after hit:" + GetCumulativeReward());
        if (chomperDamageable.CurrentHealth < 4)
        {
            
            AddReward(1f);
            Debug.Log("Cumulative reward after kill:" + GetCumulativeReward());
            EndEpisode();
            //EpisodeEndLogic();
        }
    }

    public void OnDestroyEnemies()
    {
        //AddReward(5f);
        Debug.Log("Cumulative reward after kill:" + GetCumulativeReward());
        AddReward(1f);
        EndEpisode();
        //EpisodeEndLogic();
    }


}
