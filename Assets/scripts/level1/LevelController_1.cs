using PlanetaryDeception;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController_1 : MonoBehaviour {
    public Text alertText;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var Inventory = PlayerInventory.Instance();

        if (Input.anyKey)
        {
            if (!Inventory.ContainsItem(KnownItemsInventory.BubbleGumWrappingPaper))
            {
                var KnownItems = KnownItemsInventory.Instance();
                KnownItems.TransferItem(KnownItemsInventory.BubbleGumWrappingPaper, Inventory);
            }
        }

        if (Inventory.ContainsItem(KnownItemsInventory.BubbleGumWrappingPaper))
        {
            alertText.text = "You have BubbleGumWrappingPaper";
        }
    }
}
