using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BloodWash : MonoBehaviour 
{
    private Image picture;

    public float firstFadeAwayAlpha;
    public float secondFadeAwayAlpha;

	private int counter = 0;
    private Color fadeAway1;
    private Color fadeAway2;
    
	void Awake()
	{
		picture = GetComponent<Image>();
	}

    void Start()
    {
		fadeAway1 = new Color(1, 1, 1, firstFadeAwayAlpha);
		fadeAway2 = new Color(1, 1, 1, secondFadeAwayAlpha);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            counter++;
            if (counter == 1)
                picture.color = fadeAway1;
			else if (counter == 2)
                picture.color = fadeAway2;
            else
				gameObject.SetActive(false);
        }
    }
}
