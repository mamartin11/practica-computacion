using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float moveSpeed;  // Controla la velocidad del movimiento
    public float jumpForce;  // Controla la potencia del salto
    public float gravityScale = 5f;  // Escala de la gravedad para darle fluidez al salto
    private Vector3 moveDirection;  // Vector de movimiento en los ejes X,Y,Z

    public CharacterController charController;  // Comtrola el movimiento del personaje
    public Camera playerCamera;  // Seguimiento de la cámara al personaje
    public GameObject playerModel;
    public float rotateSpeed = 5f;
    public Animator animator;

    private bool Jumping = false;  // Para controlar el estado de salto
    private float VerticalVelocity;  // Velocidad vertical para controlar el salto
    private int jumpCount = 0;  // Contador de saltos
    public int maxJumps = 2;  // Número máximo de saltos (doble salto)

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Inicializamos el animator
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float yStore = moveDirection.y;  // Almacenar la posición y del personaje para calcular sobre esta la mecánica de los saltos
        moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal")); // Control del movimiento en el eje X y Z
        moveDirection.Normalize();
        moveDirection = moveDirection * moveSpeed;  // Para que la velocidad definida afecte el movimiento del personaje
        moveDirection.y = yStore;

        // Control del salto y gravedad
        if (charController.isGrounded) // Si el personaje está en el suelo
        {
            VerticalVelocity = -1f;  // Aplicar un pequeño valor negativo para mantener al personaje pegado al suelo
            Jumping = false;
            jumpCount = 0;  // Restablecer el contador de saltos

            if (Input.GetButtonDown("Jump"))  // Si la tecla de saltar se presiona
            {
                VerticalVelocity = jumpForce;  // Asignar la fuerza de salto
                Jumping = true;  // El personaje está saltando
                jumpCount++;  // Incrementar el contador de saltos
            }
        }
        else
        {
            // Permitir el doble salto si no está en el suelo y aún no ha realizado los dos saltos
            if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
            {
                VerticalVelocity = jumpForce;  // Asignar la fuerza de salto para el doble salto
                Jumping = true;  // El personaje está saltando de nuevo
                jumpCount++;  // Incrementar el contador de saltos
            }

            VerticalVelocity += Physics.gravity.y * gravityScale * Time.deltaTime;  // Aplicar la gravedad mientras no esté en el suelo
        }

        // Aplicar la velocidad vertical al movimiento
        moveDirection.y = VerticalVelocity;

        // Mover el personaje
        charController.Move(moveDirection * Time.deltaTime);

        // Rotación del personaje con respecto a la cámara cuando se mueve
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(0f, playerCamera.transform.rotation.eulerAngles.y, 0f);  // Hace que el personaje rote con la cámara solo cuando este se mueve
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));  // Permite realizar una mejor rotación del personaje respecto a la cámara
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        // Control de las animaciones según el estado
        animator.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));  // Pasando movimiento en X y Z para la animación de correr
        animator.SetBool("Grounded", charController.isGrounded);  // Animación de aterrizaje/idle
        animator.SetBool("Jumping", Jumping);  // Activar la animación de salto
        animator.SetFloat("VerticalVelocity", VerticalVelocity);  // Usar esto para determinar si el personaje está subiendo o bajando
    }
}
