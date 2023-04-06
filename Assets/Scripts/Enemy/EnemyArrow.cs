using UnityEngine;

public class EnemyArrow : MonoBehaviour
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
        if (other.tag == "Enemy")  return;
        if (other.tag == "Effect")  return;
        
        if (other.tag == "Player")
        {
            Vector3 a = new Vector3(other.transform.position.x - transform.position.x, 0, 0);
            other.GetComponent<Player>().GetHit(10, a, 500);            
        }
        
        Destroy(gameObject);
    }
}
