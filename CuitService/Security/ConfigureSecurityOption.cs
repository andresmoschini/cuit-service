using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace CuitService.Security
{
    public class ConfigureSecurityOption : IConfigureOptions<SecurityOptions>
    {
        private readonly IConfiguration _configuration;
        private readonly IFileProvider _fileProvider;

        public ConfigureSecurityOption(IConfiguration configuration, IFileProvider fileProvider)
        {
            _configuration = configuration;
            _fileProvider = fileProvider;
        }

        private static string ReadToEnd(IFileInfo fileInfo)
        {
            using var stream = fileInfo.CreateReadStream();
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        private static RsaSecurityKey ParseXmlString(string xmlString)
        {
            using var rsaProvider = new RSACryptoServiceProvider();
            rsaProvider.FromXmlString(xmlString);
            var rsaParameters = rsaProvider.ExportParameters(false);
            return new RsaSecurityKey(RSA.Create(rsaParameters));
        }

        public void Configure(SecurityOptions options)
        {
            var path = _configuration.GetValue("public-keys", "public-keys");
            var files = _fileProvider.GetDirectoryContents(path).Where(x => !x.IsDirectory);
            var publicKeys = files
                .Select(ReadToEnd)
                .Select(ParseXmlString)
                .ToArray();

            options.SkipLifetimeValidation = false;
            options.SigningKeys = publicKeys;
        }
    }
}
