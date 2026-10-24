using System.Collections;
using UnityEngine;

public class Change_Mats : MonoBehaviour
{
    public Material materialNuevo; 
    public Material materialViejo; 
    private Renderer miDibujador;

    void Start()
    {
        miDibujador = GetComponent<Renderer>();
        materialViejo = miDibujador.material;
    }

    public void ActivarChangeMat()
    {
        
        StartCoroutine(CambiarUnRato());
    }

    IEnumerator CambiarUnRato()
    {
        
        miDibujador.material = materialNuevo;

       
        yield return new WaitForSeconds(0.5f);

        
        miDibujador.material = materialViejo;
    }
}