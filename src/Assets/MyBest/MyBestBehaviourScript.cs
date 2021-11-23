using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBestBehaviourScript : MonoBehaviour
{
    [SerializeField] ComputeShader compute_sheder;
    List<GameObject> cubes = new List<GameObject>();
    const int max = 30;

    // Start is called before the first frame update
    void Start()
    {
        ComputeBuffer buffer = new ComputeBuffer(max, sizeof(float) * 3);
        int kernel = compute_sheder.FindKernel("MyBest");
        compute_sheder.SetBuffer(kernel, "Result", buffer);
        compute_sheder.Dispatch(kernel, 1, 1, 1);

        float[] data = new float[3 * max];
        buffer.GetData(data);

        buffer.Release();

        for (int i = 0; i < max; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var c0 = data[3 * i + 0] / 100;
            var c1 = data[3 * i + 1] / 100;
            var c2 = data[3 * i + 2] / 100;
            cube.transform.Translate(c0, c1, c2);
            cubes.Add(cube);
        }
        Debug.Log(cubes.Count);
    }

    // Update is called once per frame
    void Update()
    {
        ComputeBuffer buffer = new ComputeBuffer(max, sizeof(float) * 3);
        int kernel = compute_sheder.FindKernel("MyBest");
        compute_sheder.SetBuffer(kernel, "Result", buffer);
        compute_sheder.Dispatch(kernel, 1, 1, 1);

        float[] data = new float[3 * max];
        buffer.GetData(data);

        buffer.Release();

        for (int i = 0; i < max; i++)
        {
            var c0 = data[3 * i + 0] /300;
            var c1 = data[3 * i + 1] /300;
            var c2 = data[3 * i + 2] /300;

            cubes[i].transform.Translate(c0, c1, c2);
        }
    }
}
