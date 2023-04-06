using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Collider2D coll;
    void Start()
    {
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")  return;
        if (other.tag == "Effect")  return;
        
        if (other.tag == "Enemy")
        {
            Vector3 a = new Vector3(other.transform.position.x - transform.position.x, 0, 0);
            Enemy e = other.GetComponent<Enemy>();
                
            if (e == null)
            {
                other.GetComponent<Archer>().GetHit(10, a, 500); 
            }    
            else
            {
                e.GetHit(10, a, 500);    
            }
        }
        
        if (other.tag == "Boss")
            other.GetComponent<Boss>().GetHit(10);
        
        Destroy(gameObject);
    }
}
