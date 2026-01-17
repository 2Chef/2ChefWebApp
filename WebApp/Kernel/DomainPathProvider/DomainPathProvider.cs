using Core.Kernel.DiReg;
using System.Text.Json;

namespace WebApp.Kernel.DomainPathProvider
{
    [DiReg(ServiceLifetime.Singleton, typeof(IDomainPathProvider))]
    public class DomainPathProvider : IDomainPathProvider
    {
        private IWebHostEnvironment Enviroment { get; }

        public DomainPathProvider(IWebHostEnvironment enviroment)
        {
            Enviroment = enviroment;
        }

        public async Task<string> GetDomain()
        {
            if (Enviroment.IsDevelopment())
            {
                return await GetDevelopmentDomain();
            }
            else
            {
                throw new NotImplementedException("Пока нельзя получить домен для продкашена");
            }
        }

        private async Task<string> GetDevelopmentDomain()
        {
            const string ngrokTunnelUrl = "http://127.0.0.1:4040/api/tunnels";

            using (HttpClient client = new HttpClient())
            {
                string response;
                try
                {
                    response = await client.GetStringAsync(ngrokTunnelUrl);
                }
                catch
                {
                    throw new InvalidOperationException("Не настроен туннель ngrok!");
                }

                using JsonDocument json = JsonDocument.Parse(response);

                string? publicUrl = json.RootElement
                    .GetProperty("tunnels")
                    .EnumerateArray()
                    .FirstOrDefault()
                    .GetProperty("public_url")
                    .GetString();

                if (string.IsNullOrEmpty(publicUrl))
                    throw new InvalidOperationException("ngrok вернул недействительный ответ");

                return publicUrl;
            }
        }
    }
}
