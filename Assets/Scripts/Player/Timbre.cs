using UnityEngine;
using UnityEngine.InputSystem;

public class Timbre : MonoBehaviour
{
    public LayerMask capaInteractuable; 
    public float distanciaMaximaRayo = 3f;

    private bool jugadorCerca = false; 
    private GameObject jugadorRef; 
    private Camera camaraPrincipal;

    private ControlManoUI manoUI;
    private bool estabaMirando = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camaraPrincipal = Camera.main;

        manoUI = FindObjectOfType<ControlManoUI>();
    }

    // Update is called once per frame
    void Update()
    {
        bool mirandoAhora = false;
        if (jugadorCerca)
        {
            mirandoAhora = EstaMirandoAlObjeto();

            if (mirandoAhora && !estabaMirando)
            {
                if (manoUI != null)
                {
                    manoUI.ActivarAccion("lanzar"); 
                }
            }
        }

        estabaMirando = mirandoAhora;
        // Si el jugador está cerca y presiona el clic izquierdo
        if (jugadorCerca && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // ... y además está mirando directamente al objeto interactuable
            if (EstaMirandoAlObjeto())
            {   if (manoUI != null)
                {
                    manoUI.ActivarAccion("clicar");
                }
                print("sonio jaj");
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
}
