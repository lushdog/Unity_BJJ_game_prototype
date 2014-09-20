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
            var effector = _ik.solver.GetEffector(bp.Effector);
            if (bp.MovementType == MovementType.Absolute)
            {
                effector.position = bp.EffectorTargetPosition;
                effector.positionWeight = bp.EffectorTargetPositionWeight;
                effector.rotation = bp.EffectorTargetRotation;
                effector.rotationWeight = bp.EffectorTargetRotationWeight;
                bp.MovementType = MovementType.None;
            }
            else if (bp.MovementType == MovementType.Offset)
            {
                effector.positionOffset += bp.EffectorTargetPosition;
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
        if (targets == null)
            throw new NullReferenceException("Pose targets are null.");

        //TODO(1): need to move effectors to new bone positions but not affect targets when effector is moved 
        List<Transform> bones = new List<Transform>();
        GetBones(targets, bones);
        
        foreach (Transform target in bones)
        {
            if (target == null)
                throw new NullReferenceException("Null target found in Pose() targets.");

            var current = SearchHierarchyForBone(_player.transform, target.name);
            if (current == null)
                throw new NullReferenceException("No match for " + target.name + " target in player bones.");

            //Debug.Log("Found match for " + target.name + " .");
            current.transform.position = target.position;
            current.transform.rotation = target.rotation;
        }
    }

    private void GetBones(Transform current, List<Transform> bones)
    {
        bones.Add(current);
        for (int i = 0; i < current.childCount; ++i)
        {
            var child = current.GetChild(i);
            bones.Add(child);
            GetBones(child, bones);
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
}
