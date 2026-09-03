using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    //objeto a controlar
    public GameObject currentObject;

    // Cuántos grados gira cada vez que pulsamos el botón
    public float rotationAmount = 15f;

    public void RotateLeft()
    {
        if (currentObject != null)
        {
            if (currentObject.name.Contains("Muffin"))
            {
                currentObject.transform.Rotate(0f, 0f, -rotationAmount);
            }
            else
            {
                currentObject.transform.Rotate(0f, -rotationAmount, 0f);
            }
        }
    }

    public void RotateRight()
    {
        if (currentObject != null)
        {
            if (currentObject.name.Contains("Muffin"))
            {
                currentObject.transform.Rotate(0f, 0f, rotationAmount);
            }
            else
            {
                currentObject.transform.Rotate(0f, rotationAmount, 0f);
            }
        }
    }

    // Indica cuál es el objeto que estamos controlando
    public void SetCurrentObject(GameObject newObject)
    {
        currentObject = newObject;
    }
}
