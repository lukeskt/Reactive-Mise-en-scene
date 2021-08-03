using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Nimbus : MonoBehaviour
{
    // Would be nice to combine multiple colliders to have a complex Nimbus / Aura?
    public Collider NimbusCollider;
    public float NimbusRadius;
    //private Vector3 centrepoint;

    [Range(0.0f, 10.0f)] [SerializeField] private float farDistance = 2.5f;
    [Range(0.0f, 10.0f)] [SerializeField] private float midDistance = 1.0f;

    // enums
    private enum Proximity
    {
        Far = 2,
        Mid = 1,
        Near = 0,
        Error = 400
    }

    private enum Direction
    {
        Left = 90,
        Ahead = 0,
        Right = -90,
        Behind = 180
    }

    private enum Elevation
    {
        Above = 1,
        Same = 0,
        Below = -1,
        Error = 400
    }

    // Events
    // These should send an event containing the object that entered, plus its position, direction, elevation.
    public UnityEvent<GameObject> EnterNimbus;
    public UnityEvent<GameObject> StayNimbus;
    public UnityEvent<GameObject> ExitNimbus;

    public UnityEvent<GameObject> WriteToNimbusDataStore;

    // Start is called before the first frame update
    void Start()
    {
        //if (GetComponent<Collider>().name == "Nimbus") nimbus = GetComponent<Collider>();
        //centrepoint = nimbus.bounds.center; // get centre of collider, measure position and distance from there for a scene/area/etc.
        NimbusCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool GetObjectVisibility (GameObject gameObject)
    {
        // Check if the colliding object makes sense to have a nimbus event, i.e. it isn't behind a wall etc.
        Debug.DrawLine(transform.position, gameObject.GetComponent<Collider>().bounds.center, Color.magenta);
        if (Physics.Linecast(transform.position,
                             gameObject.GetComponent<Collider>().bounds.center,
                             out RaycastHit hit) 
            && hit.collider.name == gameObject.name) { return true; }
        else { return false; }
    }

    Proximity GetObjectDistance (GameObject gameObject)
    {
        // Can we normalize this or something to range from 0 to 1?
        float dist = Vector3.Distance(gameObject.transform.position, transform.position);
        //Debug.Log($"Distance:{dist}");
        if (dist > farDistance)
        {
            return Proximity.Far;
        }
        else if (dist < farDistance && dist > midDistance)
        {
            return Proximity.Mid;
        }
        else if (dist < midDistance)
        {
            return Proximity.Near;
        }
        else
        {
            return Proximity.Error;
        }
    }

    Direction GetObjectDirection (GameObject gameObject)
    {
        Vector3 targetDir = gameObject.transform.position - transform.position;
        Vector3 forward = transform.forward;
        float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);

        if (angle > 45 && angle < 135) { return Direction.Left; }
        else if (-45 < angle && angle < 45) { return Direction.Ahead; }
        else if (angle < -45 && angle > -135) { return Direction.Right; }
        else { return Direction.Behind; }
    }

    // This just doesn't work! Numbers all wrong
    Elevation GetObjectElevation (GameObject gameObj)
    {
        float dotProdElevation = Vector3.Dot(transform.up, gameObj.transform.position);
        Debug.Log($"Elevation:{dotProdElevation}");
        if (dotProdElevation > 1)
        {
            return Elevation.Above;
        }
        else if (dotProdElevation < -1)
        {
            return Elevation.Below;
        }
        else
        {
            return Elevation.Same;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Set relevant vars
        // Get Distance
        // Get angle/positions
        // Send Events
        // other.gameObject.Invoke();
        // Event - send details of object entering and its position, direction, elevation.
    }

    private void OnTriggerStay(Collider other)
    {
        if (GetObjectVisibility(other.gameObject))
        {
            Proximity dist = GetObjectDistance(other.gameObject);
            Direction dir = GetObjectDirection(other.gameObject);
            //Elevation elev = GetObjectElevation(other.gameObject);
            Debug.Log($"Name: {other.gameObject.name}, Dist:{dist}, Dir:{dir}");//, Elev:{elev}");
        }
        // Event - Update the position, direction, elevation of object in trigger.
    }

    private void OnTriggerExit(Collider other)
    {
        // Event - send details of object that exited and its pos, dir, and elevation at the time.
    }
}
