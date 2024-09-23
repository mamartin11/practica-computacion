using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    private Vector3 respawnPosition;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Cursor.visible = false; //Oculta el cursor
        Cursor.lockState = CursorLockMode.Locked; //Mantiene el cursor en la pantalla
        respawnPosition = PlayerController.instance.transform.position;
    }

    void Update()
    {
        
    }

    public void Respawn()
    {
        PlayerController.instance.gameObject.SetActive(false);
        PlayerController.instance.transform.position = respawnPosition;
        PlayerController.instance.gameObject.SetActive(true);
    }
}
