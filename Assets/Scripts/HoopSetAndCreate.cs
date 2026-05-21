using UnityEngine;
using System.Collections;

public class HoopSetAndCreate : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject basketPrefab;
    public GameObject previewPrefab;

    [Header("Controller")]
    public Transform rightControllerTransform;

    private GameObject currentPreview;
    public GameObject spawnedBasket;

    public bool isPlacing = false;

    void Update()
    {
        /*
         * X BUTTON
         * prvi klik -> preview mode
         * drugi klik -> place basket
         */

        // Pozicija koša se može promijenit samo van levela
        if (LevelManager.instance.isFreeplay)
        {
            if (OVRInput.GetDown(OVRInput.Button.Two))
            {
                /*
                 * Ako trenutno NE postavljamo:
                 * napravi preview
                 */
                if (!isPlacing)
                {
                    DeleteBasket();
                    StartPlacement();
                }
                /*
                 * Ako VEĆ postavljamo:
                 * potvrdi placement
                 */
                else
                {
                    PlaceBasket();
                }
            }

            /*
             * Ako nema placement moda,
             * ne radi raycast
             */
            if (!isPlacing)
                return;

            UpdatePreview();
        }
    }

    void StartPlacement()
    {
        /*
         * Ako preview već postoji,
         * obriši ga
         */
        if (currentPreview != null)
        {
            Destroy(currentPreview);
        }

        /*
         * Spawn preview koša
         */
        currentPreview = Instantiate(previewPrefab);

        isPlacing = true;
    }

    void UpdatePreview()
{
    Ray ray = new Ray(
        rightControllerTransform.position,
        rightControllerTransform.forward
    );

    if (Physics.Raycast(ray, out RaycastHit hit))
    {
        /*
         * Je li Meta zid?
         */
        bool isWall =
            hit.collider.gameObject.name.Contains("WALL_FACE");

        if (!isWall)
        {
            currentPreview.SetActive(false);
            return;
        }

        currentPreview.SetActive(true);

        /*
         * Mali offset od zida
         */
        Vector3 wallOffset =
            hit.normal * 0.05f;

        Vector3 targetPos = hit.point + wallOffset;

/*
 * Dohvati wall bounds
 */
Collider col =
    hit.collider.GetComponent<Collider>();

float wallMinY =
    col.bounds.min.y;

float wallMaxY =
    col.bounds.max.y;

/*
 * Clamp visine koša
 */
targetPos.y = Mathf.Clamp(
    targetPos.y,
    wallMinY + 0.2f,
    wallMaxY - 0.2f
);
currentPreview.transform.position = targetPos;

        /*
         * Ispravna rotacija
         */
        currentPreview.transform.rotation =
            Quaternion.LookRotation(-hit.normal)*
            Quaternion.Euler(0, 270, 0);
    }
}

    public void DeleteBasket() { 
        /*
        * Ako već postoji stari koš,
        * obriši ga
        */
        if (spawnedBasket != null)
        {
            Destroy(spawnedBasket);
        }
    
    }

    public void UpdateBasket() {
        if (spawnedBasket != null)
        {
            Transform oldTransform = spawnedBasket.transform;
            DeleteBasket();

            spawnedBasket = Instantiate(
                basketPrefab,
                oldTransform.position,
                oldTransform.rotation
            );
        }

    }

    void PlaceBasket()
    {

        DeleteBasket();
        /*
         * Spawn pravog koša
         */
        spawnedBasket = Instantiate(
            basketPrefab,
            currentPreview.transform.position,
            currentPreview.transform.rotation
        );

        /*
         * Makni preview
         */
        Destroy(currentPreview);

        currentPreview = null;

        isPlacing = false;
    }
}