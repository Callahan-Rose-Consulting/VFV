using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.EventSystems;




// Get rid of the naming convention complaint and the " 'new' expression can be simplified " message.  It can't due to Unity.   
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
#pragma warning disable IDE0044 // Add readonly modifier



public class Bookstore : MonoBehaviour
{
    private bool bDebug = false;
    private bool bKeyboardUse = true;  // You must also disable Graphic Raycaster (Script) in BookStore.BookStoreCnvs
    private enum enlist { BookList, SelectedBooks};
    private enum enAction {Main, Purchase, Sell };
    private enum enSection { ActionButtons, LeftBookList, RightBookList};
    private enum enScroll { UP, DOWN };
    // Start is called before the first frame update

    // Variable for updating text in panels
    public GameObject BookstoreInterior;        // For enabling and disabling the Bookstore
    public GameObject BookPurchases;            // For enabling and disabling the Purchasing panel
    public GameObject BookSales;                // For enabling and disabling the Sales
    public GameObject BookstoreHelp;            //

    public GameObject BookTemplate;             // Template for the button list
    public GameObject UnOwnedBooksContent;      // Content of the un-owned books list
    public GameObject OwnedBooksContent;        // Content of the owned books list
    public GameObject ToBePurchasedContent;     // Content of the to be purchased list
    public GameObject ToBeSoldContent;          // Content of the to be sold list

    public List<Dropdown.OptionData> UnOwnedBooksOptions = new List<Dropdown.OptionData>();
    public List<Dropdown.OptionData> OwnedBooksOptions = new List<Dropdown.OptionData>();
    public List<Dropdown.OptionData> ToBePurchasedOptions = new List<Dropdown.OptionData>();
    public List<Dropdown.OptionData> ToBeSoldOptions = new List<Dropdown.OptionData>();

    public TextMeshProUGUI txtPurchaseBookDescription;
    public TextMeshProUGUI txtSellBookDescription;
    public float fMoney;  //Not used
    private float fTempMoney;
    public TextMeshProUGUI BookStoreMoney;
    public TextMeshProUGUI PurchasesMoney;
    public TextMeshProUGUI SalesMoney;
    public TextMeshProUGUI HelpText;

    private enAction Action = enAction.Purchase;
    private enSection Section = enSection.ActionButtons;
    private GameObject LeftContent;             // Content of the to be purchased list
    private GameObject RightContent;            // Content of the to be sold list
    private List<Dropdown.OptionData> LeftOptions;
    private List<Dropdown.OptionData> RightOptions;
    private ScrollRect LeftScrollView;
    private ScrollRect RightScrollView;
    private Button btnBuySell;
    private Button btnCancel;


    public BookStoreItem[] BookStoreItems;

    private int SelectedBookIndex = -1;

    private Color selectedColor = Color.yellow;
    private Color normalColor = Color.white;
    private Color selectedTextColor = Color.white;
    private Color normalTextColor = Color.black;

    public ScrollRect UnOwnedBooksScrollView;
    public ScrollRect OwnedBooksScrollView;
    public ScrollRect ToBePurchasedScrollView;
    public ScrollRect ToBeSoldScrollView;
    private bool bHelpVisible = false;


    void Start()
    {
        Action = enAction.Main;
        Section = enSection.ActionButtons;

        BookStoreItems = BS_Items;

        LeftOptions = null;
        // Make the "Bookstore main room" panel visible then hide the "Buy" and "Sell" panels.
        BookstoreInterior.SetActive(true);
        BookSales.SetActive(false);
        BookPurchases.SetActive(false);
        BookstoreHelp.SetActive(false);

        // Ensure that the lists are emtpy before populating.
        ClearBooks(UnOwnedBooksContent, UnOwnedBooksOptions);
        ClearBooks(OwnedBooksContent, OwnedBooksOptions);
        ClearBooks(ToBePurchasedContent, ToBePurchasedOptions);
        ClearBooks(ToBeSoldContent, ToBeSoldOptions);

        //fMoney = SceneChange.currency;
        UpdateBuySellButtons();

        if (bDebug)
        {

            SceneChange.currency = 5000f;
        }

        SceneChange.currency = 5000;

        BookStoreMoney.SetText("Money: $" + SceneChange.currency);
        PurchasesMoney.SetText("Money: $" + SceneChange.currency);
        SalesMoney.SetText("Money: $" + SceneChange.currency);

        SelectedBookIndex = -1;
    }


    // Update is called once per frame
    void Update()
    {
        if (bKeyboardUse && !bHelpVisible)
        {
            if (Action == enAction.Purchase || Action == enAction.Sell)
            {
                int NewBookIndex = -1;
                GameObject oContent = null;
                List<Dropdown.OptionData> oOptions = null;
                ScrollRect oScrollView = null;

                if (Section == enSection.LeftBookList) {
                    oContent = LeftContent;
                    oOptions = LeftOptions;
                    oScrollView = LeftScrollView;
                }
                else if (Section == enSection.RightBookList)
                {
                    oContent = RightContent;
                    oOptions = RightOptions;
                    oScrollView = RightScrollView;
                }


                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    SoundEffect(1f);
                    if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt))
                    {

                    }
                    else
                    {
                        if (Action == enAction.Purchase || Action == enAction.Sell)
                        {
                            // Move from the right list to the buttons on the left
                            if (Section == enSection.RightBookList)
                            {
                                UnSetAllBooks(RightContent);
                                btnBuySell.Select();
                                btnBuySell.OnSelect(null);
                                Section = enSection.ActionButtons;
                            }
                            //  Move from the left list to the right if the right has visible books
                            else if (Section == enSection.LeftBookList && (NewBookIndex = FindFirst(RightContent)) > -1) {
                                UnSetAllBooks(LeftContent);
                                EnterBookButton(RightContent, FindFirst(RightContent), enSection.RightBookList);
                                SelectedBookIndex = NewBookIndex;
                            }
                            // Move from the buttons on the left to the books in the center if the cent has visible books
                            else if (Section == enSection.ActionButtons && (NewBookIndex = FindFirst(LeftContent)) > -1)
                            {
                                // Disable the selected action button
                                EventSystem.current.SetSelectedGameObject(null);
                                EnterBookButton(LeftContent, FindFirst(LeftContent), enSection.LeftBookList);
                                SelectedBookIndex = NewBookIndex;
                                LeftScrollView.verticalScrollbar.value = 1f;
                            }
                            // Move from the buttons on the left to the books on the far right if it has visible books
                            else if (Section == enSection.ActionButtons && (NewBookIndex = FindFirst(RightContent)) > -1)
                            {
                                // Disable the selected action button
                                EventSystem.current.SetSelectedGameObject(null);
                                EnterBookButton(RightContent, FindFirst(RightContent),enSection.RightBookList);
                                SelectedBookIndex = NewBookIndex;

                                // Bug, oScrollView not set
                                RightScrollView.verticalScrollbar.value = 1f;
                            }
                            else
                            {
                                if (Section == enSection.LeftBookList)
                                {
                                    ExitButton(LeftContent, SelectedBookIndex);
                                }
                                else if (Section == enSection.RightBookList) 
                                {
                                    ExitButton(RightContent, SelectedBookIndex);
                                }
                                Section = enSection.ActionButtons;
                                btnBuySell.Select();
                                btnBuySell.OnSelect(null);
                            }

                        }
                    }
                }
                else if (Section == enSection.ActionButtons && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow )))
                {
                    if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() == btnBuySell)
                    {
                        btnCancel.Select();
                        btnCancel.OnSelect(null);
                    }
                    else
                    {
                        btnBuySell.Select();
                        btnBuySell.OnSelect(null);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) && Section == enSection.LeftBookList)
                {
                    if (MoveBookRight(SelectedBookIndex))
                    {
                        // Find the next selectable button, if there aren't any, find first prior selectable button.
                        if (SelectedBookIndex != (NewBookIndex = FindNext(oContent, SelectedBookIndex)))
                        {
                            EnterBookButton(oContent, NewBookIndex, Section);
                        }
                        else if (SelectedBookIndex != (NewBookIndex = FindPrevious(oContent, SelectedBookIndex)))
                        {
                            EnterBookButton(oContent, NewBookIndex, Section);
                        }

                        SelectedBookIndex = NewBookIndex;

                        if (FindFirst(LeftContent) == -1)
                        {
                            SelectedBookIndex = FindFirst(RightContent);
                            EnterBookButton(RightContent, SelectedBookIndex, enSection.RightBookList);
                        }
                        SoundEffect(.8f);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow ) && Section == enSection.RightBookList)
                {
                    MoveBookLeft(SelectedBookIndex);

                    // Find the next selectable button, if there aren't any, find first prior selectable button.
                    if (SelectedBookIndex != (NewBookIndex = FindNext(oContent, SelectedBookIndex)))
                    {
                        EnterBookButton(oContent, NewBookIndex, Section);
                    }
                    else if (SelectedBookIndex != (NewBookIndex = FindPrevious(oContent, SelectedBookIndex)))
                    {
                        EnterBookButton(oContent, NewBookIndex, Section);
                    }
                    SelectedBookIndex = NewBookIndex;

                    if (FindFirst(RightContent) == -1)
                    {
                        SelectedBookIndex = FindFirst(LeftContent);
                        EnterBookButton(LeftContent, SelectedBookIndex, enSection.LeftBookList);
                    }
                    SoundEffect(.6f);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) && SelectedBookIndex < (oOptions.Count - 1) && (Section == enSection.LeftBookList || Section == enSection.RightBookList))
                {
                    // Find the next selectable button, if there aren't any, find first prior selectable button.
                    if (SelectedBookIndex <= (NewBookIndex = FindNext(oContent, SelectedBookIndex)))
                    {
                        EnterBookButton(oContent, NewBookIndex, Section);
                    }
                    else if (SelectedBookIndex != (NewBookIndex = FindPrevious(oContent, SelectedBookIndex)))
                    {
                        EnterBookButton(oContent, NewBookIndex, Section);
                    }
                    SelectedBookIndex = NewBookIndex;

                    if (!isVisible(oScrollView, oContent, SelectedBookIndex))
                    {
                        AdjustScrollBar(oScrollView, enScroll.DOWN);
                    }
                }       
                else if (Input.GetKeyDown(KeyCode.UpArrow) && SelectedBookIndex > 0 && (Section == enSection.LeftBookList || Section == enSection.RightBookList))
                {
                    // Find a prior selectable button
                    // Find the next selectable button, if there aren't any, find first prior selectable button.
                    if (SelectedBookIndex >= (NewBookIndex = FindPrevious(oContent, SelectedBookIndex)))
                    {
                        EnterBookButton(oContent, NewBookIndex, Section);
                    }
                    else if (SelectedBookIndex != (NewBookIndex = FindNext(oContent, SelectedBookIndex)))
                    {
                        EnterBookButton(oContent, NewBookIndex, Section);
                    }
                    SelectedBookIndex = NewBookIndex;

                    if (!isVisible(oScrollView, oContent, SelectedBookIndex))
                    {
                        AdjustScrollBar(oScrollView, enScroll.UP);
                    }
                }
            }      
        }
    }

    /*
     * Determine if an item in the listbox is visible to the user.
     * Used to determine if the listbox needs to be scrolled up or down.
     */
    private bool isVisible(ScrollRect p_oScrollView, GameObject p_oContents, int p_iIndex)
    {
        float DistanceMarginForLoad = -77.4f;

        RectTransform scrollTransform = p_oScrollView.GetComponent<RectTransform>();
        Button child = p_oContents.transform.GetChild(p_iIndex).gameObject.GetComponent<Button>();

        RectTransform childTransform = child.GetComponent<RectTransform>();
        Vector3 positionInWord = childTransform.parent.TransformPoint(childTransform.localPosition);
        Vector3 positionInScroll = scrollTransform.InverseTransformPoint(positionInWord);
        float childMinY = positionInScroll.y + childTransform.rect.yMin;
        float childMaxY = positionInScroll.y + childTransform.rect.yMax;
  
        float checkRectMinY = scrollTransform.rect.yMin - DistanceMarginForLoad;
        float checkRectMaxY = scrollTransform.rect.yMax + DistanceMarginForLoad;

        return (childMaxY >= checkRectMinY && childMinY <= checkRectMaxY) ? true : false;
    }
    /*
     * Move the scroll bar up or down to keep active item visible.
     */
    private void AdjustScrollBar(ScrollRect p_oScrollView, enScroll p_enDirection)
    {
        // Scroll bar movement/size is determined by the visible children
        // 7 is the maximum number of items visible in the list (Should be calculated)
        float Adjustment = 1f / (CountVisible(p_oScrollView) - 7);
        
        if (enScroll.UP == p_enDirection)
        {
            // If scrolling up, add the adjustment to the scrollbar
            p_oScrollView.verticalScrollbar.value += Adjustment;
        }
        else
        {
            // If scrolling up, subtract the adjustment to the scrollbar
            p_oScrollView.verticalScrollbar.value -= Adjustment;
        }
    }



    /*
     * Count the number of active items in the list
     * *** Used to determine the size of the scrollbar
     */ 
    private int CountVisible(ScrollRect p_oScrollView)
    {
        int iVisible = 0;
        for (int i = 0; i < p_oScrollView.content.transform.childCount; i++)
        {
            if (p_oScrollView.content.transform.GetChild(i).gameObject.GetComponent<Button>().IsActive())
            {
                iVisible++;

            }
        }

        return iVisible;
    }



    /*
     * Find the first visible item in the content list
     */ 
    private int FindFirst(GameObject p_oContents)
    {
        for (int i = 0; i < p_oContents.transform.childCount; i++)
        {
            if (p_oContents.transform.GetChild(i).gameObject.GetComponent<Button>().IsActive())
            {
                return i;
            }
        }
 
        return -1;
    }

    /*
     * Find an active item in the list after the current one or 
     * return the current item if it is the last active item.
     */
    private int FindNext(GameObject p_oContents, int p_iSelectedBookIndex)
    {
        int newIndex = p_iSelectedBookIndex;
        if (p_iSelectedBookIndex < p_oContents.transform.childCount - 1)
        {
            for (int i = p_iSelectedBookIndex + 1; i < p_oContents.transform.childCount; i++)
            {
                if (p_oContents.transform.GetChild(i).gameObject.GetComponent<Button>().IsActive())
                {
                    newIndex = i;
                    break;
                }
            }
        }
        return newIndex;
    }

    /*
     * Find an active item in the list prior to the current one or 
     * return the current item if it is the first active item.
     */
    private int FindPrevious(GameObject p_oContents, int p_iSelectedBookIndex)
    {
        int newIndex = p_iSelectedBookIndex;
        if (p_iSelectedBookIndex >= 1)
        {
            for (int i = p_iSelectedBookIndex - 1; i >= 0; i--)
            {
                if (p_oContents.transform.GetChild(i).gameObject.GetComponent<Button>().IsActive())
                {
                    newIndex = i;
                    break;
                }
            }
        }

        return newIndex;
    }

    /* 
     * Highlight the selected index if it is in range and active 
     */
    private void HighlightItem(GameObject p_oContents, int p_iIndex)
    {
        if (p_iIndex >= 0 && 
            p_iIndex < p_oContents.transform.childCount && 
            p_oContents.transform.GetChild(p_iIndex).gameObject.GetComponent<Button>().IsActive())
        {
            SetItem(p_oContents, p_iIndex);
        }
    }

    public void PurchaseBooks()
    {
        BookPurchases.SetActive(true);
        BookstoreInterior.SetActive(false);

        // Set the state for purchases
        Action = enAction.Purchase;
        Section = enSection.LeftBookList;
        btnBuySell = GameObject.Find("btnAcceptPurchase").GetComponent<Button>();
        btnCancel = GameObject.Find("btnCancelPurchase").GetComponent<Button>();
 

        // Items for list navigation
        LeftScrollView = UnOwnedBooksScrollView;
        RightScrollView = ToBePurchasedScrollView;
        LeftContent = UnOwnedBooksContent;
        RightContent = ToBePurchasedContent;
        LeftOptions = UnOwnedBooksOptions;
        RightOptions = ToBePurchasedOptions;

        fTempMoney = SceneChange.currency;
        PurchasesMoney.SetText("Money: $" + fTempMoney);

        ClearBooks(UnOwnedBooksContent, UnOwnedBooksOptions);
        ClearBooks(ToBePurchasedContent, ToBePurchasedOptions);

        // Find all books owned and place them in a tilde seperated list.
        // (i.e.) Brain Teasers~Principles of Management~
        // They will not be included in the list
        // Player_Inventory.cs
        string sOwns = "";
        if (!bDebug)
        {
            foreach (Inventory_Item li in SceneChange.items)
            {
                sOwns += li.name + "~";
            }
        }

        List<string> oBooks = new List<string>();

        foreach (BookStoreItem i in BookStoreItems)
        {
            oBooks.Add(i.Title);
        }

        // Populate the 
        var buttons = oBooks.ToArray()
        .Select(name => new Dropdown.OptionData(name))
        .ToList();

        AddBooks(LeftOptions, buttons, LeftContent, true, false, enlist.BookList, sOwns);
        AddBooks(RightOptions, buttons, RightContent, false, false, enlist.SelectedBooks, "");

        Button btnHelp = GameObject.Find("btnPurchaseHelp").GetComponent<Button>();
        ButtonHighlighted(btnHelp, true);

        if (FindFirst(LeftContent) == -1)
        {
            Section = enSection.ActionButtons;
        }
        else
        {
            Section = enSection.LeftBookList;
            SelectedBookIndex = FindFirst(LeftContent);
            HighlightItem(LeftContent, SelectedBookIndex);
        }

    }

    public void AcceptPurchase(bool p_bYesNo)
    {
        // If p_bYesNo is true, process purchse.
        // Regardless of the p_bYesNod value, return to the bookstore main screen
        if (p_bYesNo)
        {
            //float fCurrency = 0;

            for (int i = 0; i < RightContent.transform.childCount; i++)
            {
                if (RightContent.transform.GetChild(i).gameObject.GetComponent<Button>().IsActive())
                {

                    //Book_Item book = new Book_Item();
                    Book_Item book = ScriptableObject.CreateInstance<Book_Item>();
                    //Inventory_Item book = Resources.Load<Inventory_Item>("Square");

                    book.name = BookStoreItems[i].Title;
                    book.bookStats = BookStoreItems[i].bookStoreStats;
                    book.isDefaultItem = false;
                    book.value = BookStoreItems[i].Cost;
                    book.icon = Resources.Load<Sprite>(BookStoreItems[i].book );
                    
                    book.Use_Dialogue = new string[5];
                    book.Use_Dialogue[0] = "#MULTI_START##INNER_DIALOGUE_BEGIN#I read the book...";
                    book.Use_Dialogue[1] = BookStoreItems[i].Title;
                    book.Use_Dialogue[2] = "I feel a little bit smarter after reading it."+book.bookStats; //added by Don Murphy see Inventory_Item.cs
                    book.Use_Dialogue[3] = "#MULTI_END#This will help me reach my career goals!";
                    book.Use_Dialogue[4] = "You've already read this book";
                    book.details = BookStoreItems[i].Description;

                    if (!bDebug) SceneChange.items.Add(book);
                }
            }
            SceneChange.currency = fTempMoney;
        }
        BookstoreInterior.SetActive(true);
        BookPurchases.SetActive(false);
        Action = enAction.Main;

        BookStoreMoney.SetText("Money: $" + SceneChange.currency);

        UpdateBuySellButtons();
    }

    public void SellBooks()
    {
        BookSales.SetActive(true);
        BookstoreInterior.SetActive(false);

        fTempMoney = SceneChange.currency;
        SalesMoney.SetText("Money: $" + fTempMoney);

        // Set the state for sales
        Action = enAction.Sell;
        Section = enSection.LeftBookList;
        btnBuySell = GameObject.Find("btnAcceptSale").GetComponent<Button>();
        btnCancel = GameObject.Find("btnCancelSale").GetComponent<Button>();


        // Items for list navigation
        LeftScrollView = OwnedBooksScrollView;
        RightScrollView = ToBeSoldScrollView;
        LeftContent = OwnedBooksContent;
        RightContent = ToBeSoldContent;
        LeftOptions = OwnedBooksOptions;
        RightOptions = ToBeSoldOptions;

        // If the player has nothing to sell, the cancel key should be defaulted

    
        ClearBooks(OwnedBooksContent, OwnedBooksOptions);
        ClearBooks(ToBeSoldContent, ToBeSoldOptions);

        // Find all books owned and removed them from the list
        string sOwns = "";
        if (!bDebug)
        {
            foreach (Inventory_Item li in SceneChange.items)
            {
                sOwns += li.name + "~";
            }
        }
        else
        {
            sOwns += "Brain Teasers~";
        }


        List<string> oBooks = new List<string>();

        foreach (BookStoreItem i in BookStoreItems)
        {
            oBooks.Add(i.Title);
        }

        var buttons = oBooks.ToArray()
        .Select(name => new Dropdown.OptionData(name))
        .ToList();

        AddBooks(LeftOptions, buttons, LeftContent, false, true, enlist.BookList, sOwns);
        AddBooks(RightOptions, buttons, RightContent, false, false, enlist.SelectedBooks, "");

        Button btnHelp = GameObject.Find("btnSalesHelp").GetComponent<Button>();
        ButtonHighlighted(btnHelp, true);
        if (FindFirst(LeftContent) == -1)
        {
            Section = enSection.ActionButtons;
        }
        else
        {
            Section = enSection.LeftBookList;
            SelectedBookIndex = FindFirst(LeftContent);
            HighlightItem(LeftContent, SelectedBookIndex);
        }
    }
    public void AcceptSale(bool p_bYesNo)
    {
        // If p_bYesNo is true, process sale.
        // Regardless of the p_bYesNod value, return to the bookstore main screen
        if (p_bYesNo)
        {
            // Look at all of the books in the right side list (visible and invisible)
            for (int i = 0; i < RightContent.transform.childCount; i++)
            {
                // If its visible, process it
                if (RightContent.transform.GetChild(i).gameObject.GetComponent<Button>().IsActive())
                {
                    int iIndex = -1;
                    int iRemove = -1;

                    // Search for the book in the players inventory
                    foreach (Inventory_Item li in SceneChange.items)
                    {
                        iIndex++;
                        // If found, remove it.
                        if (li.name.Equals(BookStoreItems[i].Title))
                        {
                            iRemove = iIndex;
                        }
                    }
                    if (!bDebug) SceneChange.items.RemoveAt(iRemove);
                }
            }
            SceneChange.currency = fTempMoney;
        }
        BookstoreInterior.SetActive(true);
        BookSales.SetActive(false);

        BookStoreMoney.SetText("Money: $" + SceneChange.currency);
        Action = enAction.Main;
        UpdateBuySellButtons();
    }

    public void ReturnToGame()
    {
        //Cursor.visible = true;
        SceneManager.LoadScene("World Map");
    }

    private void UpdateBuySellButtons()
    {

        //GameObject.Find("btnSell").GetComponent<Button>().interactable = true;
        GameObject.Find("btnPurchase").GetComponent<Button>().interactable = true;

        if (bKeyboardUse)
        {
            //Button btnExit = GameObject.Find("btnExit").GetComponent<Button>();
            //btnExit.Select();
            //btnExit.OnSelect(null);
        }
    }




    /*
     * Remove books from list
     */
    public void ClearBooks(GameObject p_oContent, List<Dropdown.OptionData> p_oOptions)
    {
        foreach (Transform t in p_oContent.transform)
        {
            Destroy(t.gameObject);
        }

        p_oOptions.Clear();
    }


    /*
     * Add Books to list.
     * The majority of the code is setting up listeners for the buttons that represent books.
     * This code is not currently utilized since the interface was changed to keyboard only.
     * It has been left in place incase we want to revert back to using a mouse.
     */ 
    private void AddBooks(List<Dropdown.OptionData> p_oListbox, List<Dropdown.OptionData> p_oItems, GameObject p_oContent, bool p_bActive, bool p_bState, enlist p_enList, string p_sState)
    {
        foreach (var option in p_oItems)
        {
            var copy = Instantiate(BookTemplate);
            copy.transform.SetParent(p_oContent.transform);
            copy.transform.localPosition = Vector3.zero;
            copy.transform.localScale = Vector3.one;

            copy.SetActive(p_bActive);
            if (p_sState.Contains(option.text+"~"))
            {
                copy.SetActive(p_bState);
            }

            copy.GetComponentInChildren<TextMeshProUGUI>().text = option.text;

            int copyOfIndex = p_oListbox.Count;

            // Set event for what happens when the mouse enters the button
            EventTrigger enterTrigger = copy.gameObject.AddComponent<EventTrigger>();
            var pointerEnter = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };


            // Set event for what happens when the mouse exits the button
            EventTrigger exitTrigger = copy.gameObject.AddComponent<EventTrigger>();
            var pointerExit = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };

            switch (p_enList)
            {
                case enlist.BookList:
                    copy.GetComponent<Button>().onClick.AddListener(() => { SelectBook(copyOfIndex); });

                    pointerEnter.callback.AddListener((e) => EnterBookButton(p_oContent, copyOfIndex, enSection.LeftBookList));
                    pointerExit.callback.AddListener((e) => ExitBookButton(p_oContent, copyOfIndex, enSection.LeftBookList));
                    break;
                case enlist.SelectedBooks:
                    copy.GetComponent<Button>().onClick.AddListener(() => { UnSelectBook(copyOfIndex); });

                    pointerEnter.callback.AddListener((e) => EnterBookButton(p_oContent, copyOfIndex, enSection.RightBookList));
                    pointerExit.callback.AddListener((e) => ExitBookButton(p_oContent, copyOfIndex, enSection.RightBookList));
                    break;

            }

            enterTrigger.triggers.Add(pointerEnter);
            exitTrigger.triggers.Add(pointerExit);


            p_oListbox.Add(option);

        }
    }

 
    private void SelectBook(int p_iIndex)
    {
        SelectedBookIndex = p_iIndex;
        if (MoveBookRight(SelectedBookIndex))
        {
            int NewBookIndex;
            // Find the next selectable button, if there aren't any, find first prior selectable button.
            if (SelectedBookIndex != (NewBookIndex = FindNext(LeftContent, SelectedBookIndex)))
            {
                EnterBookButton(LeftContent, NewBookIndex, Section);
            }
            else if (SelectedBookIndex != (NewBookIndex = FindPrevious(LeftContent, SelectedBookIndex)))
            {
                EnterBookButton(LeftContent, NewBookIndex, Section);
            }

            SelectedBookIndex = NewBookIndex;

            if (FindFirst(LeftContent) == -1)
            {
                SelectedBookIndex = FindFirst(RightContent);
                EnterBookButton(RightContent, SelectedBookIndex, enSection.RightBookList);
            }
            SoundEffect(.8f);
        }
    }


    private void UnSelectBook(int p_iIndex)
    {
        int NewBookIndex;
        MoveBookLeft(SelectedBookIndex);

        // Find the next selectable button, if there aren't any, find first prior selectable button.
        if (SelectedBookIndex != (NewBookIndex = FindNext(RightContent, SelectedBookIndex)))
        {
            EnterBookButton(RightContent, NewBookIndex, Section);
        }
        else if (SelectedBookIndex != (NewBookIndex = FindPrevious(RightContent, SelectedBookIndex)))
        {
            EnterBookButton(RightContent, NewBookIndex, Section);
        }
        SelectedBookIndex = NewBookIndex;

        if (FindFirst(RightContent) == -1)
        {
            SelectedBookIndex = FindFirst(LeftContent);
            EnterBookButton(LeftContent, SelectedBookIndex, enSection.LeftBookList);
        }
        SoundEffect(.6f);
    }


    private void EnterBookButton(GameObject p_oContent, int p_iIndex, enSection p_enSection)
    {
        Section = p_enSection;
        EnterButton(p_oContent, LeftOptions.Count, p_iIndex);

        String essential = "<br><br>Essential for the following career paths: ";
        bool display = false;

        if (BookStoreItems[p_iIndex].Flags.Contains("IT"))
        {
            essential += "IT Engineer, ";
            display = true;
        }
        if (BookStoreItems[p_iIndex].Flags.Contains("HR"))
        {
            essential += "HR Manager, ";
            display = true;
        }

        if (BookStoreItems[p_iIndex].Flags.Contains("SE"))
        {
            essential += "Software Engineer";
            display = true;
        }

        if (Action == enAction.Purchase)
        {
            txtPurchaseBookDescription.SetText("Price: $ " + BookStoreItems[p_iIndex].Cost + "<br><br>" + BookStoreItems[p_iIndex].Description + ((display) ? essential : ""));
        }
        else
        {
            txtSellBookDescription.SetText(BookStoreItems[p_iIndex].Description);            
        }

        if (enSection.LeftBookList == p_enSection) UnSetAllBooks(RightContent);
        else UnSetAllBooks(LeftContent);

        SelectedBookIndex = p_iIndex;
    }

    private void ClearItem(GameObject p_oContent, int p_iIndex)
    {
        if (p_iIndex < 0) return;

        SetButtonColor(p_oContent, p_iIndex, normalColor, normalTextColor);
    }

    public void EnterButton(GameObject p_oContent, int p_iItemCount, int p_iIndex)
    {
        // There are times when leaving the button does not set the
        // background to normal so I clear them all then set the one
        // that the mouse is over.
        for (int iIndex = 0; iIndex < p_iItemCount; iIndex++)
        {
            ClearItem(p_oContent, iIndex);
        }

        SetItem(p_oContent, p_iIndex);

        //txtSkillDescription.SetText(ApplyingFor.GetDescription(p_iIndex));
    }

    /* 
     * Set the button color back to normal
     */
    public void ExitButton(GameObject p_oContent, int p_iIndex)
    {
        ClearItem(p_oContent, p_iIndex);
    }
    private void SetItem(GameObject p_oContent, int p_iIndex)
    {
        if (p_iIndex < 0) return;

        SetButtonColor(p_oContent, p_iIndex, selectedColor, selectedTextColor);
    }

    private void UnSetAllBooks(GameObject p_oContent)
    {
        for (int i = 0; i < p_oContent.transform.childCount; i++)
        {
            SetButtonColor(p_oContent, i, normalColor, normalTextColor);
        }
    }


    private void SetButtonColor(GameObject p_oContent, int p_iIndex, Color p_stbackground, Color p_stforeground)
    {
        var oButton = p_oContent.transform.GetChild(p_iIndex).GetComponent<Button>();

        ColorBlock stColorBlock = new ColorBlock()
        {
            normalColor = p_stbackground,
            colorMultiplier = oButton.colors.colorMultiplier,
            disabledColor = oButton.colors.disabledColor,
            fadeDuration = oButton.colors.fadeDuration,
            highlightedColor = p_stbackground,
            pressedColor = p_stbackground
        };

        oButton.colors = stColorBlock;
        oButton.GetComponentInChildren<TextMeshProUGUI>().color = p_stforeground;
    }

    private void SetButtonColor(Button p_oButton, Color p_stbackground, Color p_stforeground)
    {
        ColorBlock stColorBlock = new ColorBlock()
        {
            normalColor = p_stbackground,
            colorMultiplier = p_oButton.colors.colorMultiplier,
            disabledColor = p_oButton.colors.disabledColor,
            fadeDuration = p_oButton.colors.fadeDuration,
            highlightedColor = p_stbackground,
            pressedColor = p_stbackground
        };

        p_oButton.colors = stColorBlock;
        p_oButton.GetComponentInChildren<TextMeshProUGUI>().color = p_stforeground;
    }

    private bool MoveBookRight(int p_iIndex)
    {
        bool bReturn = true;

        if (Action == enAction.Purchase)
        {
            if (BookStoreItems[p_iIndex].Cost <= fTempMoney)
            {
                fTempMoney -= BookStoreItems[p_iIndex].Cost;
                HideInList(LeftContent, p_iIndex);
                ShowInList(RightContent, p_iIndex);
                ClearItem(RightContent, p_iIndex);

                SelectedBookIndex = p_iIndex;
                PurchasesMoney.SetText("Money: $" + fTempMoney);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                SelectedBookIndex = p_iIndex;

                txtPurchaseBookDescription.SetText("You do not have enough money for this book");
                bReturn = false;
            }
        }
        else
        {
            fTempMoney += BookStoreItems[p_iIndex].Cost;
            HideInList(LeftContent, p_iIndex);
            ShowInList(RightContent, p_iIndex);
            SelectedBookIndex = p_iIndex;
            SalesMoney.SetText("Money: $" + fTempMoney);
        }
        return bReturn;
    }

    private void MoveBookLeft(int p_iIndex)
    {
        if (Action == enAction.Purchase)
        {
            fTempMoney += BookStoreItems[p_iIndex].Cost;
            PurchasesMoney.SetText("Money: $" + fTempMoney);
        }
        else
        {
            fTempMoney -= BookStoreItems[p_iIndex].Cost;
            SalesMoney.SetText("Money: $" + fTempMoney);
        }
        HideInList(RightContent, p_iIndex);
        ShowInList(LeftContent, p_iIndex);

        SelectedBookIndex = p_iIndex;
    }


    private void ShowInList(GameObject p_oContent, int p_iIndex) => ItemState(p_oContent, p_iIndex, true);

    private void HideInList(GameObject p_oContent, int p_iIndex) => ItemState(p_oContent, p_iIndex, false);

    private void ItemState(GameObject p_oContent, int p_iIndex, bool p_bState) => p_oContent.transform.GetChild(p_iIndex).gameObject.SetActive(p_bState);

    /* 
     * Set the button color back to normal
     */
    private void ExitBookButton(GameObject p_oContent, int p_iIndex, enSection p_enSection)
    {
        Section = p_enSection;
        ClearItem(p_oContent, p_iIndex);
    }

    public void SoundEffect(float p_fPitch)
    {
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audio.pitch = p_fPitch;
        audio.PlayOneShot((AudioClip)Resources.Load("CollectSFX"));
    }

    private void ButtonHighlighted(Button p_oButton, bool p_bHighlight)
    {
        p_oButton.Select();
        if (p_bHighlight)
        {
            p_oButton.OnSelect(null);
        }
        else
        {
            p_oButton.OnDeselect(null);
        }
    }

    public void PurchasingHelp()
    {
        Action = enAction.Purchase;
        bHelpVisible = true;
        BookPurchases.SetActive(false);
        BookstoreHelp.SetActive(true);
        
        string helpMessage =
        "<margin=1em><br><b><u>Purchasing Books:</b></u><br><br>" +
        "<margin=2em>Highlighting a book in the “Book inventory” or “To be purchased” lists will display the details pertaining to that book.<br><br>" +
        "Selecting a book from the “Book inventory” list moves it to the “To be purchased” list.<br><br>" +
        "Selecting a book from the “To be purchased” list moves it back to the “Book inventory” list.<br><br>" +
        "If the keyboard is being used:<br>" +
        "<margin=4em>When a book is selected in the “Book inventory” list, the right arrow key moves it to the <nobr>“To be purchased” list.<br><br>" +
        "When a book is selected in the “To be purchased” list, the left arrow key moves it back to the <nobr>“Book inventory” list.<br><br>" +
        "Use the Tab key to transition between the “Book inventory”, “To be purchased” and <nobr>“Buy/Cancel” buttons.<br><br><br>" +
        "<margin=2em>To purchase the book(s), press the “Buy” button.  To abort the transaction, press the “Cancel” button.<br>";

        HelpText.SetText(helpMessage);
    }

    public void SalseHelp()
    {
        Action = enAction.Sell;
        bHelpVisible = true;
        BookSales.SetActive(false);
        BookstoreHelp.SetActive(true);

        string helpMessage =
        "<margin=1em><br><b><u>Selling Books:</b></u><br><br>" +
        "<margin=2em><color=black>Highlighting a book in the “Player inventory” or “To be sold” lists will display the details pertaining to that book.<br><br>" +
        "Selecting a book from the “Player inventory” list moves it to the “To be sold” list.<br><br> " +
        "Selecting a book from the “To be sold” list moves it to the “Player inventory” list.<br><br> " +
        "If the keyboard is being used:<br>" +
        "<margin=4em>When a book is selected in the “Player inventory” list, the right arrow key moves it back to the <nobr>“To be sold” list.<br><br>" +
        "When a book is selected in the “To be sold” list the left arrow key moves it to the <nobr>“Book inventory” list.<br><br>" +
        "Use the Tab key to transition between the “Player inventory”, “To be sold” and <nobr>“Sell/Cancel” buttons.<br><br>" +
        "<margin=2em>To purchase the book(s) press the “Sell” button.  To abort the transaction, press the “Cancel” button.<br>";


        HelpText.SetText(helpMessage);

    }

    public void helpReturn()
    {
        if (Action == enAction.Purchase)
        {
            BookPurchases.SetActive(true);
            BookstoreHelp.SetActive(false);
        }
        else if (Action == enAction.Sell)
        {
            BookSales.SetActive(true);
            BookstoreHelp.SetActive(false);
        }
        bHelpVisible = false;
    }
    //Resources.Load<Sprite>("Assets/Sprites/Items/Book_1")
    public static BookStoreItem[] BS_Items =
    {
            new BookStoreItem ("What to Say and How to Say It!",                  50f, "SK",          "Book_1", "Communication Book to improve skill", "Dialog","No stat increase"),
            new BookStoreItem ("Brain Teasers",                                   50f, "SK",          "Book_3", "Critical Thinking Book to improve Skill", "", "#INCREASE#*Crit* +1 to Critical thinking!"),
            new BookStoreItem ("Team Synergy",                                    20f, "HR",          "Book_1", "How to create Tem Synergy while working with your employees, and  with their unique life experiences, perspectives, talents, and communication styles.", "","No stat increase"),
            new BookStoreItem ("Hiring Manager's Guide to Everything",            80f, "HR",          "Book_3", "This book covers a basic overview of the kinds of things that a Hiring Manager would need to know, such as conducting job analysis, planning a recruiting strategy, prescreening candidates, and asking the right questions.", "","No stat increase"),
            new BookStoreItem ("Principles of Management",                        60f, "IT~HR~SE",    "Book_1", "A guide to Fayol's Principles of Management. It talks about concepts such as the 14 Principles of Management and the 5 Functions of Management: planning, organizing, staffing, leading, and controlling.", "","No stat increase"),
            new BookStoreItem ("Employment Laws",                                 40f, "HR",          "Book_3", "Discusses previous and current employment laws, including in-depth discussions of The Fair Labor Standards Act, Occupational Safety and Health (OSHA) Laws, Worker's Compensation, and other benefits.", "","No stat increase"),
            new BookStoreItem ("CompTia A plus Certification Prep Questions",     80f, "IT~SE",       "Book_1", "An in-depth guide to the A+ Certification exams, with troubleshooting steps and previous exam examples.", "","No stat increase"),
            new BookStoreItem ("Intro to Hardware",                               40f, "IT",          "Book_3", "An overview of the hardware components in a typical consumer computer, including troubleshooting and installation guides. ", "","No stat increase"),
            new BookStoreItem ("Troubleshooting 101",                             20f, "IT",          "Book_1", "How to troubleshoot anything! Mostly applies to computers and other electronics, but also teaches you troubleshooting as a way of problem solving.", "","No stat increase"),
            new BookStoreItem ("Guide to Networking",                             60f, "IT",          "Book_3", "This book covers the basics of networking, such as types of networks, wiring techniques, and networking devices.", "","No stat increase"),
            new BookStoreItem ("How to Be a Better Leader",                       50f, "SK",          "Book_1", "Professionalism Book to improve skill", "","No stat increase"),
            new BookStoreItem ("Into to Python",                                  20f, "SE",          "Book_3", "A basic overview of the Python programming language, with Pi-specific projects as well as programming basics to get you programming fast!", "","No stat increase"),
            new BookStoreItem ("Software Engineering Principles",                 60f, "SE",          "Book_1", "Covers the most essential basics of Software Engineering, including project management, determining measurables, reducing costs, and much more!", "","No stat increase"),
            new BookStoreItem ("Debugging 101",                                   40f, "SE",          "Book_3", "A brief guide to stepping through your code. Covers the features of most built-in debuggers for many different programming environments!", "","No stat increase"),
            new BookStoreItem ("Game Design",                                     80f, "SE",          "Book_1", "A great introductory book on the concepts of game design, including planning, level design, finding a team, and much more!", "","No stat increase"),
            new BookStoreItem ("Working in a Team",                               50f, "SK",          "Book_3", "Teamwork Book to improve skill", "","No stat increase"),
            new BookStoreItem ("Microswift Office for Dummies",                   50f, "SK",          "Book_1", "Technology Book to improve skill", "","No stat increase"),
            new BookStoreItem ("Principles of Engineering",                       50f, "SE",          "Book_3", "", "","No stat increase"),
            new BookStoreItem ("I Inc Career Planning",                           50f, "IT~HR~SE",    "Book_1", "Teaches students how to market themselves effectively in today's competitive professional environment.", "","No stat increase"),
            new BookStoreItem ("Tiger in the Office",                             50f, "IT~HR~SE",    "Book_3",  "Using lessons developed by entrepreneurs, you will learn how to pursue your next career steps, rediscover buried career goals and learn to take action toward those goals.", "","No stat increase")
    };
}

[System.Serializable]
public class BookStoreItem
{
    public string Title;
    public float Cost;
    public string Flags;
    [TextArea(5, 10)]
    public string Description;
    public Sprite icon;
    [TextArea(5, 10)]
    public string Dialog;
    public string book;
    public string bookStoreStats;

    public BookStoreItem(string p_oTitle, float p_fCost, string p_oFlags, string p_oBook, string p_oDescription, string p_oDialog, string upBookStats)
    {
        Title = p_oTitle;
        Cost = p_fCost;
        Flags = p_oFlags;
        Description = p_oDescription;
        book = p_oBook;
        Dialog = p_oDialog;
        bookStoreStats = upBookStats;

    }
}
