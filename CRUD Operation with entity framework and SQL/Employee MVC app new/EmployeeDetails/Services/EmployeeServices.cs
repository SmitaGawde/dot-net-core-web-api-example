using EmployeeDetails.Model;
using System.Text.Json.Serialization;
using Newtonsoft.Json;


namespace EmployeeDetails.Services
{
    public class EmployeeServices
    {
        private readonly HttpClient client;

        public EmployeeServices()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7277/api/Employee/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
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

        public async Task<List<Employee>> getEmployeeAsync(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"{id}");
            if (response.IsSuccessStatusCode)
            {
                string jsondata = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Employee>>(jsondata);
            }
            return null;
        }

        public async Task<List<Employee>> UpdateEmployeeAsync(int id,EmployeeDto employeeDto)
        {
            var json = JsonConvert.SerializeObject(employeeDto);
            var content = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            HttpResponseMessage response = await client.PutAsync($"{id}", content);
            if (response.IsSuccessStatusCode)
            {
                string jsondata = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Employee>>(jsondata);
            }
            return null;
        }

        public async Task<List<Employee>> AddEmployeeAsync( EmployeeDto employeeDto)
        {
            var json = JsonConvert.SerializeObject(employeeDto);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("", content);
            if (response.IsSuccessStatusCode)
            {
                string jsondata = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Employee>>(jsondata);
            }
            return null;
        }


        public async Task<bool> DeleteEmployeeAsync(int id)
        {

            HttpResponseMessage response = await client.DeleteAsync($"{id}");
            return (response.IsSuccessStatusCode); 
   
        }





        // end of class


    }
}
