using UnityEngine;

public class Ball : MonoBehaviour
{
    public Transform player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Vector3 a = new Vector3(player.position.x - transform.position.x, 0, 0);
            player.GetComponent<Player>().GetHit(10, a, 1000);
        }
    }
}
