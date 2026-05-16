using UnityEngine;

public class CarSelector : MonoBehaviour
{
    public Sprite BMW, Default, Passat, Prius;
    public SpriteRenderer carRenderer;

    void Start()
    {
        LoadCarData();
    }

    // Call this to save the current state
    public void SaveCarData()
    {
        PlayerPrefs.SetFloat("CarX", carRenderer.transform.position.x);
        PlayerPrefs.SetFloat("CarY", carRenderer.transform.position.y);
        PlayerPrefs.SetFloat("CarRotZ", carRenderer.transform.eulerAngles.z);

        // SAVE THE SPRITE NAME
        PlayerPrefs.SetString("SelectedCar", carRenderer.sprite.name);

        PlayerPrefs.Save();
    }

    void LoadCarData()
    {
        if (PlayerPrefs.HasKey("CarX"))
        {
            // ... (Your existing position/rotation loading code)

            // LOAD THE SPRITE
            string savedCar = PlayerPrefs.GetString("SelectedCar");

            if (savedCar == BMW.name) carRenderer.sprite = BMW;
            else if (savedCar == Passat.name) carRenderer.sprite = Passat;
            else if (savedCar == Prius.name) carRenderer.sprite = Prius;
            else carRenderer.sprite = Default;
        }
    }

    public void OnBMWButtonClick()
    {
        
        carRenderer.sprite = BMW;
        SaveCarData(); // Save every time they pick a car
    }

    // ... (Add SaveCarData() to your other button methods too!)


public void OnDefaultButtonClick()
    {
        carRenderer.sprite = Default;
        SaveCarData();
    }

    public void OnPassatButtonClick()
    {
        carRenderer.sprite = Passat;
        SaveCarData();
    }

    public void OnPriusButtonClick()
    {
        carRenderer.sprite = Prius;
        SaveCarData();
    }
}
