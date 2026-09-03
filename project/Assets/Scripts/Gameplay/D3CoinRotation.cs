using UnityEngine;

public class D3CoinRotation : MonoBehaviour {


	void OnTriggerEnter(Collider col){
        if (col.tag == "Item")
        {
            if (col.GetComponent<D3Item>())
            {
                if (col.GetComponent<D3Item>().itemRotate != null)
                {
                    col.GetComponent<D3Item>().itemRotate.PlayCoin();
                }

                if (col.GetComponent<D3Item>().typeItem == D3Item.TypeItem.Moving_Obstacle)
                {
                    col.GetComponent<D3Item>().UseMovingItem();
                }

            }
        }
    }
	
}
