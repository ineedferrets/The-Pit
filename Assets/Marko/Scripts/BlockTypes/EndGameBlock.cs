using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameBlock : Block
{
    // Start is called before the first frame update

    protected override void DestroyBlock()
    {
        GameLogicScript_Marko.Instance.ExitGame();
    }

}
