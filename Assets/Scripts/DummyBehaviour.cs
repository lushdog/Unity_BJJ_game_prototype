using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;

public class DummyBehaviour : MonoBehaviour {

    public int PlayerNumber;

    private List<MovePoint> movePoints;
    private GameObject opponent;
	private string state;
    private const float GRAB_COMPLETE_PROXIMITY = 0.01f;
    private const float HAND_GRAB_SPEED = 2.0f;



    public void SetState(string state)  
    {
		this.state = state;
	}

	public void SetOpponent(GameObject opponent)  
    {
		this.opponent = opponent;
	}

	

    private void Start () 
    {
        movePoints = InitMovePoints();
	}

    private List<MovePoint> InitMovePoints()
    {
        var ik = GetComponentInChildren<FullBodyBipedIK>();

        MovePoint body = new MovePoint(ik.solver.bodyEffector, FullBodyBipedEffector.Body, null);
        MovePoint leftShoulder = new MovePoint(ik.solver.leftShoulderEffector, FullBodyBipedEffector.LeftShoulder, body);
        MovePoint rightShoulder = new MovePoint(ik.solver.rightShoulderEffector, FullBodyBipedEffector.RightShoulder, body);
        MovePoint leftHand = new MovePoint(ik.solver.leftHandEffector, FullBodyBipedEffector.LeftHand, leftShoulder);
        MovePoint rightHand = new MovePoint(ik.solver.rightHandEffector, FullBodyBipedEffector.RightHand, rightShoulder);
        MovePoint leftThigh = new MovePoint(ik.solver.leftThighEffector, FullBodyBipedEffector.LeftThigh, body);
        MovePoint rightThigh = new MovePoint(ik.solver.rightThighEffector, FullBodyBipedEffector.RightThigh, body);
        MovePoint leftFoot = new MovePoint(ik.solver.leftFootEffector, FullBodyBipedEffector.LeftFoot, body);
        MovePoint rightFoot = new MovePoint(ik.solver.rightFootEffector, FullBodyBipedEffector.RightFoot, body);
        return new List<MovePoint> { body, 
                                    leftShoulder, rightShoulder, 
                                    leftHand, rightHand, 
                                    leftThigh, rightThigh, 
                                    leftFoot, rightFoot };
    }
	
	private void Update () 
    {
		if (state == "PullGuard")
        {
            PullGuard();
        } 
        else if (state == "EnterGuard")
        {
            EnterGuard();
        } 
        else if (state == "Idle" && Input.GetKeyDown("space"))
        {
            //if (PlayerNumber == 1)
                SetState("GrabbingHand");
        } 
        else if (state == "GrabbingHand")
        {
            GrabLeftWristWithRightHand();
            GrabRightWristWithLeftHand();
        } 
        else if (state == "Idle")
        {
            //isOffsetUpdate = null;
        }
	}

	private void LateUpdate()  
    {
		foreach (MovePoint mp in movePoints) 
		{
            mp.Update();
		}
	}

	private void PullGuard() {
		Pose(this.transform.Find("Positions/GuardTop").transform);
		SetState("Idle");
	}
	
	private void EnterGuard() {
		Pose(this.transform.Find("Positions/GuardBottom").transform);
		SetState("Idle");
	}

	private void GrabLeftWristWithRightHand()
	{
        var opponentLeftHand = SearchHierarchyForBone(opponent.transform, "L Hand GP");
        Grab(movePoints.Find(x => x.EffectorType == FullBodyBipedEffector.RightHand), opponentLeftHand);
	}

    private void GrabRightWristWithLeftHand()
    {
        var opponentRightHand = SearchHierarchyForBone(opponent.transform, "R Hand GP");
        Grab(movePoints.Find(x => x.EffectorType == FullBodyBipedEffector.LeftHand), opponentRightHand);
    }

    private void Grab(MovePoint grabber, Transform target)
    {
        bool reachedTarget = Move(grabber, target);
        if (reachedTarget)
        {
            SetState("Idle");
        }
    }

	private bool Move(MovePoint movePoint, Transform target)  
    {
        if (Vector3.Distance(movePoint.Effector.position, target.position) < GRAB_COMPLETE_PROXIMITY)
        {
            Debug.Log("Reached target Player " + PlayerNumber);
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

	private void Pose(Transform targets)  
    {
        foreach (MovePoint mp in movePoints)
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