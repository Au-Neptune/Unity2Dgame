using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private Collider2D coll;
    public bool move = false;
    private float timer = 0;
    void Start()
    {
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            timer += Time.deltaTime;
            if (timer >= 0.4)
            {
                transform.Translate(Vector2.up * Time.deltaTime * 20f);    
                GetComponent<SpriteRenderer>().color = Color.white;            
            }

            if (timer >= 4)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (move && other.tag == "Player")
            other.GetComponent<Player>().GetHit(30);
        
    }
}
