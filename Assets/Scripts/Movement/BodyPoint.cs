using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;

public enum MovementType
{
    None, Absolute, Offset, Interaction
}

public class BodyPoint 
{
    public GameObject GameObject
    {
        get;
        set;
    }

    public FullBodyBipedEffector Effector
    {
        get;
        set;
    }

    public Vector3 EffectorTargetPosition
    {
        get;
        set;
    }

    public Quaternion EffectorTargetRotation
    {
        get;
        set;
    }

    public float EffectorTargetPositionWeight
    {
        get;
        set;
    }

    public float EffectorTargetRotationWeight
    {
        get;
        set;
    }

    public MovementType MovementType
    {
        get;
        set;
    }

    public BodyPoint (GameObject gameObject, FullBodyBipedEffector effector)
    {
        GameObject = gameObject;
        Effector = effector;
        MovementType = MovementType.None;
    }

    public InteractionObject GetInteractionObject(BodyPointLocation location)
    {
        return GameObject.transform.FindChild(location.ToString()).GetComponentInChildren<InteractionObject>();
    }
}
