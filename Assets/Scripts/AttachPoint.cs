using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;

[System.Serializable]
public enum AttachLocation
{
    None, Neck, Head, Shoulder, Wrist, Bicep, Knee, Shin, Ankle, Foot, Hand
}

[System.Serializable]
public enum AttachSide
{
    None, Left, Right
}

[System.Serializable]
public enum AttachDepth
{
    None, Front, Back
}

[System.Serializable]
public class AttachPoint
{
    public GameObject AttachObject
    {
        get;
        set;
    }

    public AttachLocation Location 
    {
        get;
        set;
    }

    public AttachSide Side 
    {
        get;
        set;
    }

    public AttachDepth Depth 
    {
        get;
        set;
    }

    public MovePoint ClosestMovePoint
    {
        get;
        set;
    }

    public MovePoint AttachedTo
    {
        get;
        set;
    }

    public AttachPoint(GameObject attachObject, MovePoint closestMovePoint, AttachLocation location, AttachSide side, AttachDepth depth)
    {
        AttachObject = attachObject;
        ClosestMovePoint = closestMovePoint;
        Location = location;
        Side = side;
        Depth = depth;
    }
}
