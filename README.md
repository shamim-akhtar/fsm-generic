<p align='left'>
  <a href="#">
    <img src="https://visitor-badge.glitch.me/badge?page_id=fsm-generic.visitor-badge" />        
  </a>
  <a href="https://www.linkedin.com/in/shamim-akhtar/">
    <img src="https://img.shields.io/badge/linkedin-%230077B5.svg?&flat-square&logo=linkedin&logoColor=white" />
  </a>
  <a href="mailto:shamim.akhtar@gmail.com">
    <img src="https://img.shields.io/badge/Gmail-D14836?flat-square&logo=gmail&logoColor=white" />        
  </a>
  <a href="https://www.facebook.com/faramiraSG/">
    <img src="https://img.shields.io/badge/Facebook-1877F2?flat-square&logo=facebook&logoColor=white" />        
  </a>
</p>

# Generic Finite State Machine Using C#
![](https://faramira.com/wp-content/uploads/2020/05/FSM-930x620.jpg)

Our objective for this tutorial is to make our Finite State Machine generic by using a generic identifier for the state type.

There are several ways you can implement a finite state machine using C#. The easiest and fastest way probably is to use the enumerator type and the switch-case statement. However, in this tutorial, we are not going to do that. Instead, we will use a slightly sophisticated, more robust, generic class-based approach that will be reusable across multiple projects.

<p align='left'>
  <a href="#">
    <img src="https://img.shields.io/badge/Unity-2020.3.5f1-green" />        
  </a>
  <a href="#">
    <img src="https://img.shields.io/badge/%20-C%23-blue" />
  </a>
</p>

At the same time, we also want to extend the functionality of using delegates in the same framework.
But first, let’s recap on what is a Finite State Machine.

![Turnstile Wikipedia](https://faramira.com/wp-content/uploads/2021/06/image-61.png)

## Definition
Finite State Machine (or FSM in short) is a **computational pattern** that defines and models state behaviour.
At any given time, a **Finite State Machine can exist in only one State out of a set of a possible number of states**. This State can change to another in response to some inputs (sometimes called events).

The process of switching from one State to another is called a **transition**.

![](https://faramira.com/wp-content/uploads/2020/08/image-36.png)


## The Classes
For organization purposes, we will put the generic reusable codes in the Patterns namespace. You can put them in any other namespace as well.

### The State Class
The State class is the base class for a Finite State Machine state. This is a data structure (class) that encapsulates the state-related functionalities. We will implement this class in the section after FiniteStateMachine class implementation. For now, we just define the basic structure.

```csharp
namespace Patterns
{
    public class State<T>
    {
        // The name for the state.
        public string Name { get; set; }
 
        // The ID of the state.
        public T ID { get; private set; }        
        public State(T id)
        {
            ID = id;
        }
        public State(T id, string name) : this(id)
        {
            Name = name;
        }
    }
}
```
We have added two constructors. One takes in the type T (unique ID) as a parameter, and the other takes in the type T (unique ID of the State) and a string value (name of the State) as parameters.

### The Finite State Machine

As defined above, a Finite State Machine

> consists of a set of states, 
> 
> and at any given time, a Finite State Machine can exist in only one State out of these possible states. 

Thus, we will need a variable to store the collection of states. This collection will represent a set of states. And then we will need a variable to keep the current state of the Finite State Machine.

```csharp
// A Finite State Machine
//    - consists of a set of states,
//    - and at any given time, an FSM can exist in only one 
//      State out of these possible set of states.
 
// A dictionary to represent the set of states.
protected Dictionary<T, State<T>> mStates;
 
// The current state.
protected State<T> mCurrentState;
```

To construct the FiniteStateMachine class, we probably won’t need any arguments. At least, not for now. We will proceed with a default constructor.

```csharp
public FiniteStateMachine()
{
    mStates = new Dictionary<T, State<T>>();
}
```

#### Add State to the Finite State Machine
In the previous section, we created the variable that stores the set of states. Now we will create a method that will fill that set by adding state.  

```csharp
public void Add(State<T> state)
{
    mStates.Add(state.ID, state);
}
 
public void Add(T stateID, State<T> state)
{
    mStates.Add(stateID, state);
}
```

#### Get State from the Finite State Machine
A method that returns a State based on the key.

```csharp
public State<T> GetState(T stateID)
{
    if(mStates.ContainsKey(stateID))
        return mStates[stateID];
    return null;
}
```

Note that the method will return null if a State of the same key has not been added previously to the FSM. This method is a convenient function.

#### Set the current State to the Finite State Machine
Now perhaps the most critical function of the Finite State Machine, SetCurrentState. This method will set the current state of the Finite State Machine

What happens when we set a state to the current State? There are two possible code paths to it. The first code path is when the previous-current State is valid, and the second code path is when the previous-current state is invalid (or null). 

```csharp
public void SetCurrentState(State<T> state)
{
    if (mCurrentState != null)
    {
    }
 
    mCurrentState = state;
}
```

The above code implements the SetCurrentState method. If the previous-current State of Finite State Mchine is invalid, then the implementation directly sets the State to the mCurrentState. However, if the previous-current State was not null, then what happens?

Can we still overwrite the previous-current state with the new current state?

The answer is probably not. We might want to implement specific functions whenever a state exits and a new state enters. How do we then implement this into our current code?

##### Enter and Exit
The answer is simple. Create two virtual methods in the State class called Enter and Exit. The base State class implements nothing for both the Enter and Exit methods and instead relies on the application to create concrete implementations of the base State class. Then call these two methods whenever there is a change in the State.

```csharp
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
```

We will finally put the Unity context to the FSM and the State class by adding two methods called Update and FixedUpdate. These two methods we will call from Unity Monobehavior for every Update and FixedUpdate.

```csharp
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
```

This completes our implementation of a Finite State Machine in C#. We will now continue by completing the State class.

### Completing the State Class
We have four key function calls. These are Enter, Exit, Update and FixedUpdate. We will keep these methods as virtual.

```csharp
virtual public void Enter()
{
}
virtual public void Exit()
{
}
virtual public void Update()
{
}
virtual public void FixedUpdate()
{
}
```
For convenience, we will add delegates to handle the key function calls such as Enter, Exit, Update and FixedUpdate.

```csharp
public delegate void DelegateNoArg();
 
public DelegateNoArg OnEnter;
public DelegateNoArg OnExit;
public DelegateNoArg OnUpdate;
public DelegateNoArg OnFixedUpdate;
```

Now we amend the four key functions as below by calling the respective delegate is valid.

```csharp
virtual public void Enter()
{
    OnEnter?.Invoke();
}
 
virtual public void Exit()
{
    OnExit?.Invoke();
}
virtual public void Update()
{
    OnUpdate?.Invoke();
}
 
virtual public void FixedUpdate()
{
    OnFixedUpdate?.Invoke();
}
```

Finally, we add two more constructors so that we can construct a State class with the given delegates as parameters.

```csharp
public State(T id,
    DelegateNoArg onEnter,
    DelegateNoArg onExit = null,
    DelegateNoArg onUpdate = null,
    DelegateNoArg onFixedUpdate = null) : this(id)
{
    OnEnter = onEnter;
    OnExit = onExit;
    OnUpdate = onUpdate;
    OnFixedUpdate = onFixedUpdate;
}
public State(T id, 
    string name,
    DelegateNoArg onEnter,
    DelegateNoArg onExit = null,
    DelegateNoArg onUpdate = null,
    DelegateNoArg onFixedUpdate = null) : this(id, name)
{
    OnEnter = onEnter;
    OnExit = onExit;
    OnUpdate = onUpdate;
    OnFixedUpdate = onFixedUpdate;
}
```

We have implemented a generic reusable Finite State Machine that we can reuse/override and apply based on what is required by our application domain down the stream. We can also use delegates to do the provide the necessary behaviour of a state without deriving a new State class.


This tutorial is an extension of my past tutorial on Implementing a Finite State Machine Using C# in Unity.

To read more about Finite State Machine please refer to my other series of tutorials on Finite State Machine.

> [Part 1: Implementing a Finite State Machine Using C# in Unity](https://faramira.com/implementing-a-finite-state-machine-using-c-in-unity-part-1/)
> 
> [Part 2: Implement a Splash Screen Using a Finite State Machine in Unity](https://faramira.com/implement-a-splash-screen-using-a-finite-state-machine-in-unity/)
> 
> [Part 3: Player Controls With Finite State Machine Using C# in Unity](https://faramira.com/implementing-player-controls-with-finite-state-machine-using-c-in-unity/)
> 
> [Part 4: Finite State Machine Using C# Delegates in Unity](https://faramira.com/finite-state-machine-using-csharp-delegates-in-unity/)
> 
> [Part 5: Enemy Behaviour With Finite State Machine Using C# Delegates in Unity](https://faramira.com/enemy-behaviour-with-finite-state-machine-using-csharp-delegates-in-unity/)
