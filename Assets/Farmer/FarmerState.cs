using UnityEngine;

public class FarmerState : MonoBehaviour
{
    public enum FarmerStates
    {
        HoldingItem,
        Idle
    }

    public FarmerStates currentState = FarmerStates.Idle;

    public bool CanHoldItem()
    {
        return currentState == FarmerStates.Idle;
    }

    public bool CanHighlightItem()
    {
        return currentState == FarmerStates.Idle;
    }

    public void SetState(FarmerStates newState)
    {
        currentState = newState;
    }
}