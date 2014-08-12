using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BjjMovementManager : MovementManager 
{	
    public BjjMovementManager(BjjPlayer player) : base(player)
    {
        
    }

    public void PullGuard() 
    {
        Pose(_player.transform.Find("Positions/GuardTop").transform);
        _player.SetState(BjjState.Idle);
    }
    
    public void EnterGuard() 
    {
        Pose(_player.transform.Find("Positions/GuardBottom").transform);
        _player.SetState(BjjState.Idle);
    }
    
    public void GrabLeftWristWithRightHand()
    {
        MovePoint grabber = MovePoints[BodyPart.RightHand];
        AttachPoint target = _player.Opponent.MovementManager.AttachPoints[BodyPart.LeftWrist];
        if (Grab(grabber, target))
        {
            _player.SetState(BjjState.Idle);
        }
    }
    
    public void GrabRightWristWithRightHand()
    {
        MovePoint grabber = MovePoints[BodyPart.RightHand];
        AttachPoint target = _player.Opponent.MovementManager.AttachPoints[BodyPart.RightWrist];
        if (Grab(grabber, target))
        {
            _player.SetState(BjjState.Idle);
        }
    }
    
    public void GrabRightWristWithLeftHand()
    {
        MovePoint grabber = MovePoints[BodyPart.RightWrist];
        AttachPoint target = _player.Opponent.MovementManager.AttachPoints[BodyPart.LeftHand];
        if (Grab(grabber, target))
        {
            _player.SetState(BjjState.Idle);
        }
    }
}
