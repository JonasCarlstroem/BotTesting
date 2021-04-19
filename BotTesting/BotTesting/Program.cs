using System;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace BotTesting
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();
        static int temp = 0;

        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Input website (I.E. www.google.se): ");
                string url = Console.ReadLine();
                Console.WriteLine("Initiating link search!");

                if (url != string.Empty)
                {
                    await ReadLinks(url);
                }

                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
                Console.WriteLine("Finished. Reloading...");
                System.Threading.Thread.Sleep(1000);
            }
        }
        public static async Task ReadLinks(string url)
        {
            Match m;
            string pat = @"href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>\S+))";
            temp = 0;
            var ellapsedTime = System.Diagnostics.Stopwatch.StartNew();
            string httpOrHttps = "http://";
            try
            {
                string responseBody = await client.GetStringAsync(httpOrHttps + url + "/");
                m = Regex.Match(responseBody, pat, RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromSeconds(1));
                while(m.Success)
                {
                    Console.WriteLine("Found href: " + m.Groups[1] + " at " + m.Groups[1].Index);
                    m = m.NextMatch();
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException: {0}", e);
            }

            ellapsedTime.Stop();
            double ellapsedTimeInSec = ellapsedTime.ElapsedMilliseconds / 1000d;
            Console.WriteLine("\nTotal amount of links: \t\t{0}", temp);
            Console.WriteLine("Operation took: \t\t{0}s", ellapsedTimeInSec);
        }
    }
}