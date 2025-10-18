using UnityEngine;

public class Rotate : MonoBehaviour
{

    public float Speed = 1;
    public float Scale = 5;
    public Transform target;

    public float Angle = 0;

    void Start()
    {
        // if there is a parent, use it's localPosition as axis for rotation
        // if (this.transform.parent != null)
        // {
        //     this.AxisVector = this.transform.parent.localPosition;
        // }

        // var parentRotation = this.Speed = this.transform.parent.GetComponent<Rotate>();
        // if (parentRotation)
        // {
        //     this.Speed = parentRotation.Speed * RelativeRotationSpeed;
        // }
    }

    void Update()
    {
        Angle += Time.deltaTime * Speed % 360;
        var position = new Vector3(Mathf.Cos(Angle), 0, Mathf.Sin(Angle)) * Scale;
        this.transform.localPosition = position + target.position;
    }
}
