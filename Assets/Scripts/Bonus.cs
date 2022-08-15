using UnityEngine;

public class Bonus : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Knife")
        {
            gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
        }
    }
}
