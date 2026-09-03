using UnityEngine;

public class D3Magnet : MonoBehaviour {

    public GameObject ObjectMagnet;
    void OnTriggerEnter(Collider col){
        if (col.tag == "Item")
        {
            if (col.GetComponent<D3Item>())
            {
                if (col.GetComponent<D3Item>().typeItem == D3Item.TypeItem.Coin)
                {
                    if (ObjectMagnet != null)
                    {
                        col.GetComponent<D3Item>().StartCoroutine(col.GetComponent<D3Item>().UseAbsorb(ObjectMagnet.gameObject));
                    }
                    else
                    {
                        col.GetComponent<D3Item>().StartCoroutine(col.GetComponent<D3Item>().UseAbsorb(this.gameObject));
                    }

                }
            }
        }

    }
}
