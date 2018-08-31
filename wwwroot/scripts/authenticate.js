// Get User Id
microsoftTeams.getContext(function(context)
{
    if (context && context.userObjectId)
    {
        const ID_FIELD = document.querySelector("#user-id-field");
        if (context.userObjectId)
        {
            if (ID_FIELD.value != context.userObjectId)
            {
                ID_FIELD.value = context.userObjectId;

                document.querySelector("#register-user-form").submit();
            }
        }
    }
});