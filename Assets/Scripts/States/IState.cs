using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IState  
{
    void TransitionTo(IState newState);
    void TransitionFrom(IState oldState);
    void Update();
}








