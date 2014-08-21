using UnityEngine;
using System.Collections;

public interface IMovementManager 
{
    void Update();
    void Pose(Transform targets);
    bool Move(BodyPoint movePoint, Transform target);
    //bool Grab(MovePoint grabber, AttachPoint target);	
}
