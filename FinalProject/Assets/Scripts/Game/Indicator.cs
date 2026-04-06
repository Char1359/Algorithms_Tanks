using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public GameObject RingObject;
    public GameObject OuterObject;

    private Barrel barrel;

    private Material ringMaterial;
    private Material outerMaterial;

    private float kAngularSpeed = 90.0f;

    // Start is called before the first frame update
    void Start()
    {
        ringMaterial = RingObject.GetComponentInChildren<MeshRenderer>().material;
        outerMaterial = OuterObject.GetComponent<MeshRenderer>().material;
        SetColor(Color.white);
    }

    // Update is called once per frame
    void Update()
    {
        OuterObject.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f) * kAngularSpeed * Time.deltaTime);

        if(barrel != null)
        {
            Vector3 barrelLocation = barrel.transform.position;
            Vector3 location = new Vector3(barrelLocation.x, 0.0f, barrelLocation.z);
            transform.position = location;
        }
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void SetColor(Color color)
    {
        if (ringMaterial == null)
        {
            ringMaterial = RingObject.GetComponentInChildren<MeshRenderer>().material;
        }

        if (outerMaterial == null)
        {
            outerMaterial = OuterObject.GetComponent<MeshRenderer>().material;
        }

        ringMaterial.color = color;
        outerMaterial.color = color;
    }

    public void SetBarrel(Barrel barrel)
    {
        this.barrel = barrel;
    }
}
