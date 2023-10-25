namespace LastSeenApplication;

public class Loader
{
    public async Task<string> Load(string apiUrl)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(new Uri(apiUrl)).Result;

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    return await response.Content.ReadAsStringAsync();


                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        return null;
    }

    public async Task<string> Post(string apiUrl, string id)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(id);

                HttpResponseMessage response = await client.PostAsync(new Uri(apiUrl), content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                    return responseBody;

                }
                else
                {
                    Console.WriteLine($"Mistake: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Mistake: {ex.Message}");
        }

        return null;
        }
    }
