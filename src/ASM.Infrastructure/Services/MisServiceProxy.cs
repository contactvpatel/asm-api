using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Core.Entities;
using ASM.Core.Models;
using ASM.Core.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace ASM.Infrastructure.Services
{
    public class MisServiceProxy : IMisServiceProxy
    {
        private readonly IRestClient _client;
        private readonly IRestRequest _request;
        private readonly IOptions<MisApiModel> _misApiModel;

        public MisServiceProxy(IRestClient client, IOptions<MisApiModel> misApiModel)
        {
            _misApiModel = misApiModel ?? throw new ArgumentNullException(nameof(misApiModel));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _request = new RestRequest();
        }

        public async Task<IEnumerable<Department>> GetDepartment()
        {
            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Department;
            var departmentServiceUrl = misApiUrl + endPoint + "?divisionId=" + 1;
            var response = await Execute<DepartmentData>(departmentServiceUrl);
            var departmentModel = new List<Department>();
            if (response != null)
            {
                foreach (var department in response.Data)
                {
                    departmentModel.Add(new Department
                        {DepartmentId = department.DepartmentId, DepartmentName = department.DepartmentName});
                }
            }

            return departmentModel;
        }

        public async Task<Department> GetDepartmentById(int id)
        {
            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Department;
            var departmentServiceUrl = misApiUrl + endPoint + "?divisionId=" + 1 + "&departmentId=" + id;
            var response = await Execute<DepartmentData>(departmentServiceUrl);
            var departmentModel = new Department();
            if (response != null)
            {
                foreach (var department in response.Data)
                {
                    departmentModel.DepartmentId = department.DepartmentId;
                    departmentModel.DepartmentName = department.DepartmentName;
                }
            }

            return departmentModel;
        }

        public async Task<IEnumerable<Role>> GetRoleByDepartmentId(int departmentId)
        {
            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Role;
            var departmentServiceUrl = misApiUrl + endPoint + "?divisionId=" + 1 + "&departmentId=" + departmentId;
            var response = await Execute<RoleData>(departmentServiceUrl);
            var roleModel = new List<Role>();
            if (response.Data != null)
            {
                foreach (var role in response.Data)
                {
                    roleModel.Add(new Role
                        {RoleId = role.RoleId, RoleName = role.RoleName});
                }
            }

            return roleModel;
        }

        public async Task<IEnumerable<Position>> GetPosition(int departmentId, int roleId)
        {
            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.Position;
            var departmentServiceUrl = misApiUrl + endPoint + "?departmentId=" + departmentId + "&roleId=" + roleId;
            var response = await Execute<PositionData>(departmentServiceUrl);
            var positionModel = new List<Position>();
            if (response.Data != null)
            {
                foreach (var position in response.Data)
                {
                    positionModel.Add(new Position
                        {RolePositionId = position.RolePositionId, RolePositionName = position.RolePositionName});
                }
            }

            return positionModel;
        }

        public async Task<IEnumerable<Position>> GetPerson(int personId)
        {
            var misApiUrl = _misApiModel.Value.Url;
            var endPoint = _misApiModel.Value.Endpoint.PersonPosition;
            var personServiceUrl = misApiUrl + endPoint + "?PersonId=" + personId;
            var response = await Execute<PersonPositionData>(personServiceUrl);
            var personPositionModel = new List<Position>();
            if (response.Data != null)
            {
                foreach (var position in response.Data)
                {
                    personPositionModel.Add(new Position
                        {RolePositionId = position.RolePositionId, RolePositionName = position.RolePositionName});
                }
            }

            return personPositionModel;
        }
        
        private async Task<T> Execute<T>(string url)
        {
            _request.Parameters.Clear();
            _request.Resource = url;
            _request.Method = Method.GET;
            _request.AddHeader("Content-type", "application/json");
            var response = await _client.ExecuteAsync(_request);
            return JsonConvert.DeserializeObject<T>(response.Content);
        }
    }
}
