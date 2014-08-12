using UnityEngine;
using System.Collections;

public interface IMovementManager 
{
    void Update();
    void Pose(Transform targets);
    bool Move(MovePoint movePoint, Transform target);
    bool Grab(MovePoint grabber, AttachPoint target);	
}
