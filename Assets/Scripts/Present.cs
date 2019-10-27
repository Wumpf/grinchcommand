using UnityEngine;

public class Present : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag != tag)
            Destroy(this.gameObject);
    }
}
