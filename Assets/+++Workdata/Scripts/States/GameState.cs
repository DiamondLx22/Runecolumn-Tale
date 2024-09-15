using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static event Action StateChanged;
    public static event Action<string, int> StateAdded;
    
    //[SerializeField]
    //private InventorySlot inventorySlot;

    #region Inspector

    [SerializeField] public List<State> states;

    //public StateInfo[] stateInfos;

    #endregion

    public State Get(string id)
    {
        foreach (State state in states)
        {
            if (state.id == id)
            {
                return state;
            }
        }

        return null;
    }

    public void Add(string id, int amount, bool invokeEvent = true)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            Debug.LogError("Id of state is empty. Make sure to give each state an id.", this);
            return;
        }

        if (amount == 0)
        {
            Debug.LogWarning($"Trying to add 0 to id '{id}'. This will result in to change to the state.", this);
            return;
        }

        State state = Get(id);

        if (state == null)
        {
            State newState = new State(id, amount);
            states.Add(newState);
        }
        else
        {
            state.amount += amount;
        }

        if (invokeEvent)
        {
            StateChanged?.Invoke();
            StateAdded?.Invoke(id, amount);
        }

        StartCoroutine(CheckItems());
    }

    private bool isCoroutineRunning = false;
    IEnumerator CheckItems()
    {
        if (isCoroutineRunning)
        {
            yield break; // Coroutine abbrechen, wenn sie bereits läuft
        }
        isCoroutineRunning = true;
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].amount == 0)
            {
                states.RemoveAt(i);
                i--;
            }
        }
        isCoroutineRunning = false;
    }


    public void Add(State state, bool invokeEvent = true)
    {
        Add(state.id, state.amount, invokeEvent);
    }


    public void Add(List<State> states)
    {
        foreach (State state in states)
        {
            Add(state, false);
            //StateAdded?.Invoke(state.id, state.amount);
        }
        StateChanged?.Invoke();
    }

    public List<State> GetStateList()
    {
        return states;
    }

    //#todo Check Conditions 

    /// <summary>
    /// Check <paramref name="conditions"/> against the game <see cref="states"/>. All conditions are implicitly AND connected.
    /// A condition passes if the value of the <see cref="State"/> in the game <see cref="states"/> is equal or higher than the value of the condition.
    /// </summary>
    /// <param name="conditions">List of conditions to check.</param>
    /// <returns>If all <paramref name="conditions"/> passed.</returns>
    public bool CheckConditions(List<State> conditions)
    {
        // Check each condition in the list of conditions.
        foreach (State condition in conditions)
        {
            // Get the state with the same id as the condition.
            State state = Get(condition.id);
            // Extract the value if the state exists; otherwise 0.
            int stateAmount = state != null ? state.amount : 0;
            // Compare the value of the state against the value of the condition.
            if (stateAmount < condition.amount)
            {
                // Return immediately false if any condition fails.
                // We do not need to check any conditions after that (Short-circuit).
                return false;
            }
        }

        // If we passed through the loop without ever returning false,
        // we reach the end of the function and can confidently return true.
        return true;
    }
}

