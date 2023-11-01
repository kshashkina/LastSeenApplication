namespace LastSeenApplication;

public class Localization
{
    public string ChooseLanguage()
    {
        Console.WriteLine("Choose the language:");
        Console.WriteLine("1. English");
        Console.WriteLine("2. Українська");
        Console.WriteLine("3. Deutsch");
        Console.WriteLine("4. Français");
        int languageChoice = Convert.ToInt32(Console.ReadLine());
        var language = LanguageKey(languageChoice);
        return language;
    }


    public string LanguageKey(int key)
    {
        string language = "en";

        switch (key)
        {
            case 1:
                language = "en";
                return language;
            case 2:
                language = "uk";
                return language;
            case 3:
                language = "de";
                return language;
            case 4:
                language = "fr";
                return language;

            default:
                Console.WriteLine("There is no such language, starting in English");
                return language;
        }

        return language;
    }
    public string Output(string language)
    {
        switch (language)
        {
            case "en":
                var textEN =
                    "What you want to do?\n" +
                    "Have a list of all users - 1\n" +
                    "Have the number of users at the exact time - 2\n" +
                    "Check if the user was online at the exact date - 3\n" +
                    "Prediction about the number of users online - 4\n" +
                    "Prediction about a user being online - 5\n" +
                    "Total amount of time online for a user - 6\n" +
                    "Average time for a user - 7\n" +
                    "Display deleted user - 8\n" +
                    "Post report - 9\n" + 
                    "Get report - 10";
                Console.WriteLine(textEN);
                return textEN;

            case "uk":
                var textUK =
                    "Що ви хочете зробити?\n" +
                    "Отримати список всіх користувачів - 1\n" +
                    "Отримати кількість користувачів в певний час - 2\n" +
                    "Перевірити, чи користувач був онлайн на певну дату - 3\n" +
                    "Прогноз щодо кількості користувачів онлайн - 4\n" +
                    "Прогноз щодо того, чи користувач буде онлайн - 5\n" +
                    "Загальна кількість часу онлайн для користувача - 6\n" +
                    "Середній час для користувача - 7\n" +
                    "Показати видалених користувачів - 8\n" +
                    "Надіслати звіт - 9\n" + 
                    "Отримати звіт - 10";
                Console.WriteLine(textUK);
                return textUK;

            case "de":
                var textDE =
                    "Was möchten Sie tun?\n" +
                    "Alle Benutzer auflisten - 1\n" +
                    "Anzahl der Benutzer zu einem bestimmten Zeitpunkt - 2\n" +
                    "Überprüfen, ob der Benutzer an einem bestimmten Datum online war - 3\n" +
                    "Prognose zur Anzahl der Benutzer online - 4\n" +
                    "Prognose, ob ein Benutzer online sein wird - 5\n" +
                    "Gesamte Online-Zeit für einen Benutzer - 6\n" +
                    "Durchschnittliche Zeit für einen Benutzer - 7\n" +
                    "Gelöschte Benutzer anzeigen - 8\n" +
                    "Bericht veröffentlichen - 9\n" + 
                    "Bericht abrufen - 10";
                Console.WriteLine(textDE);
                return textDE;

            case "fr":
                var textFR =
                    "Que voulez-vous faire ?\n" +
                    "Obtenir la liste de tous les utilisateurs - 1\n" +
                    "Obtenir le nombre d'utilisateurs à un moment précis - 2\n" +
                    "Vérifier si l'utilisateur était en ligne à une date précise - 3\n" +
                    "Prévision du nombre d'utilisateurs en ligne - 4\n" +
                    "Prévision de la présence d'un utilisateur en ligne - 5\n" +
                    "Temps total en ligne pour un utilisateur - 6\n" +
                    "Temps moyen pour un utilisateur - 7\n" +
                    "Afficher les utilisateurs supprimés - 8\n" +
                    "Poster un rapport - 9\n" + 
                    "Obtenir un rapport - 10";
                Console.WriteLine(textFR);
                return textFR; 
        }
        return "";
    }
    public string FormatUserData(User user, string language)
    {
        string nickName = user.nickname;

        if (user.lastSeenDate == null)
        {
            switch (language)
            {
                case "en":
                    return $"{nickName} is online.";
                case "uk":
                    return $"{nickName} у мережі.";
                case "de":
                    return $"{nickName} ist online.";
                case "fr":
                    return $"{nickName} est en ligne.";
                default:
                    return $"{nickName} is online.";
            }
        }
        else
        {
            DateTime now = DateTime.Now;
            DateTime givenDate = user.lastSeenDate.Value;
            TimeSpan difference = now - givenDate;
            string timeAgo = GetTimeAgoString(difference, language);

            switch (language)
            {
                case "en":
                    return $"{nickName} {timeAgo}";
                case "uk":
                    return $"{nickName} був(ла) у мережі {timeAgo}";
                case "de":
                    return $"{nickName} war vor {timeAgo} online.";
                case "fr":
                    return $"{nickName} était en ligne il y a {timeAgo}.";
                default:
                    return $"{nickName} {timeAgo}";
            }
        }
    }
    public string GetTimeAgoString(TimeSpan difference, string language)
        {
            switch (language)
            {
                case "en":
                    if (difference.TotalSeconds <= 30)
                    {
                        return "just now";
                    }
                    else if (difference.TotalSeconds <= 60)
                    {
                        return "less than a minute ago";
                    }
                    else if (difference.TotalMinutes <= 59)
                    {
                        return "a couple of minutes ago";
                    }
                    else if (difference.TotalMinutes <= 119)
                    {
                        return "an hour ago";
                    }
                    else if (difference.TotalMinutes <= 23 * 60)
                    {
                        return "today";
                    }
                    else if (difference.TotalMinutes <= 47 * 60)
                    {
                        return "yesterday";
                    }
                    else if (difference.TotalDays < 7)
                    {
                        return "this week";
                    }
                    else
                    {
                        return "a long time ago";
                    }
                case "uk":
                    if (difference.TotalSeconds <= 30)
                    {
                        return "лише що";
                    }
                    else if (difference.TotalSeconds <= 60)
                    {
                        return "менше хвилини тому";
                    }
                    else if (difference.TotalMinutes <= 59)
                    {
                        return "кілька хвилин тому";
                    }
                    else if (difference.TotalMinutes <= 119)
                    {
                        return "годину тому";
                    }
                    else if (difference.TotalMinutes <= 23 * 60)
                    {
                        return "сьогодні";
                    }
                    else if (difference.TotalMinutes <= 47 * 60)
                    {
                        return "вчора";
                    }
                    else if (difference.TotalDays < 7)
                    {
                        return "цього тижня";
                    }
                    else
                    {
                        return "давно";
                    }
                case "de":
                    if (difference.TotalSeconds <= 30)
                    {
                        return "gerade eben";
                    }
                    else if (difference.TotalSeconds <= 60)
                    {
                        return "vor weniger als einer Minute";
                    }
                    else if (difference.TotalMinutes <= 59)
                    {
                        return "vor ein paar Minuten";
                    }
                    else if (difference.TotalMinutes <= 119)
                    {
                        return "vor einer Stunde";
                    }
                    else if (difference.TotalMinutes <= 23 * 60)
                    {
                        return "heute";
                    }
                    else if (difference.TotalMinutes <= 47 * 60)
                    {
                        return "gestern";
                    }
                    else if (difference.TotalDays < 7)
                    {
                        return "diese Woche";
                    }
                    else
                    {
                        return "vor langer Zeit";
                    }
                case "fr":
                    if (difference.TotalSeconds <= 30)
                    {
                        return "à l'instant";
                    }
                    else if (difference.TotalSeconds <= 60)
                    {
                        return "il y a moins d'une minute";
                    }
                    else if (difference.TotalMinutes <= 59)
                    {
                        return "il y a quelques minutes";
                    }
                    else if (difference.TotalMinutes <= 119)
                    {
                        return "il y a une heure";
                    }
                    else if (difference.TotalMinutes <= 23 * 60)
                    {
                        return "aujourd'hui";
                    }
                    else if (difference.TotalMinutes <= 47 * 60)
                    {
                        return "hier";
                    }
                    else if (difference.TotalDays < 7)
                    {
                        return "cette semaine";
                    }
                    else
                    {
                        return "il y a longtemps";
                    }
                default:
                    return "long time ago";
            }
        }

    public string FirstFeatureTranslation(string language)
    {
        switch (language)
        {
            case "en":
                Console.WriteLine("Write your date:");
                var dateEN = Console.ReadLine();
                return dateEN;
            case "uk":
                Console.WriteLine("Напишіть потрібну дату:");
                var dateUK = Console.ReadLine();
                return dateUK;
            case "de":
                Console.WriteLine("Schreiben Sie Ihr Datum:");
                var dateDE = Console.ReadLine();
                return dateDE;
            case "fr":
                Console.WriteLine("Écrivez votre date :");
                var dateFR = Console.ReadLine();
                return dateFR;


        }
        return "";
    } 
    public (string date, string userId) SecondFeatureTranslation(string language)
    {
        string date = "";
        string userId = "";

        switch (language)
        {
            case "en":
                Console.WriteLine("Write your date:");
                date = Console.ReadLine();
                Console.WriteLine("Enter the user ID:");
                userId = Console.ReadLine();
                break;

            case "uk":
                Console.WriteLine("Напишіть потрібну дату:");
                date = Console.ReadLine();
                Console.WriteLine("Введіть ідентифікатор користувача:");
                userId = Console.ReadLine();
                break;

            case "de":
                Console.WriteLine("Schreiben Sie Ihr Datum:");
                date = Console.ReadLine();
                Console.WriteLine("Geben Sie die Benutzer-ID ein:");
                userId = Console.ReadLine();
                break;

            case "fr":
                Console.WriteLine("Écrivez votre date :");
                date = Console.ReadLine();
                Console.WriteLine("Entrez l'identifiant de l'utilisateur :");
                userId = Console.ReadLine();
                break;
        }
    
        return (date, userId);
    }

    public (string report, string users, string metrics) FifthFeatureTranslationPost(string language)
    {
        string report = "";
        string users = "";
        string metrics = "";
        switch (language)
        {
            case "en":
                Console.WriteLine("Write report name:");
                report = Console.ReadLine();
                Console.WriteLine("Enter the user ID:");
                users = Console.ReadLine();
                Console.WriteLine("Enter the metrics:");
                metrics = Console.ReadLine();
                break;

            case "uk":
                Console.WriteLine("Введіть назву звіту:");
                report = Console.ReadLine();
                Console.WriteLine("Введіть ідентифікатор користувача:");
                users = Console.ReadLine();
                Console.WriteLine("Введіть метрику:");
                metrics = Console.ReadLine();
                break;


            case "de":
                Console.WriteLine("Geben Sie den Berichtsnamen ein:");
                report = Console.ReadLine();
                Console.WriteLine("Geben Sie die Benutzer-ID ein:");
                users = Console.ReadLine();
                Console.WriteLine("Geben Sie die Metriken ein:");
                metrics = Console.ReadLine();
                break;


            case "fr":
                Console.WriteLine("Entrez le nom du rapport :");
                report = Console.ReadLine();
                Console.WriteLine("Entrez l'identifiant de l'utilisateur :");
                users = Console.ReadLine();
                Console.WriteLine("Entrez les métriques :");
                metrics = Console.ReadLine();
                break;
        }
        return (report, users, metrics);

    }
    
    public (string report, string from, string to) FifthFeatureTranslationGet(string language)
    {
        string report = "";
        string from = "";
        string to = "";
        switch (language)
        {
            case "en":
                Console.WriteLine("Write report name:");
                report = Console.ReadLine();
                Console.WriteLine("Enter the start date:");
                from = Console.ReadLine();
                Console.WriteLine("Enter the finish date:");
                to = Console.ReadLine();
                break;

            case "uk":
                Console.WriteLine("Введіть назву звіту:");
                report = Console.ReadLine();
                Console.WriteLine("Введіть початкову дату:");
                from = Console.ReadLine();
                Console.WriteLine("Введіть кінцеву дату:");
                to = Console.ReadLine();
                break;



            case "de":
                Console.WriteLine("Geben Sie den Berichtsnamen ein:");
                report = Console.ReadLine();
                Console.WriteLine("Geben Sie das Startdatum ein:");
                from = Console.ReadLine();
                Console.WriteLine("Geben Sie das Enddatum ein:");
                to = Console.ReadLine();
                break;



            case "fr":
                Console.WriteLine("Entrez le nom du rapport :");
                report = Console.ReadLine();
                Console.WriteLine("Entrez la date de début :");
                from = Console.ReadLine();
                Console.WriteLine("Entrez la date de fin :");
                to = Console.ReadLine();
                break;

        }
        return (report, from, to);

    }
    
    public (string date, string userId, string tolerance) ForthFeatureTranslation(string language)
    {
        string date = "";
        string userId = "";
        string tolerance = "";

        switch (language)
        {
            case "en":
                Console.WriteLine("Write your date:");
                date = Console.ReadLine();
                Console.WriteLine("Enter the user ID:");
                userId = Console.ReadLine();
                Console.WriteLine("Enter the tolerance:");
                tolerance = Console.ReadLine();
                break;

            case "uk":
                Console.WriteLine("Напишіть потрібну дату:");
                date = Console.ReadLine();
                Console.WriteLine("Введіть ідентифікатор користувача:");
                userId = Console.ReadLine();
                Console.WriteLine("Введіть допуск:");
                tolerance = Console.ReadLine();
                break;

            case "de":
                Console.WriteLine("Schreiben Sie Ihr Datum:");
                date = Console.ReadLine();
                Console.WriteLine("Geben Sie die Benutzer-ID ein:");
                userId = Console.ReadLine();
                Console.WriteLine("Geben Sie die Toleranz ein:");
                tolerance = Console.ReadLine();
                break;

            case "fr":
                Console.WriteLine("Écrivez votre date :");
                date = Console.ReadLine();
                Console.WriteLine("Entrez l'identifiant de l'utilisateur :");
                userId = Console.ReadLine();
                Console.WriteLine("Entrez la tolérance:");
                tolerance = Console.ReadLine();
                break;
        }
    
        return (date, userId, tolerance);
    }

    public string Assignment4Translation(string language)
    {
        string userId = "";
        switch (language)
        {
            case "en":
                Console.WriteLine("Enter the user ID:");
                userId = Console.ReadLine();
                break;

            case "uk":
                Console.WriteLine("Введіть ідентифікатор користувача:");
                userId = Console.ReadLine();
                break;

            case "de":
                Console.WriteLine("Geben Sie die Benutzer-ID ein:");
                userId = Console.ReadLine();
                break;

            case "fr":
                Console.WriteLine("Entrez l'identifiant de l'utilisateur :");
                userId = Console.ReadLine();
                break;
        }

        return userId;

    }


}

