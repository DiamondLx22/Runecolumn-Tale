 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestItemSpawnScript : MonoBehaviour
{
    public GameObject itemPrefab;  // Das Item, das in der Truhe enthalten ist
    public Transform spawnPoint;   // Der Punkt, an dem das Item erscheinen soll

    private bool isOpen = false;   // Status, ob die Truhe geöffnet ist

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Überprüft, ob der Spieler die Truhe berührt
        if (other.CompareTag("Player") && !isOpen)
        {
            OpenChest();  // Öffnet die Truhe
        }
    }

    private void OpenChest()
    {
        isOpen = true;  // Setzt den Status der Truhe auf geöffnet

        // Erzeugt das Item an der angegebenen Position
        Instantiate(itemPrefab, spawnPoint.position, spawnPoint.rotation);

     
    }
}
