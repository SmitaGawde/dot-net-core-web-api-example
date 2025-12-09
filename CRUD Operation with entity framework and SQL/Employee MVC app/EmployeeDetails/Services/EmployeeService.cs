using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using EmployeeDetails.Models;
using EmployeeAdminPortal.Models.Entities;

namespace EmployeeDetails.Services
{
    public class EmployeeService
    {
        private readonly HttpClient client;

        public EmployeeService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7277/api/Employees/"); // Change to your API URL
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<List<Employee>> getAllEmployeeAsync()
        {
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                string jsondata = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Employee>>(jsondata);
            }
            return null;
        }
        public async Task<Employee> GetEmployeeAsync(Guid id)
        {
            HttpResponseMessage response = await client.GetAsync($"{id}");
            if (response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Employee>(jsonData);
            }

            return null;
        }

        public async Task<Employee> AddEmployeeAsync(AddemployeeDto emp)
        {
            var json = JsonConvert.SerializeObject(emp);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("", content);
            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Employee>(responseJson);
            }

            return null;
        }

        public async Task<Employee> UpdateEmployeeAsync(Guid id, AddemployeeDto emp)
        {
            var json = JsonConvert.SerializeObject(emp);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{id}", content);
            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Employee>(responseJson);
            }

            return null;
        }

        public async Task<bool> DeleteEmployeeAsync(Guid id)
        {
            var response = await client.DeleteAsync($"{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
