﻿namespace LastSeenApplication;

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

        string language = "en";

        switch (languageChoice)
        {
            case 1:
                language = "en";
                break;
            case 2:
                language = "uk";
                break;
            case 3:
                language = "de";
                break;
            case 4:
                language = "fr";
                break;

            default:
                Console.WriteLine("There is no such language, starting in English");
                break;
        }

        return language;
    }
    public void Output(string language)
    {
        switch (language)
        {
            case "en":
                Console.WriteLine("What you want to do? \nHave a list of all users - 1 \nHave number of users at the exact time -" +
                                  " 2\n Check if the user was online at the exact date - 3\nPrediction about amount of the users online - 4\n" +
                                  "Prediction about user online - 5");
                break;
            case "uk":
                Console.WriteLine("Що ви хочете зробити? \nОтримати список всіх користувачів - 1 \nОтримати кількість користувачів в точний час - 2" +
                                  "\n Перевірити, чи був користувач в мережі в точну дату - 3" +
                                  "\nПрогноз кількості користувачів онлайн - 4\nПрогноз користувачів онлайн - 5");
                break;
            case "de":
                Console.WriteLine("Was möchten Sie tun? \nEine Liste aller Benutzer haben - 1 \nDie Anzahl der Benutzer zu einem bestimmten Zeitpunkt haben - 2" +
                                  "\n Überprüfen Sie, ob der Benutzer an einem bestimmten Datum online war - 3" +
                                  "\nPrognose zur Anzahl der Benutzer online - 4\nPrognose für Benutzer online - 5");
                break;
            case "fr":
                Console.WriteLine("Que souhaitez-vous faire ? \nObtenir la liste de tous les utilisateurs - 1 \nObtenir le nombre d'utilisateurs à un moment précis - 2\n" +
                                  " Vérifier si l'utilisateur était en ligne à une date précise - 3\nPrévision sur le nombre d'utilisateurs en ligne - " +
                                  "4\nPrévision sur les utilisateurs en ligne - 5");
                break;
        }
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
    public static string GetTimeAgoString(TimeSpan difference, string language)
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
}