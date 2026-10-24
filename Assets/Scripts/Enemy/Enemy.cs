using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int vidas;
    private int currentHealth;
    public GameObject winScreenUI;
    private Change_Mats Change_Mat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = vidas;
        Change_Mat = GetComponent<Change_Mats>();

        if (winScreenUI != null)
        {
            winScreenUI.SetActive(false); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        //muerte
        if(currentHealth <=0){
            Destroy(gameObject);
            Time.timeScale = 0f;
            Cursor.visible = true;                  
            Cursor.lockState = CursorLockMode.None;

           
            if (winScreenUI != null)
            {
                winScreenUI.SetActive(true);
            }
        }
    }
    public void TakeDamage(int amount)
    {
        currentHealth-=amount;

        if (Change_Mat != null)
        {
        Change_Mat.ActivarChangeMat();
        }
    }
}
 