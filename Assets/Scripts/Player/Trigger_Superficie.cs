using UnityEngine;

public class Trigger_Superficie : MonoBehaviour
{
    private bool jugadorCerca = false; 
    private GameObject jugadorRef;
    
    private AudioSource[] VocesNinyos = new AudioSource[5];

    public AudioSource voces1;
    public AudioSource voces2;
    public AudioSource voces3;
    public AudioSource voces4;
    public AudioSource voces5;

    private AudioSource vozActual;
    private int voiceIndex = 0;
    private bool yaEstabaDentro = false;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VocesNinyos[0] = voces1;
        VocesNinyos[1] = voces2;
        VocesNinyos[2] = voces3;
        VocesNinyos[3] = voces4;
        VocesNinyos[4] = voces5;

        vozActual = VocesNinyos[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!yaEstabaDentro)
        { 
            if (other.CompareTag("Player")) 
            {
                jugadorCerca = true;
                jugadorRef = other.gameObject;
                print("va cagao ves");
                ReproducirSonidoVoces();
                yaEstabaDentro = true;


            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            jugadorRef = null;
            yaEstabaDentro=false;
        }
    }

    private void ReproducirSonidoVoces()
    {
        if (vozActual != null)
        {

            if (vozActual.clip != null)
            {
                vozActual = VocesNinyos[voiceIndex];
                vozActual.PlayOneShot(vozActual.clip);

                if(voiceIndex < VocesNinyos.Length-1) voiceIndex += 1;
                else voiceIndex = 0;

            }

        }

    }
}
