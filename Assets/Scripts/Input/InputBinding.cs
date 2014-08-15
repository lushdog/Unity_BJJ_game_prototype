using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO: support more than 8 joystick directions
public enum Control {Up, Down, Left, Right, LeftArm, RightArm, LeftLeg, RightLeg, LeftMod1, RightMod1, LeftMod2, RightMod2, L3, R3, Select, Start}


public abstract class InputBinding 
{
    public Dictionary<Control, string> Inputs;

    public abstract bool IsInputActive(Control ctrl);

    protected void BindInput(Control control, string inputName)
    {
        if (Inputs.ContainsKey(control))
        { 
            Inputs[control] = inputName;
        }
        else
        {
            Inputs.Add(control, inputName);
        }
    }

    //TODO: load existing inputs from file or if file does not exist then load defaults
    protected InputBinding()
    {
        Inputs = new Dictionary<Control, string>();
    }	
}

public class KeyboardInputBinding : InputBinding
{
    public KeyboardInputBinding(int playerNumber) : base ()
    {
        foreach (Control ctrl in System.Enum.GetValues(typeof(Control)))
        {
            if (ctrl == Control.RightArm)
                Inputs.Add(ctrl, "space");
        }
    }

    public override bool IsInputActive(Control ctrl) 
    {
        string key = Inputs[ctrl];
        return Input.GetKeyDown(key);
    }

}


