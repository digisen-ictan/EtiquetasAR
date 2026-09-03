using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ImageTrackingController : MonoBehaviour
{
    // --------------------------------------------------------- 
    // 1. PREFABS 
    // --------------------------------------------------------- 

    // Aquí se indica desde el Inspector qué prefab corresponde a cada etiqueta. 

    public GameObject pizzaPrefab;
    public GameObject albondigasPrefab;
    public GameObject muffinPrefab;

    public ObjectController objectController; //llamada a la clase ObjectController (script propio) para poder controlar la rotación de los objetos instanciados

    // --------------------------------------------------------- 
    // 2. AR TRACKED IMAGE MANAGER 
    // --------------------------------------------------------- 
    // Este componente es el que se encarga de detectar las imágenes mediante AR Foundation.
    private ARTrackedImageManager imageManager;


    // --------------------------------------------------------- 
    // 3. OBJETOS QUE HEMOS CREADO 
    // --------------------------------------------------------- 
    // Necesitamos guardar una referencia a cada objeto que creemos. 
    // Si desaparece la etiqueta Pizza, tenemos que saber qué objeto tenemos que destruir

    private GameObject pizzaObject;
    private GameObject albondigasObject;
    private GameObject muffinObject;



    private int visibleObjects = 0; //objetos visibles en la escena
    private List<GameObject> visibleObjectList = new List<GameObject>(); //para saber cuales son los que estan activos
    private Camera arCamera;


    private void Awake()
    {
        imageManager = GetComponent<ARTrackedImageManager>(); // Obtenemos el componente ARTrackedImageManager del GameObject al que está adjunto este script (que debería ser el AR Session Origin)
        arCamera = Camera.main; // Obtenemos la cámara principal
    }

    private void OnEnable()
    {
        imageManager.trackedImagesChanged += OnTrackedImagesChanged; // Nos suscribimos al evento de cambio de imágenes rastreadas
    }

    private void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnTrackedImagesChanged; // Nos desuscribimos del evento cuando el objeto se desactiva
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        //AR Foundation nos proporciona tres listas: added, updated y removed.

        // Nueva imagen detectada

        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            // Averiguamos cómo se llama la imagen detectada.
            string imageName = trackedImage.referenceImage.name;
            visibleObjects++; //se detecta un objeto en la escena

            if (imageName == "Pizza") //si se llama pizza, se crea el objeto pizza
            {
                CreatePizza(trackedImage);
                visibleObjectList.Add(pizzaObject); //se añade a la lista de objetos visibles
            }
            else if (imageName == "Albondigas")
            {
                CreateAlbondigas(trackedImage);
                visibleObjectList.Add(albondigasObject);
            }
            else if (imageName == "Muffin")
            {
                CreateMuffin(trackedImage);
                visibleObjectList.Add(muffinObject);
            }
        }

        // Imagen actualizada, por ejemplo, si se mueve o rota
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            string imageName = trackedImage.referenceImage.name;

            if (imageName == "Pizza")
            {
                if (trackedImage.trackingState == TrackingState.Tracking)
                {
                    if (pizzaObject == null)
                    {
                        CreatePizza(trackedImage);
                    }
                    else
                    {
                        pizzaObject.transform.position =
                            trackedImage.transform.position;

                        //si queremos rotar el objeto con los botones, hay que desactivar la rotación automática cada vez que detecta la etiqueta

                        //pizzaObject.transform.rotation =
                        //trackedImage.transform.rotation;
                    }
                }
                else
                {
                    DestroyPizza();
                }
            }

            else if (imageName == "Albondigas")
            {
                if (trackedImage.trackingState == TrackingState.Tracking)
                {
                    if (albondigasObject == null)
                    {
                        CreateAlbondigas(trackedImage);
                    }
                    else
                    {
                        albondigasObject.transform.position =
                            trackedImage.transform.position;

                        //albondigasObject.transform.rotation =
                        //trackedImage.transform.rotation;
                    }
                }
                else
                {
                    DestroyAlbondigas();
                }
            }
            else if (imageName == "Muffin")
            {
                if (trackedImage.trackingState == TrackingState.Tracking)
                {
                    if (muffinObject == null)
                    {
                        CreateMuffin(trackedImage);
                    }
                    else
                    {
                        muffinObject.transform.position =
                            trackedImage.transform.position;

                        //muffinObject.transform.rotation =
                        //trackedImage.transform.rotation;
                        //muffinObject.transform.Rotate(-90f, 0f, 0f);
                    }
                }
                else
                {
                    DestroyMuffin();
                }
            }
        }

        //Imagen eliminada por AR Foundation, por ejemplo, si se pierde el seguimiento
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            visibleObjects--; //se elimina un objeto de la escena
            string imageName = trackedImage.referenceImage.name;

            if (imageName == "Pizza")
            {
                visibleObjectList.Remove(pizzaObject); //ya no esta, eliminado de la lista de objetos visibles
                DestroyPizza();
            }
            else if (imageName == "Albondigas")
            {
                visibleObjectList.Remove(albondigasObject);
                DestroyAlbondigas();
            }
            else if (imageName == "Muffin")
            {
                visibleObjectList.Remove(muffinObject);
                DestroyMuffin();
            }
        }

        //-------------------------------- Objeto actual a controlar --------------------------------

        if (visibleObjectList.Count == 1)
        {
            objectController.SetCurrentObject(visibleObjectList[0]);
        }
    }

    // -----------------------  Crear objetos  ----------------------------------

    private void CreatePizza(ARTrackedImage trackedImage) // Creamos la pizza en la posición y rotación de la imagen rastreada
    {
        // Por seguridad, evitamos duplicarla
        if (pizzaObject != null)
        {
            Destroy(pizzaObject);
        }

        //Esta funcion crea el objeto pizza en la posición y rotación de la imagen rastreada. Funcion propia de Unity, de AR Tracker Image Manager.
        pizzaObject = Instantiate(
            pizzaPrefab,
            trackedImage.transform.position,
            trackedImage.transform.rotation
        );
        pizzaObject.transform.Rotate(0f, 112.1f, 0f);

        objectController.SetCurrentObject(pizzaObject); //objeto actual a controlar es la pizza, para que los botones de rotación funcionen con este objeto
    }
    private void CreateAlbondigas(ARTrackedImage trackedImage)
    {
        if (albondigasObject != null)
        {
            Destroy(albondigasObject);
        }

        albondigasObject = Instantiate(
            albondigasPrefab,
            trackedImage.transform.position,
            trackedImage.transform.rotation
        );
        objectController.SetCurrentObject(albondigasObject);
    }

    private void CreateMuffin(ARTrackedImage trackedImage)
    {
        if (muffinObject != null)
        {
            Destroy(muffinObject);
        }

        muffinObject = Instantiate(
            muffinPrefab,
            trackedImage.transform.position,
            trackedImage.transform.rotation
        );
        muffinObject.transform.Rotate(-90f, 0f, 0f);

        objectController.SetCurrentObject(muffinObject);
    }


    // -----------------------  Destruir objetos  ----------------------------------

    private void DestroyPizza()
    {
        if (pizzaObject != null)
        {
            Destroy(pizzaObject);
            pizzaObject = null;
        }
    }

    private void DestroyAlbondigas()
    {
        if (albondigasObject != null)
        {
            Destroy(albondigasObject);
            albondigasObject = null;
        }
    }
    private void DestroyMuffin()
    {
        if (muffinObject != null)
        {
            Destroy(muffinObject);
            muffinObject = null;
        }
    }

    private void Update()
    {
        if (visibleObjectList.Count > 1) //cuando hay más de un objeto en la escena, se puede seleccionar el que quieras tocando la pantalla
        {
            //si tocas la pantalla, se hace un raycast desde la cámara hasta el punto donde tocas, y si hay un objeto, se selecciona 
            if (Input.touchCount > 0 &&
                Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Touch touch = Input.GetTouch(0); // obtenemos el toque de la pantalla

                Ray ray = arCamera.ScreenPointToRay(touch.position);

                RaycastHit hit; // variable para almacenar la información del objeto que se toca

                if (Physics.Raycast(ray, out hit)) // hacemos el raycast y si hay un objeto, lo seleccionamos
                {
                    GameObject selectedObject = hit.collider.gameObject;

                    objectController.SetCurrentObject(selectedObject); //objeto actual a controlar es el que se toca
                }
            }
        }
    }
}
