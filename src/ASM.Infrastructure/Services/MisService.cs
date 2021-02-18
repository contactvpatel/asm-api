using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ASM.Core.Models;
using ASM.Core.Services;
using ASM.Util.Models;
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
        private readonly IRedisCacheService _redisCacheService;

        public MisService(IRestClient client, IOptions<MisApiModel> misApiModel, IRedisCacheService redisCacheService)
        {
            _misApiModel = misApiModel ?? throw new ArgumentNullException(nameof(misApiModel));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _redisCacheService = redisCacheService ?? throw new ArgumentNullException(nameof(redisCacheService));
            _request = new RestRequest();
        }

        public async Task<IEnumerable<DepartmentModel>> GetAllDepartments()
        {
            const string cacheKey = "departments";

            var cachedResponse = await _redisCacheService.GetCachedData<IEnumerable<DepartmentModel>>(cacheKey);
            if (cachedResponse != null)
                return cachedResponse;

            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Department;
            var departmentServiceUrl = misApiUrl + endPoint + "?divisionId=" + 1;
            var response = await Execute<DepartmentData>(departmentServiceUrl);

            await _redisCacheService.SetCacheData(cacheKey, response.Data, TimeSpan.FromSeconds(86400));

            return response.Data;
        }

        public async Task<DepartmentModel> GetDepartmentById(int id)
        {
            const string cacheKey = "departments";

            var cachedResponse = await _redisCacheService.GetCachedData<IEnumerable<DepartmentModel>>(cacheKey);
            if (cachedResponse != null)
                return cachedResponse.FirstOrDefault(x => x.DepartmentId == id);

            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Department;
            var departmentServiceUrl = misApiUrl + endPoint + "?divisionId=" + 1 + "&departmentId=" + id;
            var response = await Execute<DepartmentData>(departmentServiceUrl);

            return response.Data.FirstOrDefault();
        }

        public async Task<IEnumerable<RoleModel>> GetAllRoles()
        {
            const string cacheKey = "roles";

            var cachedResponse = await _redisCacheService.GetCachedData<IEnumerable<RoleModel>>(cacheKey);
            if (cachedResponse != null)
                return cachedResponse;

            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Role;
            var roleServiceUrl = misApiUrl + endPoint + "?divisionId=" + 1;
            var response = await Execute<RoleData>(roleServiceUrl);

            await _redisCacheService.SetCacheData(cacheKey, response.Data, TimeSpan.FromSeconds(86400));

            return response.Data;
        }

        public async Task<RoleModel> GetRoleById(int id)
        {
            const string cacheKey = "roles";

            var cachedResponse = await _redisCacheService.GetCachedData<IEnumerable<RoleModel>>(cacheKey);
            if (cachedResponse != null)
                return cachedResponse.FirstOrDefault(x => x.RoleId == id);

            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Role;
            var roleServiceUrl = misApiUrl + endPoint + "?roleId=" + id;
            var response = await Execute<RoleData>(roleServiceUrl);

            return response.Data.FirstOrDefault();
        }

        public async Task<IEnumerable<RoleModel>> GetRoleByDepartmentId(int departmentId)
        {
            const string cacheKey = "roles";

            var cachedResponse = await _redisCacheService.GetCachedData<IEnumerable<RoleModel>>(cacheKey);
            if (cachedResponse != null)
                return cachedResponse.Where(x => x.DepartmentId == departmentId);

            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Role;
            var roleServiceUrl = misApiUrl + endPoint + "?divisionId=" + 1 + "&departmentId=" + departmentId;
            var response = await Execute<RoleData>(roleServiceUrl);

            return response.Data;
        }

        public async Task<IEnumerable<PositionModel>> GetPositions(int roleId)
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
