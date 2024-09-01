using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    #region Inspektor
    
    [SerializeField] private State state;

    [SerializeField] private UnityEvent onCollected;

    #endregion

    public void Collect()
    {
        onCollected.Invoke();
        FindObjectOfType<GameState>().Add(state);
        Destroy(gameObject);
    }
}
