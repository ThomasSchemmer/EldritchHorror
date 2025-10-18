using UnityEngine;

public class RotateClouds : MonoBehaviour
{

    public float Speed = 1;
    public Vector3 AxisVector = new Vector3(0, 1, 0); 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(AxisVector, Speed * Time.deltaTime);
    }
}
