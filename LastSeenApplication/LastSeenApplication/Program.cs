namespace LastSeenApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var offset = 0;
            while (true)
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client
                        .GetAsync(new Uri($"https://sef.podkolzin.consulting/api/users/lastSeen?offset={offset}"))
                        .Result;

                    if (response.IsSuccessStatusCode)
                    {
                        // Code for handling the successful HTTP response goes here.
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                    }
                }
   
            }
        }
    }
}