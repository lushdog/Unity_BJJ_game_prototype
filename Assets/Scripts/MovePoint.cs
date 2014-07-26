using UnityEngine;
using RootMotion.FinalIK;
using System.Collections;

public class MovePoint : Object {

    private IKEffector _effector;

    public enum MoveType
    {
        Offset, NonOffset, Idle
    }

    public FullBodyBipedEffector EffectorType
    {
        get;
        set;
    }

    public IKEffector Effector
    {
        get { return _effector; }
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

    public float EffectorTargetPostionWeight
    {
        get;
        set;
    }

    public float EffectorTargetRotationWeight
    {
        get;
        set;
    }

    public MoveType MovementType 
    {
        get;
        set;
    }

    public MovePoint Parent
    {
        get;
        set;
    }

    public MovePoint(IKEffector effector, FullBodyBipedEffector effectorType, MovePoint parentMovePoint)
    {
        _effector = effector;
        EffectorType = effectorType;
        MovementType = MoveType.Idle;
        Parent = parentMovePoint;
        EffectorTargetPosition = new Vector3(0, 0, 0);
        EffectorTargetRotation = Quaternion.identity;
    }

    public void Update()
    {
        _effector.position = EffectorTargetPosition;
        _effector.rotation = EffectorTargetRotation;
        _effector.positionWeight = EffectorTargetPostionWeight;
        _effector.rotationWeight = EffectorTargetRotationWeight;
    }

}
