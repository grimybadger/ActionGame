using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
   // A Single State
    public class State
    {
        // The state's visible name. Also used to identify the state to the state machine
        public string name; 

        // Called every frame while the state is active
        public System.Action onFrame;   

        // Called when the state is transitioned to from another state
        public System.Action onEnter;

        // Called when the state is tranistioning to another state
        public System.Action onExit;
        
        public override string ToString() 
        {    
            return name;
        }       
    }
        
    // The collection of named states
    Dictionary<string, State> states = new Dictionary<string, State>();

    // The state that we are currently in 
    public State currentState{get; private set;}

    // The state that we will start in 
    public State initialState;

    // Creates, registers, and returns a new named state
    public State CreateState(string name)
    {    
        var newState = new State();

        // Give it a name
        newState.name = name;
        
        // If this is the first state, it will be our initial state
        if(states.Count == 0)
        {
            initialState = newState;
        }

        // Add it to the dictionary
        states[name] = newState;

        // And return it, so the that it can be further configured
        return newState;
    }
    
    public void Update()
    {
        // If we dont have any states to use, log the error
        if(states.Count == 0 || initialState == null)
        {
            Debug.LogErrorFormat("State Machine Has No States!"); 
            return; 
        }

        // If we don't currently have a state, transition to the inital state
        if(currentState == null)
        {
            TransitionTo(initialState);
        }

        // If the current state has no onFrame method, call it
        if (currentState.onFrame != null)
        {
            currentState.onFrame();
           // Debug.Log("Running OnFrame");
        }
    }

    // Transistions to the specified state
    public void TransitionTo(State newState)
    {
        //Ensure we weren't passed null
        if(newState == null)
        {
            Debug.LogErrorFormat("Cannot Transition To A Null State!");
            return;
        }

        // If we have a current state and that state has an onExit method, call it
        if(currentState != null && currentState.onExit != null)
        {
            currentState.onExit();
        }

        Debug.LogFormat("Transitioning from '{0}' to '{1}'",currentState, newState);

        // This is now our current state
        currentState = newState;

        // If the new state has an enter method, call it
        if(newState.onEnter != null )
        {
            newState.onEnter();
        }
    }


    // Transistions to a named state
    public void TransitionTo(string name)
    {
        if(states.ContainsKey(name) == false)
        {
            Debug.LogErrorFormat("State machine doesn't contain a state" + "named {0}!", name);
            return;
        }

        // Find the state in the dictionary 
        var newState = states[name];

        // Transition to it
        TransitionTo(newState);
    }
}
