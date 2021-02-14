using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ASM.Core.Entities;
using ASM.Core.Models;
using ASM.Core.Services;
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

        public MisService(IRestClient client, IOptions<MisApiModel> misApiModel)
        {
            _misApiModel = misApiModel ?? throw new ArgumentNullException(nameof(misApiModel));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _request = new RestRequest();
        }

        public async Task<IEnumerable<Department>> GetAllDepartments()
        {
            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Department;
            var departmentServiceUrl = misApiUrl + endPoint + "?divisionId=" + 1;
            var response = await Execute<DepartmentData>(departmentServiceUrl);
            return response.Data;
        }

        public async Task<Department> GetDepartmentById(int id)
        {
            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Department;
            var departmentServiceUrl = misApiUrl + endPoint + "?divisionId=" + 1 + "&departmentId=" + id;
            var response = await Execute<DepartmentData>(departmentServiceUrl);
            return response.Data.FirstOrDefault();
        }

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Role;
            var roleServiceUrl = misApiUrl + endPoint + "?divisionId=" + 1;
            var response = await Execute<RoleData>(roleServiceUrl);
            return response.Data;
        }

        public async Task<IEnumerable<Role>> GetRoleByDepartmentId(int departmentId)
        {
            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Role;
            var roleServiceUrl = misApiUrl + endPoint + "?divisionId=" + 1 + "&departmentId=" + departmentId;
            var response = await Execute<RoleData>(roleServiceUrl);
            return response.Data;
        }

        public async Task<IEnumerable<Position>> GetPositions(int roleId)
        {
            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Position;
            var positionServiceUrl = misApiUrl + endPoint + "?roleId=" + roleId;
            var response = await Execute<PositionData>(positionServiceUrl);
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
    }
}
