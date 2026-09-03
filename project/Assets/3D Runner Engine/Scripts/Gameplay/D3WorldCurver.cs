using UnityEngine;

[System.Serializable]
public class D3WorldCurver : MonoBehaviour
{
    [HideInInspector]
    public Vector3 Curvature = new Vector3(0, 5f, 0);
    [HideInInspector]
    public float Distance = 1;
    [HideInInspector]
    public float CurvatureScaleUnit = 1000f;

    int CurvatureID;
    int DistanceID;

    public static D3WorldCurver WorldCurver;

    private void Start()
    {
        WorldCurver = this;
        CurvatureID = Shader.PropertyToID("_Curvature");
        DistanceID = Shader.PropertyToID("_Distance");
    }

    void Update()
    {
        Vector3 curvature = CurvatureScaleUnit == 0 ? Curvature : Curvature / CurvatureScaleUnit;

        Shader.SetGlobalVector(CurvatureID, curvature);
        Shader.SetGlobalFloat(DistanceID, Distance);
    }
}
