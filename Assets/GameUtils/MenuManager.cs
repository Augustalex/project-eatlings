using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;
    
    private static MenuManager instance;

    private void Awake()
    {
        instance = this;
        
        menu.SetActive(false);
    }

    public static MenuManager Get()
    {
        return instance;
    }

    public void ToggleMenu()
    {
        menu.SetActive(!menu.activeSelf);
    }
}
