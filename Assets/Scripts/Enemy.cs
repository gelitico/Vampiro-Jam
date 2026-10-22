using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int vidas;
    private int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = vidas;
    }

    // Update is called once per frame
    void Update()
    {
        //muerte
        if(currentHealth <=0)
            Destroy(gameObject);
    }
    public void TakeDamage(int amount)
    {
        currentHealth-=amount;
    }
}
 