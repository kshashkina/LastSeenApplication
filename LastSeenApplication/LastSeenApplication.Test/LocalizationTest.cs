using Moq;
using System.Net;
using LastSeenApplication;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

public class LocalizationTests
{
    [Fact]
    public void ChooseLanguage_ReturnsCorrectLanguage()
    {
        // Arrange
        var localization = new Localization();

        // Act
        string resultEN = localization.LanguageKey(1);
        string resultDE = localization.LanguageKey(3);
        string resultFR = localization.LanguageKey(4);

        // Assert
        Assert.Equal("en", resultEN);
        Assert.Equal("de", resultDE);
        Assert.Equal("fr", resultFR);
    }

    [Fact]
    public void Output_ReturnsCorrectText()
    {
        // Arrange
        var localization = new Localization();
        string languageEN = "en";
        string languageDE = "de";

        // Act
        string resultEN = localization.Output(languageEN);
        string resultDE = localization.Output(languageDE);

        // Assert
        Assert.Equal( "What you want to do?\n" +
                      "Have a list of all users - 1\n" +
                      "Have the number of users at the exact time - 2\n" +
                      "Check if the user was online at the exact date - 3\n" +
                      "Prediction about the number of users online - 4\n" +
                      "Prediction about a user being online - 5\n" +
                      "Total amount of time online for a user - 6\n" +
                      "Average time for a user - 7\n" +
                      "Display deleted user - 8", resultEN);
        Assert.Equal( "Was möchten Sie tun?\n" +
                      "Eine Liste aller Benutzer haben - 1\n" +
                      "Die Anzahl der Benutzer zu einem bestimmten Zeitpunkt haben - 2\n" +
                      "Überprüfen Sie, ob der Benutzer an einem bestimmten Datum online war - 3\n" +
                      "Prognose zur Anzahl der Benutzer online - 4\n" +
                      "Prognose für Benutzer online - 5\n" +
                      "Gesamte Online-Zeit für einen Benutzer - 6\n" +
                      "Durchschnittliche Zeit für einen Benutzer - 7\n" +
                      "Gelöschten Benutzer anzeigen - 8", resultDE); 
    }

    [Fact]
    public void FormatUserData_ReturnsCorrectString()
    {
        // Arrange
        var localization = new Localization();
        User user = new User { nickname = "John", lastSeenDate = DateTime.Now };
        string languageEN = "en";
        string languageDE = "de";

        // Act
        string resultEN = localization.FormatUserData(user, languageEN);
        string resultDE = localization.FormatUserData(user, languageDE);

        // Assert
        Assert.Equal($"{user.nickname} just now", resultEN);
        Assert.Equal($"{user.nickname} war vor gerade eben online.", resultDE);
    }

    [Fact]
    public void GetTimeAgoString_ReturnsCorrectString()
    {
        // Arrange
        var localization = new Localization();
        TimeSpan difference = TimeSpan.FromMinutes(30);
        string languageEN = "en";
        string languageDE = "de";

        // Act
        string resultEN = localization.GetTimeAgoString(difference, languageEN);
        string resultDE = localization.GetTimeAgoString(difference, languageDE);

        // Assert
        Assert.Equal("a couple of minutes ago", resultEN);
        Assert.Equal("vor ein paar Minuten", resultDE);

    }
}
