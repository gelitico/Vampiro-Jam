using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class Change_Scene : MonoBehaviour
{
    // Nombre de la escena a la que vamos a ir.
    [SerializeField] private string nombreEscenaAInvocarse;

    [Header("Configuracion de Interaccion")]
    public LayerMask capaInteractuable; // Capa para que el Raycast detecte solo cosas interactivas
    public float distanciaMaximaRayo = 3f; // Qué tan lejos llega nuestro rayo para tocar el timbre

    [Header("Configuracion del Enemigo")]
    public GameObject enemigoEnEscena; // El enemigo que queremos arrastrar a la otra escena

    // Variables de control internas
    private bool jugadorCerca = false; // Indica si el jugador entró al Trigger del timbre
    private GameObject jugadorRef; // Guarda la referencia del jugador que tocó el timbre
    private Camera camaraPrincipal; // Atajo para la cámara

    // Guardamos los componentes del enemigo para apagarlos y prenderlos
    private Enemy_Fly scriptVuelo;
    private Enemy_Follow scriptSeguir;
    private UnityEngine.AI.NavMeshAgent navMesh;

    // Guardamos los componentes de movimiento del jugador de forma estática para que no se pierdan al cambiar de escena
    private static CharacterController jugadorController;
    private static Rigidbody jugadorRigidbody;
    private static PlayerInput jugadorInputSystem; 

    void Start()
    {
        // Buscamos la cámara principal de la escena al empezar para usar el Raycast
        camaraPrincipal = Camera.main;

        // Si arrastramos un enemigo al inspector, le buscamos sus scripts de movimiento
        if (enemigoEnEscena != null)
        {
            scriptVuelo = enemigoEnEscena.GetComponent<Enemy_Fly>();
            scriptSeguir = enemigoEnEscena.GetComponent<Enemy_Follow>();
            navMesh = enemigoEnEscena.GetComponent<UnityEngine.AI.NavMeshAgent>();

            // Nos aseguramos de que empiece apagado si es necesario
            ApagarMovimientoEnemigo();
        }
    }

    void Update()
    {
        // Si el jugador está cerca y presiona la tecla E...
        if (jugadorCerca && Keyboard.current.eKey.wasPressedThisFrame)
        {
            // ... y además está mirando directamente al timbre
            if (EstaMirandoAlObjeto())
            {
                IniciarSecuenciaTimbre();
            }
        }
    }

    // Método que lanza un rayo invisible desde el centro de la pantalla para ver si miramos el timbre
    private bool EstaMirandoAlObjeto()
    {
        if (camaraPrincipal == null) return false;

        // Crea un rayo justo en el medio de la pantalla (0.5, 0.5)
        Ray rayo = camaraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Si el rayo choca con algo en la capa interactuable...
        if (Physics.Raycast(rayo, out hit, distanciaMaximaRayo, capaInteractuable))
        {
            // ... y ese algo es este mismo objeto (el timbre)
            if (hit.transform == this.transform)
            {
                return true; // ¡Sí lo estamos mirando!
            }
        }
        return false;
    }

    // Detecta cuando el jugador entra en el área del timbre
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            jugadorCerca = true;
            jugadorRef = other.gameObject; // Guardamos al jugador para usarlo luego
        }
    }

    // Detecta cuando el jugador se aleja del timbre
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            jugadorRef = null;
        }
    }

    // Arranca todo el proceso cuando tocamos el timbre
    private void IniciarSecuenciaTimbre()
    {
        if (enemigoEnEscena != null)
        {
            // Apagamos la IA del enemigo para que no camine durante la carga
            ApagarMovimientoEnemigo();

            // Hace que el enemigo no se destruya al borrar la escena actual
            DontDestroyOnLoad(enemigoEnEscena);

            // Buscamos cuál script tiene el enemigo para colgarle la Corrutina del contador
            MonoBehaviour scriptInmortal = scriptVuelo != null ? (MonoBehaviour)scriptVuelo : (MonoBehaviour)scriptSeguir;
            if (scriptInmortal != null)
            {
                // Iniciamos el contador de 14 segundos corriendo desde el enemigo
                scriptInmortal.StartCoroutine(TemporizadorEnemigoYJugador());
            }
        }

        // Llamamos al método que se lleva al jugador y cambia la escena
        CambiarDeEscena();
    }

    // Apaga los scripts de movimiento del enemigo
    private void ApagarMovimientoEnemigo()
    {
        if (scriptVuelo != null) scriptVuelo.enabled = false;
        if (scriptSeguir != null) scriptSeguir.enabled = false;
        if (navMesh != null) navMesh.enabled = false;
    }

    // La cuenta atrás de 14 segundos (Corrutina)
    private IEnumerator TemporizadorEnemigoYJugador()
    {
        // Espera 14 segundos reales (no le afecta si el tiempo del juego se pausa)
        yield return new WaitForSecondsRealtime(14f);

        // 1. PASADOS LOS 14 SEGUNDOS: DESPERTAMOS AL ENEMIGO
        if (navMesh != null) navMesh.enabled = true;
        if (scriptVuelo != null) scriptVuelo.enabled = true;
        if (scriptSeguir != null) scriptSeguir.enabled = true;

        // 2. DEVOLVER EL MOVIMIENTO AL JUGADOR
        if (jugadorController != null) jugadorController.enabled = true;
        if (jugadorRigidbody != null) jugadorRigidbody.isKinematic = false;
        if (jugadorInputSystem != null) jugadorInputSystem.enabled = true;

        Debug.Log("¡14 segundos terminaron! Controles de movimiento WASD devueltos.");
    }

    // Gestiona al jugador y carga el nuevo mapa
    private void CambiarDeEscena()
    {
        if (jugadorRef != null)
        {
            // Guardamos la posición actual por seguridad
            Vector3 posicionUltima = jugadorRef.transform.position; 

            // Conseguimos los componentes de movimiento del jugador
            jugadorController = jugadorRef.GetComponent<CharacterController>();
            jugadorRigidbody = jugadorRef.GetComponent<Rigidbody>();
            jugadorInputSystem = jugadorRef.GetComponent<PlayerInput>();

            // CONGELAMOS AL JUGADOR (Apagamos sus componentes físicos para que no camine)
            if (jugadorController != null) jugadorController.enabled = false;
            if (jugadorInputSystem != null) jugadorInputSystem.enabled = false;
            
            if (jugadorRigidbody != null)
            {
                jugadorRigidbody.linearVelocity = Vector3.zero; // Frenamos su fuerza acumulada
                jugadorRigidbody.isKinematic = true; // Lo hacemos inmune a la gravedad e impactos
            }

            // Evitamos que el jugador se borre al cambiar de escena
            DontDestroyOnLoad(jugadorRef); 
            jugadorRef.transform.position = posicionUltima; 
        }

        // Cargamos la escena solicitada
        SceneManager.LoadScene(nombreEscenaAInvocarse);
    }
}