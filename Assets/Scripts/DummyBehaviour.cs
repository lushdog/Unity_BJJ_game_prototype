using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;

[System.Serializable]
public class DummyBehaviour : MonoBehaviour {

    public int PlayerNumber;
    public List<AttachPoint> AttachPoints;
    public List<IState> States;

    private List<MovePoint> _movePoints;
    private GameObject _opponent;
	private IState _currentState;

    private const float GRAB_COMPLETE_PROXIMITY = 0.01f;
    private const float HAND_GRAB_SPEED = 2.0f;

    public void DebugLog(string message)
    {
        Debug.Log("Player" + PlayerNumber + ":" + message);
    }

    public void SetState(IState state)  
    {
		if (_currentState != null)
        {
            _currentState.TransitionTo(state);
        } 
        else
        {
            state.TransitionFrom(null);
        }
        _currentState = state;
	}

	public void SetOpponent(GameObject opponent)  
    {
		_opponent = opponent;
	}

    private void Awake()
    {
        InitPoints();
        InitStates();
    }
	
    private void Start () 
    {
        if (_currentState == null)
            SetState(States.Find(x => x is IdleState));
	}

    private void InitStates()
    {
        IdleState idle = new IdleState(this);
        GrabbingState grab = new GrabbingState(this);
        PullingGuard pullingGuard = new PullingGuard(this);
        EnteringGuard enteringGuard = new EnteringGuard(this);
        States = new List<IState> { idle, grab, pullingGuard, enteringGuard };
    }

    private void InitPoints()
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
        _movePoints = new List<MovePoint> { body, 
                                    leftShoulder, rightShoulder, 
                                    leftHand, rightHand, 
                                    leftThigh, rightThigh, 
                                    leftFoot, rightFoot };

        //TODO: Create rest of these
        AttachPoint leftWrist = new AttachPoint(SearchHierarchyForBone(this.transform, "L Hand GP").gameObject, leftHand, AttachLocation.Wrist, AttachSide.Left, AttachDepth.Front);
        AttachPoint rightWrist = new AttachPoint(SearchHierarchyForBone(this.transform, "R Hand GP").gameObject, rightHand, AttachLocation.Wrist, AttachSide.Right, AttachDepth.Front);
        AttachPoints = new List<AttachPoint> { leftWrist, rightWrist };

    }

	private void Update () 
    {
	   
        //input manager
        if (_currentState is IdleState && Input.GetKeyDown("space"))
        {
            if (PlayerNumber == 1)
            {
                SetState(States.Find(x => x is GrabbingState));
            }
        } 
       
        _currentState.Update();
        //DebugLog(state);
	}

	private void LateUpdate()  
    {
		
        foreach (MovePoint mp in _movePoints) 
		{
            mp.Update();
		}

        foreach (AttachPoint ap in AttachPoints)
        {
            if (ap.AttachedTo != null)
            {
                //TODO: slight hitch upon attach due to slight difference between position of attach point and grab complete prox.
                ap.ClosestMovePoint.EffectorTargetPosition = ap.AttachedTo.EffectorTargetPosition;
                ap.ClosestMovePoint.Update();
            }
        }
	}

	public void PullGuard() {
		Pose(this.transform.Find("Positions/GuardTop").transform);
		SetState(States.Find(x => x is IdleState));
	}
	
	public void EnterGuard() {
		Pose(this.transform.Find("Positions/GuardBottom").transform);
        SetState(States.Find(x => x is IdleState));
	}

    public void GrabLeftWristWithRightHand()
	{
        MovePoint grabber = _movePoints.Find(x => x.EffectorType == FullBodyBipedEffector.RightHand);
        AttachPoint target = _opponent.GetComponent<DummyBehaviour>().AttachPoints.Find
            (x => (x.Location == AttachLocation.Wrist) && (x.Side == AttachSide.Left) && (x.Depth == AttachDepth.Front));
        if (Grab(grabber, target))
        {
            SetState(States.Find(x => x is IdleState));
        }
	}

    public void GrabRightWristWithRightHand()
    {
        MovePoint grabber = _movePoints.Find(x => x.EffectorType == FullBodyBipedEffector.RightHand);
        AttachPoint target = _opponent.GetComponent<DummyBehaviour>().AttachPoints.Find
            (x => (x.Location == AttachLocation.Wrist) && (x.Side == AttachSide.Right) && (x.Depth == AttachDepth.Front));
        if (Grab(grabber, target))
        {
            SetState(States.Find(x => x is IdleState));
        }
    }

    public void GrabRightWristWithLeftHand()
    {
        MovePoint grabber = _movePoints.Find(x => x.EffectorType == FullBodyBipedEffector.LeftHand);
        AttachPoint target = _opponent.GetComponent<DummyBehaviour>().AttachPoints.Find
            (x => (x.Location == AttachLocation.Wrist) && (x.Side == AttachSide.Right) && (x.Depth == AttachDepth.Front));
        if (Grab(grabber, target))
        {
            SetState(States.Find(x => x is IdleState));
        }
    }

    //TODO: Push() and Pull() /*like grab but no target on opponent's body*/
    private bool Grab(MovePoint grabber, AttachPoint target)
    {
        bool reachedTarget = Move(grabber, target.AttachObject.transform);
        if (reachedTarget)
        {
            DebugLog("Reached target Player " + PlayerNumber);
            target.AttachedTo = grabber; 
        }
        return reachedTarget;
    }

	private bool Move(MovePoint movePoint, Transform target)  
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

	private void Pose(Transform targets)  
    {
        foreach (MovePoint mp in _movePoints)
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