using UnityEngine;

public class D3Floor : MonoBehaviour
{

    public GameObject FloorDefault;
    public GameObject FloorSpecial;
    // Start is called before the first frame update

    private void Start()
    {
        if (FloorDefault != null)
        {
            FloorDefault.SetActive(false);
        }
        if (FloorSpecial != null)
        {
            FloorSpecial.SetActive(false);
        }
        
        
    }


    public void UseFloorDefault() {

        if (FloorDefault != null)
        {
            FloorDefault.SetActive(true);
            if (FloorSpecial != null)
            {
                FloorSpecial.SetActive(false);
            }
           
        }
        else {
            if (FloorSpecial != null)
            {
                FloorSpecial.SetActive(true);
            }
            if (FloorDefault != null)
            {
                FloorDefault.SetActive(false);
            }
        }
    }


    public void UseFloorSpecial()
    {

        if (FloorSpecial != null)
        {
            FloorSpecial.SetActive(true);
            if (FloorDefault != null)
            {
                FloorDefault.SetActive(false);
            }

        }
        else
        {
            if (FloorDefault != null)
            {
                FloorDefault.SetActive(true);
            }
            if (FloorSpecial != null)
            {
                FloorSpecial.SetActive(false);
            }
        }
    }
}
