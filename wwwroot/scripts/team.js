(function() 
{
    microsoftTeams.initialize();

    // Check the initial theme user chose and respect it
    microsoftTeams.getContext(function(context)
    {
        if (context && context.theme)
            setTheme(context.theme);
    });

    // Handle theme changes
    microsoftTeams.registerOnThemeChangeHandler(function(theme)
    {
        setTheme(theme);
    });

    // Methods
    
    // Sets the theme according to user preference
    function setTheme(theme)
    {
        if (theme)
        {
            // Possible values for theme: 'default', 'light', 'dark' and 'contrast'
            document.body.className = "theme-" +
                (theme === "default" ? "light" : theme);
        }
    }
})();