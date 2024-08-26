using UnityEngine;
using System.Collections;

public class SignalLight : MonoBehaviour
{
    [SerializeField]
    private Material lightMat;
    [SerializeField]
    private float flickRate = 0.2f;

    public bool isLeftSignal;
    
    private bool isOn = false;
    private Material originMat;
    private MeshRenderer mesh;

    float time = 0;
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        originMat = mesh.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
            }
            else
            {
                time = flickRate;
                mesh.material = mesh.material == originMat ? lightMat : originMat;
            } 
        }
    }

    public void Turn(bool isOn)
    {
        this.isOn = isOn;
        if (!isOn) mesh.material = originMat;
    }
}
