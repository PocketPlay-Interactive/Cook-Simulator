using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenePanel : MonoSingleton<ScenePanel>
{
    public Text _TutorialText;

    public SuccessPanel _SuccessPanel;
    public FailurePanel _FailurePanel;

    public void SetText(int _TaskIndex)
    {
        switch(_TaskIndex)
        {
            case 0:
                _TutorialText.text = "Please click to choose a piece of beef";
                break;
            case 1:
                _TutorialText.text = "You need to cut off the piece of meat closest to the blue line. Tap continuously on the screen to cut.";
                break;
            case 2:
                _TutorialText.text = "You need to press on the salt shaker and then move it once it's in place.\nPress the clamp. When the clamp is clamped, swipe your hand on the screen from right to left. Finally, click on the screen when the meat is seasoned.";
                break;
            case 3:
                _TutorialText.text = "Adjust the heat to high so that the meat cooks quickly. When the meat is cooked, turn the heat to the lowest to turn off the stove.";
                break;
            case 4:
                _TutorialText.text = "Touch the piece of meat to help the player eat.";
                break;
        }
    }
}
