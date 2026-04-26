using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("💥 Bala impactó contra: " + collision.gameObject.name);

        Enemy target = collision.gameObject.GetComponent<Enemy>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
