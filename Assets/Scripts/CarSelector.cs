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
        // Save Position
        PlayerPrefs.SetFloat("CarX", carRenderer.transform.position.x);
        PlayerPrefs.SetFloat("CarY", carRenderer.transform.position.y);

        // Save Rotation (Just the Z axis)
        PlayerPrefs.SetFloat("CarRotZ", carRenderer.transform.eulerAngles.z);

        PlayerPrefs.Save(); // Forces Unity to write to disk
    }

    void LoadCarData()
    {
        // Check if we have a saved X position (otherwise it's the first time playing)
        if (PlayerPrefs.HasKey("CarX"))
        {
            float x = PlayerPrefs.GetFloat("CarX");
            float y = PlayerPrefs.GetFloat("CarY");
            float zRot = PlayerPrefs.GetFloat("CarRotZ");

            carRenderer.transform.position = new Vector3(x, y, 0);
            carRenderer.transform.eulerAngles = new Vector3(0, 0, zRot + 180);
        }
        else
        {
            // Default setup if no save exists
            carRenderer.transform.position = Vector3.zero;
            carRenderer.transform.eulerAngles = new Vector3(0, 0, 180f);
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
