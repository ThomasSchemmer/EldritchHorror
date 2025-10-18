using UnityEngine;

public class RotateAroundAxis : MonoBehaviour
{
    public float Speed = 1;
    public Vector3 Axis = Vector3.one;

    void Start()
    {
        
    }

    void Update()
    {
        this.transform.Rotate(Axis, Speed * Time.deltaTime);    
    }
}
