using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using ToDoApp.Models;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.Services.Implementations
{
    public class TaskApiService : ITaskApiService
    {
        private readonly HttpClient _httpClient;
        private readonly Supabase.Client _supabaseClient;

        public TaskApiService(HttpClient httpClient, Supabase.Client supabaseClient)
        {
            _httpClient = httpClient;
            _supabaseClient = supabaseClient;
        }

        private void AttachToken()
        {
            var token = _supabaseClient.Auth.CurrentSession?.AccessToken;

            if (token is not null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<TaskItem>> GetTasksAsync(Guid id)
        {
            AttachToken();

            var tasks = await _httpClient.GetFromJsonAsync<List<TaskItem>>($"Tasks?userId={id}");

            return tasks ?? new List<TaskItem>();
        }

        public async Task<TaskItem?> PostTaskAsync(TaskItem taskItem)
        {
            AttachToken();

            var response = await _httpClient.PostAsJsonAsync("Tasks", taskItem);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var createdTask = await response.Content.ReadFromJsonAsync<TaskItem>();

            return createdTask;
        }

        public async Task PutTaskAsync(int id, TaskItem taskItem)
        {
            AttachToken();

            await _httpClient.PutAsJsonAsync($"Tasks/{id}", taskItem);
        }

        public async Task DeleteTaskAsync(int id)
        {
            AttachToken();

            await _httpClient.DeleteAsync($"Tasks/{id}");
        }
    }
}
