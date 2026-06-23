using UnityEngine;

public class BulletVisual : MonoBehaviour
{
    private Vector3 target = new Vector3(1, 1, 1);
    private float speed = 0;

    public void Init(Vector3 targetPosition, float moveSpeed)
    {
        target = targetPosition;
        speed = moveSpeed;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.forward = (target - transform.position).normalized;
        if (Vector3.Distance(transform.position, target) < 0.05f) Destroy(gameObject);
    }
}