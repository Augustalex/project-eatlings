using Unity.Mathematics;
using UnityEngine;

public class EatlingModeController : MonoBehaviour
{
    public enum EatlingMode
    {
        Baby,
        Grown
    };

    [SerializeField] public EatlingMode startingMode = EatlingMode.Baby;

    public GameObject baby;
    public GameObject grown;

    public void SetBaby()
    {
        grown.SetActive(false);
        baby.SetActive(true);
    }

    public void SetFullyGrown()
    {
        baby.SetActive(false);
        grown.SetActive(true);
    }

    // Private
    private void Awake()
    {
        if (startingMode == EatlingMode.Baby)
            SetBaby();
        else
            SetFullyGrown();

        var startingPosition = transform.position;
        transform.position = Vector3.zero;
        baby.transform.position = startingPosition;
        grown.transform.position = startingPosition;
    }

    private void Update()
    {
        if (baby.activeSelf)
        {
            grown.transform.position = baby.transform.position;
        }
        else
        {
            baby.transform.position = grown.transform.position;
        }
    }

    public void SetFullyGrownPlantedAt(FarmTile tile)
    {
        grown.GetComponent<GrownEatlingPlanted>().SetPlanted(tile);
    }

    public bool InBabyMode()
    {
        return baby.activeSelf;
    }
}