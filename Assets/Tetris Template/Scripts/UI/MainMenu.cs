//  /*********************************************************************************
//   *********************************************************************************
//   *********************************************************************************
//   * Produced by Skard Games										                  *
//   * Facebook: https://goo.gl/5YSrKw											      *
//   * Contact me: https://goo.gl/y5awt4								              *											
//   * Developed by Cavit Baturalp Gürdin: https://tr.linkedin.com/in/baturalpgurdin *
//   *********************************************************************************
//   *********************************************************************************
//   *********************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public GameObject menuButtons;
    public GameObject restartButton;
    public TMPro.TextMeshProUGUI _text;

    void Awake()
    {
    }

    void OnEnable()
	{
        menuButtons.SetActive (true);   
    }

	void OnDisable()
	{
		menuButtons.SetActive (false); 
    }

    public void DisableMenuButtons ()
	{
		menuButtons.SetActive (false);
	}

    public void MainMenuStartAnimation()
    {
        menuButtons.GetComponent<RectTransform>().DOAnchorPosY(0, 1, true);
    }

    public void MainMenuEndAnimation()
    {
        menuButtons.GetComponent<RectTransform>().DOAnchorPosY(-1450, 0.3f, true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnManager.dtfMode = !SpawnManager.dtfMode;
            var onOff = SpawnManager.dtfMode ? "OFF" : "ON";
            _text.text = $"Press SPACE to turn {onOff} DTF Mode";
        }
    }
}

