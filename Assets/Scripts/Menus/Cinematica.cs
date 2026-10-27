using System.Collections;
using UnityEngine;

public class Cinematica : MonoBehaviour
{
    public GameObject cinematicacanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(EsperaryDestruir());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator EsperaryDestruir()
    {
        yield return new WaitForSeconds(13f);//espera exactamente lo q dura
       if (cinematicacanvas != null)
        {
            Destroy(cinematicacanvas);
        }
        
       
    }
}
 
