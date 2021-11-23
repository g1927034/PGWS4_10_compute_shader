using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeShaderBehaviourScript : MonoBehaviour
{
    [SerializeField] ComputeShader compute_sheder;
    // Start is called before the first frame update
    void Start()
    {
        int x = 8;
        int y = 8;
        ComputeBuffer buffer = new ComputeBuffer(x * y, sizeof(float) * 4);

        int kernel = compute_sheder.FindKernel("CSMain");
        compute_sheder.SetBuffer(kernel, "Result", buffer);
        compute_sheder.Dispatch(kernel, x / 8, y / 8, 1);

        float[] data = new float[4 * x * y];
        buffer.GetData(data);

        buffer.Release();

        for(int i=0;i<x*y;i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);           
            var c0 = data[4 * i + 0];
            var c1 = data[4 * i + 1]*10.0f;
            var c2 = data[4 * i + 2] * 10.0f;
            Debug.Log(c0 + " " + c1 + " " + c2);
            cube.transform.Translate(c0, c1,c2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
