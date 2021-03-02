using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ASM.Core.Models;
using ASM.Core.Services;
using ASM.Util.Logging;
using ASM.Util.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace ASM.Infrastructure.Services
{
    public class SsoService : ISsoService
    {
        private readonly IRestClient _client;
        private readonly IRestRequest _request;
        private readonly IOptions<SsoApiModel> _ssoApiModel;
        private readonly ILogger<SsoService> _logger;

        public SsoService(IRestClient client, IOptions<SsoApiModel> ssoApiModel, ILogger<SsoService> logger)
        {
            _ssoApiModel = ssoApiModel ?? throw new ArgumentNullException(nameof(ssoApiModel));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _request = new RestRequest();
        }

        public async Task<IEnumerable<ApplicationModel>> GetAllApplications()
        {
            return GetTempApplications();

            /*
            var ssoApiUrl = _ssoApiModel.Value.Url;
            var endPoint = _ssoApiModel.Value.Endpoint.Application;
            var applicationServiceUrl = ssoApiUrl + endPoint;
            var response = await Execute<ApplicationData>(applicationServiceUrl);

            if (!response.Status)
                RaiseApplicationException(response);
            
            return response.Data;
            */
        }

        public async Task<ApplicationModel> GetApplicationById(Guid id)
        {
            return GetTempApplications().FirstOrDefault(x => x.ApplicationId == id);

            /*
            var ssoApiUrl = _ssoApiModel.Value.Url;
            var endPoint = _ssoApiModel.Value.Endpoint.Application;
            var applicationServiceUrl = ssoApiUrl + endPoint + "/" + id;
            var response = await Execute<ApplicationData>(applicationServiceUrl);

            if (!response.Status)
                RaiseApplicationException(response);

            return response.Data.FirstOrDefault();
            */
        }

        private async Task<T> Execute<T>(string url)
        {
            _request.Parameters.Clear();
            _request.Resource = url;
            _request.Method = Method.GET;
            _request.AddHeader("Content-type", "application/json");
            var response = await _client.ExecuteAsync(_request);

            if (response.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<T>(response.Content);

            throw new ApplicationException(response.Content);
        }

        private void RaiseApplicationException<T>(MisResponse<T> response)
        {
            var errorMessage = response.Messages?.FirstOrDefault()?.FieldName + "-" +
                               response.Messages?.FirstOrDefault()?.ErrorCode + "-" +
                               response.Messages?.FirstOrDefault()?.ErrorMessage;
            _logger.LogErrorExtension(errorMessage, null);
            throw new ApplicationException(errorMessage);
        }

        private static List<ApplicationModel> GetTempApplications()
        {
            return new()
            {
                new ApplicationModel
                    {ApplicationId = Guid.Parse("79835b30-930c-4400-8721-491201589c59"), ApplicationName = "EMS"},
                new ApplicationModel
                    {ApplicationId = Guid.Parse("acda0825-4ee0-46c4-af20-d34430406655"), ApplicationName = "FMS"},
                new ApplicationModel
                    {ApplicationId = Guid.Parse("595da0e9-7c3d-499f-816b-eed603daa4b8"), ApplicationName = "GSS"},
                new ApplicationModel
                    {ApplicationId = Guid.Parse("6083ee5c-60f1-43a9-8409-2858bd73bb3e"), ApplicationName = "PBR"},
            };
        }
    }
}
