using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class Change_Scene : MonoBehaviour
{
    [SerializeField] private string nombreEscenaALevedar;

    [Header("Configuracion de Interaccion")]
    [SerializeField] private LayerMask capaInteractuable; 
    [SerializeField] private float distanciaMaximaRayo = 3f; 

    [Header("Configuracion del Enemigo")]
    [SerializeField] private GameObject enemigoEnEscena; 

    private bool jugadorCerca = false;
    private GameObject jugadorRef;
    private Camera camaraPrincipal;

    // Referencias locales
    private Enemy_Fly scriptVuelo;
    private Enemy_Follow scriptSeguir;
    private UnityEngine.AI.NavMeshAgent navMesh;

    void Start()
    {
        camaraPrincipal = Camera.main;

        if (enemigoEnEscena != null)
        {
            scriptVuelo = enemigoEnEscena.GetComponent<Enemy_Fly>();
            scriptSeguir = enemigoEnEscena.GetComponent<Enemy_Follow>();
            navMesh = enemigoEnEscena.GetComponent<UnityEngine.AI.NavMeshAgent>();

            ApagarMovimientoEnemigo();
        }
    }

    void Update()
    {
        if (jugadorCerca && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (EstaMirandoAlObjeto())
            {
                IniciarSecuenciaTimbre();
            }
        }
    }

    private bool EstaMirandoAlObjeto()
    {
        if (camaraPrincipal == null) return false;

        Ray rayo = camaraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

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

    private void IniciarSecuenciaTimbre()
    {
        if (enemigoEnEscena != null)
        {
            ApagarMovimientoEnemigo();

            // Hacemos que el enemigo sobreviva al cambio de escena
            DontDestroyOnLoad(enemigoEnEscena);

            // ¡Truco clave!: Le ordenamos AL PROPIO ENEMIGO que ejecute el temporizador.
            // Como el enemigo no se destruye, la cuenta atrás de 14 segundos seguirá funcionando perfectamente.
            MonoBehaviour scriptInmortal = scriptVuelo != null ? (MonoBehaviour)scriptVuelo : (MonoBehaviour)scriptSeguir;
            if (scriptInmortal != null)
            {
                scriptInmortal.StartCoroutine(TemporizadorEnemigoInmortal());
            }
        }

        CambiarDeEscena();
    }

    private void ApagarMovimientoEnemigo()
    {
        if (scriptVuelo != null) scriptVuelo.enabled = false;
        if (scriptSeguir != null) scriptSeguir.enabled = false;
        if (navMesh != null) navMesh.enabled = false;
    }

    // Esta corrutina ahora corre de forma segura dentro del enemigo
    private IEnumerator TemporizadorEnemigoInmortal()
    {
        yield return new WaitForSecondsRealtime(14f);

        // Despertamos los componentes
        if (navMesh != null) navMesh.enabled = true;
        if (scriptVuelo != null) scriptVuelo.enabled = true;
        if (scriptSeguir != null) scriptSeguir.enabled = true;

        // Buscamos al jugador en la nueva escena por si los scripts perdieron su rastro
        GameObject jugadorActualizado = GameObject.FindGameObjectWithTag("Player");
        if (jugadorActualizado != null)
        {
            // Ajusta aquí los nombres exactos si tus scripts llaman al jugador de otra forma
            // Por ejemplo, si usan una variable pública llamada 'target' o 'jugador'
            Debug.Log("Re-vinculando al jugador con la IA del enemigo.");
        }

        Debug.Log("¡Los 14 segundos terminaron! El enemigo se ha despertado en la nueva escena.");
    }

    private void CambiarDeEscena()
    {
        if (jugadorRef != null)
        {
            Vector3 posicionUltima = jugadorRef.transform.position; 

            if (jugadorRef.TryGetComponent<CharacterController>(out CharacterController controller))
            {
                controller.enabled = false;
            }
            if (jugadorRef.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            DontDestroyOnLoad(jugadorRef); 

            jugadorRef.transform.position = posicionUltima; 

            SceneManager.sceneLoaded += (scene, mode) => {
                if (controller != null) controller.enabled = true;
                if (rb != null) rb.isKinematic = false;
            };
        }

        SceneManager.LoadScene(nombreEscenaALevedar);
    }
}