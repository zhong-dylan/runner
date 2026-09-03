using UnityEngine;

public class D3ImageRotate : MonoBehaviour
{
    public float speedRotate = 100f;
    void FixedUpdate()
    {
        transform.Rotate(0, 0, speedRotate * Time.fixedDeltaTime);
    }
}
