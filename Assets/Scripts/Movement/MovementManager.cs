using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;

public enum BodyPart { Body, LeftShoulder, RightShoulder, LeftHand, RightHand, LeftThigh, RightThigh, LeftFoot, RightFoot, LeftWrist, RightWrist };

public abstract class MovementManager : IMovementManager
{
    public Dictionary<BodyPart, AttachPoint> AttachPoints;
    public Dictionary<BodyPart, MovePoint> MovePoints;
   
    protected BjjPlayer _player;
    private const float GRAB_COMPLETE_PROXIMITY = 0.01f;
    private const float HAND_GRAB_SPEED = 2.0f;

    public MovementManager(BjjPlayer bjjPlayer) //TODO: really we should be passing a subclass of BjjPlayer here
    {
        _player = bjjPlayer;
        var ik = _player.GetComponentInChildren<FullBodyBipedIK>();
        
        MovePoint body = new MovePoint(ik.solver.bodyEffector, FullBodyBipedEffector.Body, null);
        MovePoint leftShoulder = new MovePoint(ik.solver.leftShoulderEffector, FullBodyBipedEffector.LeftShoulder, body);
        MovePoint rightShoulder = new MovePoint(ik.solver.rightShoulderEffector, FullBodyBipedEffector.RightShoulder, body);
        MovePoint leftHand = new MovePoint(ik.solver.leftHandEffector, FullBodyBipedEffector.LeftHand, leftShoulder);
        MovePoint rightHand = new MovePoint(ik.solver.rightHandEffector, FullBodyBipedEffector.RightHand, rightShoulder);
        MovePoint leftThigh = new MovePoint(ik.solver.leftThighEffector, FullBodyBipedEffector.LeftThigh, body);
        MovePoint rightThigh = new MovePoint(ik.solver.rightThighEffector, FullBodyBipedEffector.RightThigh, body);
        MovePoint leftFoot = new MovePoint(ik.solver.leftFootEffector, FullBodyBipedEffector.LeftFoot, body);
        MovePoint rightFoot = new MovePoint(ik.solver.rightFootEffector, FullBodyBipedEffector.RightFoot, body);
        MovePoints = new Dictionary<BodyPart, MovePoint> { { BodyPart.Body, body},
            {BodyPart.LeftShoulder, leftShoulder}, {BodyPart.RightShoulder, rightShoulder}, 
            {BodyPart.LeftHand, leftHand}, {BodyPart.RightHand, rightHand},
            {BodyPart.LeftThigh, leftThigh}, {BodyPart.RightThigh, rightThigh}, 
            {BodyPart.LeftFoot, leftFoot}, {BodyPart.RightFoot, rightFoot}};
        
        
        //TODO: Create rest of these
        AttachPoint leftWrist = new AttachPoint(SearchHierarchyForBone(_player.transform, "L Hand GP").gameObject, leftHand, AttachLocation.Wrist, AttachSide.Left, AttachDepth.Front);
        AttachPoint rightWrist = new AttachPoint(SearchHierarchyForBone(_player.transform, "R Hand GP").gameObject, rightHand, AttachLocation.Wrist, AttachSide.Right, AttachDepth.Front);
        AttachPoints = new Dictionary<BodyPart, AttachPoint> {{BodyPart.LeftWrist, leftWrist}, {BodyPart.RightWrist, rightWrist }};
    }

    public void Update()
    {
        foreach (MovePoint mp in MovePoints.Values) 
        {
            mp.Update();
        }
        
        foreach (AttachPoint ap in AttachPoints.Values)
        {
            if (ap.AttachedTo != null)
            {
                //TODO: slight hitch upon attach due to slight difference between position of attach point and grab complete prox.
                ap.ClosestMovePoint.EffectorTargetPosition = ap.AttachedTo.EffectorTargetPosition;
                ap.ClosestMovePoint.Update();
            }
        }
    }

    //TODO: Push() and Pull() /*like grab but no target on opponent's body*/
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
    
    public bool Move(MovePoint movePoint, Transform target)  
    {
        if (Vector3.Distance(movePoint.Effector.position, target.position) < GRAB_COMPLETE_PROXIMITY)
        {
            return true;
        }
        
        float step = HAND_GRAB_SPEED * Time.deltaTime;
        movePoint.EffectorTargetPostionWeight = 1.0f;
        movePoint.EffectorTargetPosition = Vector3.Lerp(movePoint.Effector.position, target.position, step);
        //TODO: align EffectorTargetRotation w/ target.Rotation to make grabbing look better, maybe param to Move() to determine align
        //      style (grab version push versus foot hook etc. etc.
        //TODO: RotationWeightTarget
        
        if (movePoint.Effector.isEndEffector)
        {
            movePoint.Parent.EffectorTargetPostionWeight = 
                Mathf.Lerp(movePoint.Parent.Effector.positionWeight, 0.0f, step);
        }
        
        Debug.DrawLine(movePoint.EffectorTargetPosition, target.position, Color.white);
        
        return false;
    }
    
    public void Pose(Transform targets)  
    {
        foreach (MovePoint mp in MovePoints.Values)
        {
            var target = SearchHierarchyForBone(targets, mp.Effector.bone.name);
            mp.EffectorTargetPosition = target.position;
            mp.EffectorTargetRotation = target.rotation;
            mp.EffectorTargetPostionWeight = 1.0f;
            mp.EffectorTargetRotationWeight = 1.0f;
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
