using UnityEngine;

public class CarStateMachine : MonoBehaviour
{
    public enum CarState
    {
        Stable,
        Moving
    }
    public CarState currentState { get; private set; } = CarState.Stable;

    // Method to safely change state with optional side effects
    public void SetState(CarState newState)
    {
        if (newState != currentState)
        {
            currentState = newState;
            Debug.Log("The New State is " + newState);
        }
    }
}
