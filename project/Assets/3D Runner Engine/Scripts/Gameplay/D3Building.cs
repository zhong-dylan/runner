using UnityEngine;

public class D3Building : MonoBehaviour {
	
	[HideInInspector]
	public bool buildingActive;
	[HideInInspector]
	public int buildIndex;
	
	public static D3Building instance;

	bool Mesh = true;


    void Start(){
		instance = this;
        Mesh = true;
    }


    public void ShowMesh()
    {
        if (!Mesh)
        {
            Mesh = true;
            foreach (Transform tran in transform) tran.gameObject.SetActive(true);
        }
    }

    public void HideMesh()
    {
        if (Mesh)
        {
            Mesh = false;
            foreach (Transform tran in transform) tran.gameObject.SetActive(false);
        }
    }

    //Reset building when character die	
    public void Reset(){
		buildingActive = false;
	}

    private void Update()
    {
        if (buildingActive)
        {
            ShowMesh();
        }
        if (!buildingActive)
        {
            HideMesh();
        }
    }

    public bool ShowGizmo = true;
    void OnDrawGizmos()
    {
        if (ShowGizmo)
        {
            if (transform.rotation.y == 0)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawCube((transform.position + new Vector3(-3f, 0f, 0f)), new Vector3(6f, 0.5f, 8f));

                Gizmos.color = Color.green;
                Gizmos.DrawCube((transform.position + new Vector3(13f, 0f, 0f)), new Vector3(26f, 0.5f, 8f));

                Gizmos.color = Color.green;
                Gizmos.DrawCube((transform.position + new Vector3(-19f, 0f, 0f)), new Vector3(26f, 0.5f, 8f));
            }
        }

	}

}
