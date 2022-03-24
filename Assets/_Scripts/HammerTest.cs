
using System.Collections;
using UnityEngine;

namespace Assets._Scripts
{
    public class HammerTest : MonoBehaviour
    {

        GameManager gm;

        void Start()
        {
            gm = GameManager.GetInstance();
        }

        public void ClickWorthy()
        {
            gm.ChangeState(GameManager.GameState.WORTHY);
        }

        public void ClickNotWorthy()
        {
            Application.Quit();
        }
    }
}