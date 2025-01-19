using System.Net.Http;
using log4net;

namespace CrypticLauncherBeautify.Generic
{
    public class HttpClientManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HttpClientManager));

        public static HttpClient? HttpClient { get; set; } = null;

        private static readonly SemaphoreSlim HttpClientSemaphore = new SemaphoreSlim(1, 1);

        public static async Task InitHttpClient()
        {
            try
            {
                await HttpClientSemaphore.WaitAsync();

                if (HttpClient == null)
                {
                    try
                    {
                        HttpClient = new HttpClient();
                        Log.Info("HttpClient successfully initialized.");
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Error initializing HttpClient: {ex.Message}\n{ex.StackTrace}");
                    }
                }
                else
                {
                    Log.Info("HttpClient is already initialized.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error initializing HttpClient: {ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                HttpClientSemaphore.Release();
            }
        }
    }
}