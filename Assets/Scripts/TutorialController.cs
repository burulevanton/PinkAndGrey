using System;
using Enum;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
        private int tutorialStage = 0;
        private EDirection _direction = EDirection.None;

        public GameObject[] tutorialHints;
        public static event Action<EDirection> OnSwipe;
        

        private void Start()
        {
                tutorialHints[0].SetActive(true);
                SwipeController.OnSwipe += new Action<EDirection>(ProcessSwipe);
        }

        private void OnDisable()
        {
                SwipeController.OnSwipe -= new Action<EDirection>(ProcessSwipe);
        }

        private void ProcessSwipe(EDirection direction)
        {
                if (tutorialStage>3)
                {
                        OnSwipe?.Invoke(direction);
                }
                switch (direction)
                {
                        case EDirection.Right:
                                if (tutorialStage == 0)
                                {
                                        tutorialHints[0].SetActive(false);
                                        tutorialHints[1].SetActive(true);
                                        tutorialStage++;
                                }
                                else
                                        return;
                                break;
                        case EDirection.Up:
                                if (tutorialStage == 1)
                                {
                                        tutorialHints[1].SetActive(false);
                                        tutorialHints[2].SetActive(true);
                                        tutorialStage++;
                                }
                                else
                                        return;
                                break;
                        case EDirection.Left:
                                if (tutorialStage == 2)
                                {
                                        tutorialHints[2].SetActive(false);
                                        tutorialHints[3].SetActive(true);
                                        tutorialStage++;
                                }
                                else
                                        return;
                                break;
                        case EDirection.Down:
                                if (tutorialStage == 3)
                                {
                                        tutorialHints[3].SetActive(false);
                                        tutorialStage++;
                                }
                                else
                                        return;
                                break;
                }
                OnSwipe?.Invoke(direction);
        }
}