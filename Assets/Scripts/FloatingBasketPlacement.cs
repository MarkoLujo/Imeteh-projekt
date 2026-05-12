using UnityEngine;

public class FloatingBasketPlacement : MonoBehaviour
{
    [Header("References")]
    public GameObject basketPrefab;

    // Kamera/headset transform
    public Transform cameraTransform;

    [Header("Placement Settings")]
    public float distanceFromPlayer = 2f;

    private GameObject previewBasket;

    private bool isPlacing = false;

    void Update()
    {
        /*
         * X BUTTON (Left controller)
         * Quest 3 input
         */
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            StartPlacement();
        }

        /*
         * Ako nismo u placement modu,
         * ne radi ništa dalje
         */
        if (!isPlacing)
            return;

        /*
         * Koš prati igrača
         */
        UpdatePreviewPosition();

        /*
         * Right trigger potvrđuje placement
         */
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            PlaceBasket();
        }
    }

    void StartPlacement()
    {
        /*
         * Ako već postoji preview,
         * obriši ga
         */
        if (previewBasket != null)
        {
            Destroy(previewBasket);
        }

        /*
         * Spawnaj novi koš
         */
        previewBasket = Instantiate(basketPrefab);

        /*
         * Ugasi physics dok preview traje
         */
        Rigidbody rb = previewBasket.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        isPlacing = true;
    }

    void UpdatePreviewPosition()
    {
        /*
         * Pozicija:
         * 2m ispred headseta
         */
        Vector3 targetPosition =
            cameraTransform.position +
            cameraTransform.forward * distanceFromPlayer;

        previewBasket.transform.position = targetPosition;

        /*
         * Rotacija:
         * koš gleda prema igraču
         */
        Vector3 lookDirection =
            cameraTransform.position - previewBasket.transform.position;

        lookDirection.y = 0;

        previewBasket.transform.rotation =
            Quaternion.LookRotation(-lookDirection);
    }

    void PlaceBasket()
    {
        /*
         * Upali physics
         */
        Rigidbody rb = previewBasket.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
        }

        /*
         * Izlaz iz placement moda
         */
        previewBasket = null;

        isPlacing = false;
    }
}