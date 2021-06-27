using System;
using System.Collections.Generic;
using System.Text;

namespace Patterns
{
    public class FiniteStateMachine<T>
    {
        protected Dictionary<T, State<T>> mStates;
        protected State<T> mCurrentState;

        public FiniteStateMachine()
        {
            mStates = new Dictionary<T, State<T>>();
        }

        public void Add(State<T> state)
        {
            mStates.Add(state.ID, state);
        }

        public void Add(T stateID, State<T> state)
        {
            mStates.Add(stateID, state);
        }

        public State<T> GetState(T stateID)
        {
            return mStates[stateID];
        }

        public void SetCurrentState(T stateID)
        {
            State<T> state = mStates[stateID];
            SetCurrentState(state);
        }

        public State<T> GetCurrentState()
        {
            return mCurrentState;
        }

        public void SetCurrentState(State<T> state)
        {
            if (mCurrentState == state)
            {
                return;
            }

            if (mCurrentState != null)
            {
                mCurrentState.Exit();
            }

            mCurrentState = state;

            if (mCurrentState != null)
            {
                mCurrentState.Enter();
            }
        }

        public void Update()
        {
            if (mCurrentState != null)
            {
                mCurrentState.Update();
            }
        }

        public void FixedUpdate()
        {
            if (mCurrentState != null)
            {
                mCurrentState.FixedUpdate();
            }
        }
    }
}
