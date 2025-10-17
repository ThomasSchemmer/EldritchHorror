using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    public float Speed = 1;

    void Start()
    {
        transform.localPosition = Vector3.zero;
    }

    public void Move()
    {
        transform.localPosition += new Vector3(1, 0, 0) * Time.deltaTime * Speed;
    }
}
