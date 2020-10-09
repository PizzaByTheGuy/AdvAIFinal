using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Transform player;
    private NavMeshAgent nav;
    private Vector3 targerDir;
    private int patrolPointIndex = 0;
    private float distance;
    private bool sight;
    public Transform[] patrolPoints;


    private float patrolSpeed = 5f;



    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        nav = GetComponent<NavMeshAgent>();
        patrolPointIndex = 0;
        transform.LookAt(patrolPoints[patrolPointIndex].position);

    }

    // Update is called once per frame
    void Update()
    {
        sight = RaycastSight();


        targerDir = player.position - transform.position;

        float _angle = (Vector3.Angle(targerDir, transform.forward));

        if (_angle >= -50 && _angle <= 50 && sight == true)
        {
            nav.SetDestination(player.position);
            patrolMovement();
        }
        else
        {
            nav.SetDestination(patrolPoints[patrolPointIndex].position);
            patrolMovement();


        }

        distance = Vector3.Distance(transform.position, patrolPoints[patrolPointIndex].position);

        if (distance <= 1.0f)
        {
            increasePatrolIndex();
        }

    }

    private void patrolMovement()
    {
        transform.Translate(Vector3.forward * patrolSpeed * Time.deltaTime);
    }

    private void increasePatrolIndex()
    {
        patrolPointIndex++;

        if (patrolPointIndex >= patrolPoints.Length)
        {
            patrolPointIndex = 0;
        }

        nav.SetDestination(patrolPoints[patrolPointIndex].position);
    }

    private bool RaycastSight()
    {
        Vector3 objectOrgin = transform.position;
        Vector3 sightDirection = transform.forward;
        float maxDistance = 3.0f;

        Debug.DrawRay(objectOrgin, sightDirection * maxDistance, Color.red);

        Ray ray = new Ray(objectOrgin, sightDirection);

        bool visable = Physics.Raycast(ray, out RaycastHit raycast, maxDistance);

        if (visable)
        {
            raycast.collider.CompareTag("Player");
        }

        return visable;

    }
}