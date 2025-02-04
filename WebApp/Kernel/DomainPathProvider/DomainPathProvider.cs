using Core.Kernel.DiReg;
using Newtonsoft.Json.Linq;

namespace WebApp.Kernel.DomainPathProvider
{
    [DiReg(ServiceLifetime.Singleton, typeof(IDomainPathProvider))]
    public class DomainPathProvider : IDomainPathProvider
    {
        private const string ngrokTunnelUrl = "http://127.0.0.1:4040/api/tunnels";

        private IWebHostEnvironment Enviroment { get; }

        public DomainPathProvider(IWebHostEnvironment enviroment)
        {
            Enviroment = enviroment;
        }

        public async Task<string> GetDomain()
        {
            if (Enviroment.IsDevelopment())
            {
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

                    JObject json = JObject.Parse(response);

                    string? publicUrl = json["tunnels"]?[0]?["public_url"]?.ToString();

                    if (string.IsNullOrEmpty(publicUrl))
                        throw new InvalidOperationException("ngrok вернул недействительный ответ");

                    return publicUrl;
                }
            }
            else
            {
                throw new NotImplementedException("Пока нельзя получить домен для продкашена");
            }
        }
    }
}
