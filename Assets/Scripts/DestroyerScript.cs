using UnityEngine;
using System.Collections;

public class DestroyerScript : MonoBehaviour
{

    void OnTriggerExit2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }
}
