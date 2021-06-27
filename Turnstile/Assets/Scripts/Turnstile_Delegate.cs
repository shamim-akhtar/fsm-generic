using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;

public class Turnstile_Delegate : MonoBehaviour
{
    enum TurnstileStates
    {
        LOCKED,
        UNLOCKED,
    }
    FiniteStateMachine<TurnstileStates> mFsm = new FiniteStateMachine<TurnstileStates>();

    private void Start()
    {
        // Add the Locked state to the fsm
        mFsm.Add(
            new State<TurnstileStates>(
                TurnstileStates.LOCKED, 
                "Locked", 
                OnEnterLocked, 
                null,
                OnUpdateLocked, 
                null
                )
            );
        // Add the Unlocked state to the fsm
        mFsm.Add(
            new State<TurnstileStates>(
                TurnstileStates.UNLOCKED,
                "Unlocked",
                OnEnterUnlocked,
                null,
                OnUpdateUnlocked,
                null
                )
            );

        mFsm.SetCurrentState(TurnstileStates.LOCKED);
    }

    private void Update()
    {
        mFsm.Update();
    }

    private void FixedUpdate()
    {
        mFsm.FixedUpdate();
    }

    #region Delegates implementation for the states.
    void OnEnterLocked()
    {
        Debug.Log("Turnstile LOCKED. Press C key to insert a coin");
    }

    void OnUpdateLocked()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log("Turnstile unlocking");
                mFsm.SetCurrentState(TurnstileStates.UNLOCKED);
            }
            else
            {
                Debug.Log("Incorrect coin");
            }
        }
    }

    void OnEnterUnlocked()
    {
        Debug.Log("Turnstile UNLOCKED.");
    }


    void OnUpdateUnlocked()
    {
        if (Input.anyKeyDown)
        {
            if (!Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log("Turnstile locking");
                mFsm.SetCurrentState(TurnstileStates.LOCKED);
            }
        }
    }
    #endregion
}
