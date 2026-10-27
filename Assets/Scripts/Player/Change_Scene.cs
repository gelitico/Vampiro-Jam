using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class Change_Scene : MonoBehaviour
{
    //serialize para que variable privada sea visible y editable desde en el inspector, pero no pública
    [SerializeField] private string nombreEscenaAInvocarse;

    //variable para que el raycast solo detecte objetos que pertenezcan a x capas (p.ej la del ammo o el timbre)
    public LayerMask capaInteractuable; 
    public float distanciaMaximaRayo = 3f; 

    [SerializeField] private float segundosCongelado = 5f;

    //variables para controlar jugador y camara
    private bool jugadorCerca = false; 
    private GameObject jugadorRef; 
    private Camera camaraPrincipal; 

    //---------------------------------------------MANO
    private ControlManoUI manoUI;
    private bool estabaMirando = false;

    private bool yaClickado=false;

   
    void Start()
    {
        camaraPrincipal = Camera.main;

        manoUI = FindObjectOfType<ControlManoUI>();
    }

    void Update()
    {
        bool mirandoAhora = false;
        if (jugadorCerca)
        {
            mirandoAhora = EstaMirandoAlObjeto(); //hace el raycast

            if (mirandoAhora && !estabaMirando) //si lo mira ahora, pero en el fotograma anterior no (por eso !) lo estaba mirando
            {
                if (manoUI != null)
                {
                    manoUI.ActivarAccion("lanzar"); 
                }
            }
        }

        //se guarda el estado actual para que sirva de comparativa en el proximo fotograma en el bucle
        estabaMirando = mirandoAhora;
        //si el jugador esta cerca y presiona el clic izquierdo
        if (jugadorCerca && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // y esta mirando directamente al objeto interactuable
            if (EstaMirandoAlObjeto())
            {   if (manoUI != null)
                {
                    manoUI.ActivarAccion("clicar");
                }

                IniciarCambioEscena();
                yaClickado=true;
            }
        }
    }

    private bool EstaMirandoAlObjeto()
    {
        //si la camara se destruye o no existe, cancela todo
        if (camaraPrincipal == null) return false;

        //crea rayo que sale en la camara y apunta hacia el centro del viewport (por eso 0.5, 0.5)
        Ray rayo = camaraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //el motor de fisica guarda el choque (distancia, punto de impacto) y se guarda en el "hit"
        RaycastHit hit;

        // lanza el rayo al espacio 3D usando la distancia maxima que se declaro anres y con el filtro de la capa interactuable
        if (Physics.Raycast(rayo, out hit, distanciaMaximaRayo, capaInteractuable))
        {
            if (hit.transform == this.transform)
            {
                return true; 
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            jugadorCerca = true;
            jugadorRef = other.gameObject; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            jugadorRef = null;
        }
    }

    private void IniciarCambioEscena()
    {
        if(!yaClickado)
        {
        if (jugadorRef != null)
        {
            // Evitamos que el jugador se destruya al cambiar de escena
            DontDestroyOnLoad(jugadorRef); 

            // Conseguimos sus componentes de movimiento
            CharacterController controller = jugadorRef.GetComponent<CharacterController>();
            PlayerInput inputSystem = jugadorRef.GetComponent<PlayerInput>();
            Rigidbody rb = jugadorRef.GetComponent<Rigidbody>();

            // Desactivamos sus componentes para que no pueda interactuar ni moverse
            if (controller != null) controller.enabled = false;
            if (inputSystem != null) inputSystem.enabled = false;
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            // Hacemos que este script sobreviva temporalmente para encargarse de descongelar todo en la nueva escena
            DontDestroyOnLoad(this.gameObject);

            // Iniciamos la cuenta atrás pasando el jugador como parámetro
            StartCoroutine(TemporizadorCongelarJuego(segundosCongelado, jugadorRef));
        }

        // Cargamos la nueva escena de forma inmediata
        SceneManager.LoadScene(nombreEscenaAInvocarse);
        yaClickado=false;
        }
    }

    private IEnumerator TemporizadorCongelarJuego(float tiempoEspera, GameObject jugador)
    {
        // 1. ESPERA UN FOTOGRAMA: Nos aseguramos de que Unity ya esté cargando/cargado la nueva escena
        yield return null;

        // 2. CONGELAMOS EL TIEMPO GLOBAL DEL JUEGO
        Time.timeScale = 0f;
        Debug.Log($"Juego congelado en la nueva escena. Esperando {tiempoEspera} segundos...");

        // 3. ESPERA LOS SEGUNDOS ESPECIFICADOS
        // Usamos WaitForSecondsRealtime porque WaitForSeconds normal no avanza si Time.timeScale es 0
        yield return new WaitForSecondsRealtime(tiempoEspera);

        // 4. DESCONGELAMOS EL TIEMPO GLOBAL DEL JUEGO
        Time.timeScale = 1f;

        // 5. DEVOLVEMOS EL MOVIMIENTO AL JUGADOR
        if (jugador != null)
        {
            CharacterController controller = jugador.GetComponent<CharacterController>();
            PlayerInput inputSystem = jugador.GetComponent<PlayerInput>();
            Rigidbody rb = jugador.GetComponent<Rigidbody>();

            if (controller != null) controller.enabled = true;
            if (inputSystem != null) inputSystem.enabled = true;
            if (rb != null) rb.isKinematic = false;

            Debug.Log("¡Tiempo de espera finalizado! Todo vuelve a la normalidad.");
        }

        // Buscamos la nueva cámara principal de la escena recién cargada para futuros comportamientos
        camaraPrincipal = Camera.main;

        // Destruimos este objeto del botón interactuable que ya cumplió su función
        Destroy(this.gameObject);
    }
}