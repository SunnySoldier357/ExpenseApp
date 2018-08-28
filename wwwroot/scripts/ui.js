document.querySelector("#search-input")
    .addEventListener("keyup", search, false);

function search()
{
    const INPUT = document.querySelector("#search-input");
    var filter = INPUT.value.toLowerCase();
    var searchingByTitle = document.querySelector("#title-radio-button")
        .checked;
    const TABLE = document.querySelector("#expense-table");
    const TR = TABLE.querySelectorAll("tbody tr:not(#empty-search-result)");

    var filterType = getSearchType();

    var notDisplayed = 0;

    // Loop through all table rows, and hide those that don't match
    // the search query
    for (let i = 0; i < TR.length; i++)
    {
        var tdStatus = TR[i].querySelectorAll("td")[2].innerHTML.toLowerCase();
        const TD = TR[i].querySelectorAll("td")[searchingByTitle ? 1 : 0];
        if (TD)
        {
            if (TD.innerHTML.toLowerCase().indexOf(filter) > -1 &&
                (filterType == "none" || tdStatus == filterType))
            {
                TR[i].style.display = "";
            }
            else
            {
                TR[i].style.display = "none";
                notDisplayed++;
            }
        }
    }

    const TBODY = TABLE.querySelector("tbody");

    if (notDisplayed == TR.length)
    {
        if (null == TBODY.querySelector("#empty-search-result"))
        {
            var tr = document.createElement("tr");
            tr.setAttribute("id", "empty-search-result");

            var td = document.createElement("td");
            td.setAttribute("colspan", "4");

            var div = document.createElement("div");
            div.classList.add("empty-list-label");
            div.innerHTML = "There are no Expense Reports that match the search query.";

            td.appendChild(div);
            tr.appendChild(td);
            TBODY.appendChild(tr);
        }
    }
    else if (notDisplayed != TR.length && null != TBODY.querySelector("#empty-search-result"))
        TBODY.removeChild(TBODY.querySelector("#empty-search-result"));
}

function getSearchType()
{
    const RADIO_GROUPS = document.querySelectorAll(".filter .radio-group");
    console.log(RADIO_GROUPS);
    for (let i = 0; i < RADIO_GROUPS.length; i++)
    {
        const INPUT = RADIO_GROUPS[i].querySelector("input");
        console.log("Checking " + INPUT.getAttribute("value") + "...");
        if (INPUT.checked)
        {
            console.log("CHECKED!");
            return INPUT.getAttribute("value");
        }
    }
    /*
        for (var RADIO_GROUP in RADIO_GROUPS)
        {
            const INPUT = RADIO_GROUP.querySelector("input");
            if (INPUT.checked)
                return INPUT.getAttribute("value");
        }
        */
}