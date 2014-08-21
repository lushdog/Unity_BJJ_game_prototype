using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;

public enum BodyPointLocation { Front, Back, Left, Right }

public abstract class MovementManager : IMovementManager
{
    public List<BodyPoint> BodyPoints;

    protected BjjPlayer _player;
    protected FullBodyBipedIK _ik;
    protected InteractionSystem _is;
    public BodyPoint leftHand, rightHand, leftFoot, rightFoot, leftThigh, rightThigh, leftShoulder, rightShoulder, body;
   
    private const float MOVE_COMPLETE_PROXIMITY = 0.01f;
    private const float HAND_MOVE_SPEED = 2.0f;
  
    public MovementManager(BjjPlayer bjjPlayer) //TODO: really we should be passing a subclass of BjjPlayer here
    {
        _player = bjjPlayer;
        _ik = _player.GetComponentInChildren<FullBodyBipedIK>();
        _is = _player.GetComponentInChildren<InteractionSystem>();
        _ik.Disable();

        leftHand = new BodyPoint(SearchHierarchyForBone(_player.transform, "L Wrist").gameObject, FullBodyBipedEffector.LeftHand);
        rightHand = new BodyPoint(SearchHierarchyForBone(_player.transform, "R Hand GP").gameObject, FullBodyBipedEffector.RightHand);
        leftFoot = new BodyPoint(null, FullBodyBipedEffector.LeftFoot);
        rightFoot = new BodyPoint(null, FullBodyBipedEffector.RightFoot);
        leftThigh = new BodyPoint(null, FullBodyBipedEffector.LeftThigh);
        rightThigh = new BodyPoint(null, FullBodyBipedEffector.RightThigh);
        leftShoulder = new BodyPoint(null, FullBodyBipedEffector.LeftShoulder);
        rightShoulder = new BodyPoint(null, FullBodyBipedEffector.RightShoulder);
        body = new BodyPoint(null, FullBodyBipedEffector.Body);

        BodyPoints = new List<BodyPoint> {leftHand, rightHand, leftFoot, rightFoot, leftThigh, rightThigh, leftShoulder, rightShoulder, body};
    }

    public void Update()
    {
        foreach (BodyPoint bp in BodyPoints) 
        {
            if (bp.MovementType == MovementType.Absolute)
            {
                var effector = _ik.solver.GetEffector(bp.Effector);
                effector.position = bp.EffectorTargetPosition;
                effector.positionWeight = bp.EffectorTargetPositionWeight;
                effector.rotation = bp.EffectorTargetRotation;
                effector.rotationWeight = bp.EffectorTargetRotationWeight;
                bp.MovementType = MovementType.None;
            }
        }
        _ik.solver.Update();
    }

    //TODO: Push() and Pull() /*like grab but no target on opponent's body*/
    /*
    public bool Grab(MovePoint grabber, AttachPoint target)
    {
        bool reachedTarget = Move(grabber, target.AttachObject.transform);
        if (reachedTarget)
        {
            //DebugLog("Reached target Player " + PlayerNumber);
            target.AttachedTo = grabber; 
        }
        return reachedTarget;
    }
    */

    public bool Move(BodyPoint bodyPoint, Transform target)  
    {
        IKEffector effector = _ik.solver.GetEffector(bodyPoint.Effector);
        if (Vector3.Distance(effector.position, target.position) < MOVE_COMPLETE_PROXIMITY)
        {
            return true;
        }
        
        float step = HAND_MOVE_SPEED * Time.deltaTime;
        bodyPoint.EffectorTargetPositionWeight = 1.0f;
        bodyPoint.EffectorTargetPosition = Vector3.Lerp(effector.position, target.position, step);
        Debug.DrawLine(bodyPoint.EffectorTargetPosition, target.position, Color.white);
        
        return false;
    }
    
    public void Pose(Transform targets)  
    {
        foreach (BodyPoint bp in BodyPoints)
        {
            var bone = _ik.solver.GetEffector(bp.Effector).bone;

            if (bone == null)
                throw new NullReferenceException(bp.Effector + " does not have a bone.");

            if (bone.name == null)
                throw new NullReferenceException(bone.ToString() + " bone does not have a name.");

            var target = SearchHierarchyForBone(targets, bone.name);
            bp.EffectorTargetPosition = target.position;
            bp.EffectorTargetRotation = target.rotation;
            bp.EffectorTargetPositionWeight = 1.0f;
            bp.EffectorTargetRotationWeight = 1.0f;
            bp.MovementType = MovementType.Absolute;
        }
    }
    
    private Transform SearchHierarchyForBone(Transform current, string name)   
    {
        if ((current.name.Contains(name) || current.name == name))
            return current;
        
        for (int i = 0; i < current.childCount; ++i)
        {
            Transform found = SearchHierarchyForBone(current.GetChild(i), name);
            if (found != null)
                return found;
        }
        return null;
    }

    private FullBodyBipedEffector GetParentEffector(FullBodyBipedEffector childEffector)
    {
        if (childEffector == FullBodyBipedEffector.Body)
        {
            throw new ArgumentException("Body effector has no parent effector.");
        }

        FullBodyBipedEffector parentEffector = FullBodyBipedEffector.Body;
        if (childEffector == FullBodyBipedEffector.LeftHand)
            parentEffector = FullBodyBipedEffector.LeftShoulder;
        else if (childEffector == FullBodyBipedEffector.RightHand)
            parentEffector = FullBodyBipedEffector.RightShoulder;
        else if (childEffector == FullBodyBipedEffector.LeftFoot)
            parentEffector = FullBodyBipedEffector.LeftThigh;
        else if (childEffector == FullBodyBipedEffector.RightFoot)
            parentEffector = FullBodyBipedEffector.RightThigh;

        return parentEffector;
    }

}
