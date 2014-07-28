using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IState  
{
    List<IState> SubStates 
    {
        get;
        set;
    } 

    void OnEnterState(); 
    void InState();
    void ExitState();
}

public class IdleState : IState
{
    public void OnEnterState ()
    {
        throw new System.NotImplementedException ();
    }

    public void InState ()
    {
        throw new System.NotImplementedException ();
    }

    public void ExitState ()
    {
        throw new System.NotImplementedException ();
    }
}

public class GrabbingState : IState
{
    public void OnEnterState ()
    {
        throw new System.NotImplementedException ();
    }

    public void InState ()
    {
        throw new System.NotImplementedException ();
    }

    public void ExitState ()
    {
        throw new System.NotImplementedException ();
    }
}