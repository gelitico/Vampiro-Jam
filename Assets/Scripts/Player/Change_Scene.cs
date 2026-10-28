using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class Change_Scene : MonoBehaviour
{
    //serialize para que variable privada sea visible y editable desde en el inspector, pero no publica
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
            if (hit.transform == this.transform) // si contra lo que choca (coinciden los transform) el rayo es este objeto (el que tiene el script) 
            {
                return true; //saca true (estamirandoalojeto)
            }
        }
        return false; //si no hace lo de arriba no choco
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // coge algo que tenga la etiqueta de player (osea el player)
        {
            jugadorCerca = true; //dice que esta dentro
            jugadorRef = other.gameObject; // se guarda la referencia la jugador
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
        if(!yaClickado) //solo hace la logica si no hay una peticion activa procesandose en el momento (si no se ha clicado)
        {
        if (jugadorRef != null) //si existe
        {
            // el jugador no se destruye cuando cambia de escena
            DontDestroyOnLoad(jugadorRef); 

            // conseguir componentes de movimiento para pararlo y se guarda 
            CharacterController controller = jugadorRef.GetComponent<CharacterController>();
            PlayerInput inputSystem = jugadorRef.GetComponent<PlayerInput>();
            Rigidbody rb = jugadorRef.GetComponent<Rigidbody>();

            // se desactivan los componentes para que no pueda interactuar ni moverse
            if (controller != null) controller.enabled = false;

            if (inputSystem != null) inputSystem.enabled = false;

            if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero; //velocidad a cero del rigidbody en todas direcciones
                    rb.isKinematic = true; //no le afecta la gravedad ni las fisicas
                }

                // este script sobrevive temporalmente porque no se destruye el jug para encargarse de descongelar todo en la nueva escena
                DontDestroyOnLoad(this.gameObject);

                // inicia la cuenta atras pasando el jugador como parametro (para dejarlo quieto)
                StartCoroutine(TemporizadorCongelarJuego(segundosCongelado, jugadorRef));
        }

        // cargamos nueva escena
        SceneManager.LoadScene(nombreEscenaAInvocarse);
        yaClickado = false; //saca que no se ha clicado para reutilizar el codigo luego
        }
    }

    private IEnumerator TemporizadorCongelarJuego(float tiempoEspera, GameObject jugador)
    {
        // espera un fotograma, en el qu
        yield return null;

        // congelar tiempo
        Time.timeScale = 0f;

        // WaitForSecondsRealtime porque WaitForSeconds normal no avanza si timeScale es 0
        yield return new WaitForSecondsRealtime(tiempoEspera);

        Time.timeScale = 1f;

        // devolver mov a jugador
        if (jugador != null)
        {
            CharacterController controller = jugador.GetComponent<CharacterController>();
            PlayerInput inputSystem = jugador.GetComponent<PlayerInput>();
            Rigidbody rb = jugador.GetComponent<Rigidbody>();

            if (controller != null) controller.enabled = true;
            if (inputSystem != null) inputSystem.enabled = true;
            if (rb != null) rb.isKinematic = false;
        }

        // busca nueva camara principal de la escena recien cargada (de momento no hace falta)
        camaraPrincipal = Camera.main;

        // destruye este objeto, solo se usa 1 vez
        Destroy(this.gameObject);
    }
}