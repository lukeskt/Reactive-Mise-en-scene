using System;
using System.Collections.Generic;
using UnityEngine;

// TODO: For ALL interactions: Convert to multiple components for ECS.
// Place these structs into Array of Structs (or Struct of Arrays?) for orchestration etc.
// Use Systems (orchestrators) to handle (each part?) processing.
public class ObjectNimbus : MonoBehaviour
{
    // This Object's Nimbus
    public Collider Nimbus;
    public float NimbusRadius;

    private enum Direction
    {
        left = 90,
        ahead = 0,
        right = -90,
        behind = 180
    }

    private enum YDirection
    {
        above = 1,
        equal = 0,
        below = -1
    }

    private Dictionary<GameObject, (float Distance, Direction Direction, bool Visibility)> ObjectsInNimbus = new Dictionary<GameObject, (float Distance, Direction Direction, bool Visibility)>();

    // Other Objects' Nimbii
    bool isInANimbus;
    List<GameObject> inNimbusesOf = new List<GameObject>();
    float distanceFrom; // ugh, distance from what?
    TimeSpan totalNimbusTime; // of what?
    TimeSpan lastNimbusTime;
    TimeSpan firstNimbusTime;
    TimeSpan longestNimbusTime;

    // Functions
    void Start()
    {
        Nimbus.isTrigger = true;
    }

    void Update()
    {
        // HANG ON! Do we want triggers etc, or do we want to measure distances? (Also physics sphere?)
        // Then we could, based on nimbus-specific objects, handle distances and then trigger actions, position, etc.
    }

    // no longer need overlapsphere since querytriggeraction working?
    private void OverlapSphereNimbus(float rad)
    {
        Collider[] nimbusCheck = Physics.OverlapSphere(transform.position, rad);
        foreach (var collider in nimbusCheck)
        {
            //GetDistance();
            //GetDirection();
            //GetRelativeHeight();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!ObjectsInNimbus.ContainsKey(other.gameObject))
            ObjectsInNimbus.Add(
                other.gameObject,
                (GetDistanceFromNimbusCentre(gameObject),
                 GetRelativePositionFromNimbusCentre(gameObject),
                 GetVisibilityFromNimbusCentre(gameObject))
            );
    }

    private void OnTriggerStay(Collider other)
    {
        // For objects in, keep updating position and distance!
        ObjectsInNimbus[other.gameObject] =
            (GetDistanceFromNimbusCentre(other.gameObject),
             GetRelativePositionFromNimbusCentre(other.gameObject),
             GetVisibilityFromNimbusCentre(other.gameObject));
        foreach (var gameObject in ObjectsInNimbus)
        {
            print($"Object: {gameObject.Key.name}, Distance: {gameObject.Value.Distance}, Direction {gameObject.Value.Direction}, Visibility: {gameObject.Value.Visibility}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (ObjectsInNimbus.ContainsKey(other.gameObject))
            ObjectsInNimbus.Remove(other.gameObject);
    }

    // Move stuff like distance and direction and visible funcs into util funcs and call from there?
    // Set up UnityEvents for useful events like viewer in nimbus, and their details?

    void CheckForViewersInNimbus(List<GameObject> gameObjects)
    {
        // Filter all objects in nimbus and check if any are viewers.
    }

    float GetDistanceFromNimbusCentre(GameObject gameObject)
    {
        return Vector3.Distance(gameObject.transform.position, transform.position);
    }

    Direction GetRelativePositionFromNimbusCentre(GameObject gameObject)
    {
        // TODO: Check if other objects are ahead, behind, left, right of this object. 
        // This code is garbage, make it better
        Vector3 targetDir = gameObject.transform.position - transform.position;
        Vector3 forward = transform.forward;
        float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);

        if (angle > 45 && angle < 135) { return Direction.left; }
        else if (-45 < angle && angle < 45) { return Direction.ahead; }
        else if (angle < -45 && angle > -135) { return Direction.right; }
        else { return Direction.behind; } 
    }

    private void GetRelativeYPosition()
    {

    } // i.e. above, below, same height.

    bool GetVisibilityFromNimbusCentre(GameObject gameObject)
    {
        // TODO: Check if other objects have direct "Line Of Sight" to this object.
        Debug.DrawLine(transform.position, gameObject.GetComponent<Collider>().bounds.center, Color.magenta);
        if (Physics.Linecast(transform.position,
                             gameObject.GetComponent<Collider>().bounds.center,
                             out RaycastHit hit) &&
            hit.collider.name == gameObject.name)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
