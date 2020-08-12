using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
public class oscillator: MonoBehaviour
{
    [SerializeField] Vector3 movement;
    [SerializeField] float period;
    [SerializeField] float moverange;
    // Start is called before the first frame update

    Vector3 startpt;
    void Start()
    {
        startpt = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(period<=Mathf.Epsilon)
        {
            return;
        }
        float cycle = Time.time / period;
        const float tau = Mathf.PI * 2f;
        float sinwave = Mathf.Sin(tau * cycle);
        moverange = (sinwave/2f)+0.5f;
        Vector3 offset = movement * moverange;
         transform.position =startpt+ offset;
    }
}
