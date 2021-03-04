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
    public class MisService : IMisService
    {
        private readonly IRestClient _client;
        private readonly IRestRequest _request;
        private readonly IOptions<MisApiModel> _misApiModel;
        private readonly ILogger<MisService> _logger;

        public MisService(IRestClient client, IOptions<MisApiModel> misApiModel, ILogger<MisService> logger)
        {
            _misApiModel = misApiModel ?? throw new ArgumentNullException(nameof(misApiModel));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _request = new RestRequest();
        }

        public async Task<IEnumerable<DepartmentModel>> GetAllDepartments()
        {
            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Department;
            var departmentServiceUrl = misApiUrl + endPoint + "?divisionId=" + 1;
            var response = await Execute<MisResponse<DepartmentModel>>(departmentServiceUrl);

            if (!response.Status)
                RaiseApplicationException(response);

            return response.Data;
        }

        public async Task<DepartmentModel> GetDepartmentById(int id)
        {
            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Department;
            var departmentServiceUrl = misApiUrl + endPoint + "?divisionId=" + 1 + "&departmentId=" + id;
            var response = await Execute<MisResponse<DepartmentModel>>(departmentServiceUrl);

            if (!response.Status)
                RaiseApplicationException(response);

            return response.Data.FirstOrDefault();
        }

        public async Task<IEnumerable<RoleModel>> GetAllRoles()
        {
            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Role;
            var roleServiceUrl = misApiUrl + endPoint + "?divisionId=" + 1;
            var response = await Execute<MisResponse<RoleModel>>(roleServiceUrl);

            if (!response.Status)
                RaiseApplicationException(response);

            return response.Data;
        }

        public async Task<RoleModel> GetRoleById(int id)
        {
            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Role;
            var roleServiceUrl = misApiUrl + endPoint + "?roleId=" + id;
            var response = await Execute<MisResponse<RoleModel>>(roleServiceUrl);

            if (!response.Status)
                RaiseApplicationException(response);

            return response.Data.FirstOrDefault();
        }

        public async Task<IEnumerable<RoleModel>> GetRoleByDepartmentId(int departmentId)
        {
            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Role;
            var roleServiceUrl = misApiUrl + endPoint + "?divisionId=" + 1 + "&departmentId=" + departmentId;
            var response = await Execute<MisResponse<RoleModel>>(roleServiceUrl);

            if (!response.Status)
                RaiseApplicationException(response);

            return response.Data;
        }

        public async Task<IEnumerable<PositionModel>> GetPositionsByRoleId(int roleId)
        {
            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Position;
            var positionServiceUrl = misApiUrl + endPoint + "?roleId=" + roleId;
            var response = await Execute<MisResponse<PositionModel>>(positionServiceUrl);

            if (!response.Status)
                RaiseApplicationException(response);

            return response.Data;
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

        public async Task<IEnumerable<PositionModel>> GetPositions()
        {
            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Position;
            var positionServiceUrl = misApiUrl + endPoint;
            var response = await Execute<MisResponse<PositionModel>>(positionServiceUrl);

            if (!response.Status)
                RaiseApplicationException(response);

            return response.Data;
        }
    }
}
