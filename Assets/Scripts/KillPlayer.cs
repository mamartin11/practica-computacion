using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
   private void OnTriggerEnter(Collider other) //Definiendo zona de muerte
   {
    if(other.tag == "Player") //Si el player colisiona contra este objeto, muere
    {
        GameManager.instance.Respawn();
    }
   }
}
