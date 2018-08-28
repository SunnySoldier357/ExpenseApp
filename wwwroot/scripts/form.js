const STATEMENT_NUMBER_FIELD = document.querySelector("#statement-number-field");
const FROM_FIELD = document.querySelector("#from-field");
const PROJECT_FIELD = document.querySelector("#project-field");

FROM_FIELD.addEventListener("input", statementNumber, false);
PROJECT_FIELD.addEventListener("blur", statementNumber, false);

function statementNumber() 
{
    var string = "";
    var getNumber = true;
    var changedField = false;

    if (FROM_FIELD.value != "")
        string += getDate(FROM_FIELD.value);
    else
    {
        string += "MMYY";
        getNumber = false;
    }

    string += "-";

    if (PROJECT_FIELD.value != "")
        string += PROJECT_FIELD.value;
    else
    {
        string += "CLIENT-PROJECT";
        getNumber = false;
    }

    if (getNumber)
    {
        var result = "";

        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function() 
        {
            if (this.readyState == 4 && this.status == 200)
            {
                result += this.response;

                if (changedField)
                {
                    var value = STATEMENT_NUMBER_FIELD.value;
                    STATEMENT_NUMBER_FIELD.value = value.slice(value.length - 3);
                }

                changedField = true;

                if (result != "")
                {
                    string += "-";
                    if (result.length == 1)
                        string += "0";
                    string += result;
                }
                else
                    string += "-##";

                STATEMENT_NUMBER_FIELD.value = string;
            }
        };
        xhttp.open("POST", "/Form/GetNextIdNumber/" + string, false);
        xhttp.send();
    }
    else
        string += "-##";

    if (!changedField)
        STATEMENT_NUMBER_FIELD.value = string;
}

function getDate(dateString) 
{
    var year = dateString.slice(2, 4);
    var month = dateString.slice(5, 7);

    return month + year;
}

statementNumber();