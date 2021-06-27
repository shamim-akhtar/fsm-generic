using System;

namespace Patterns
{
    class Program
    {
        //Example: coin-operated turnstile
        public enum TurnstileStates
        {
            LOCKED,
            UNLOCKED,
        }

        // Delegate based FSM test for type as GameStates
        static void FSM_Delegates_Test()
        {
            FiniteStateMachine<TurnstileStates> turnstile_fsm = 
                new FiniteStateMachine<TurnstileStates>();

            turnstile_fsm.Add(
                new State<TurnstileStates>(
                    TurnstileStates.LOCKED,
                    "Locked State", 
                    OnEnterLockedState,
                    OnExitLockedState,
                    OnUpdateLockedState
                ));

        }

        static void OnEnterLockedState()
        {
            Console.WriteLine("Entered Locked State. Press numeric key 5 to insert 50 cents and unlock");
        }
        static void OnExitLockedState()
        {
            Console.WriteLine("Exiting Locked State.");
        }
        static void OnUpdateLockedState()
        {
            Console.WriteLine("In locked state");
        }


        static void Main(string[] args)
        {
            FSM_Delegates_Test();
        }
    }
}
