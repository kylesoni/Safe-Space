using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public string dialogue;
    private bool inRange = false;
    private UI_Inventory player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<UI_Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && Input.GetMouseButtonDown(0))
        {
            if (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero))
            {
                if (!dialogueBox.activeInHierarchy && !player.isInventoryMode)
                {
                    dialogueBox.SetActive(true);
                    dialogueText.text = dialogue;
                }
                else
                {
                    dialogueBox.SetActive(false);
                }
            }
        }

        if (player.isInventoryMode && dialogueBox.activeInHierarchy)
        {
            dialogueBox.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Movement>() != null)
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Movement>() != null)
        {
            inRange = false;
            dialogueBox.SetActive(false);
        }
    }
}
