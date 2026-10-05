using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using ToDoApp.Common;
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

        public async Task<List<TaskItem>> GetTasksAsync()
        {
            AttachToken();

            var tasks = await _httpClient.GetFromJsonAsync<List<TaskItem>>("Tasks");

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

        public async Task<ApiResult<TaskItem>> PutTaskAsync(int id, TaskItem taskItem)
        {
            AttachToken();

            var response = await _httpClient.PutAsJsonAsync($"Tasks/{id}", taskItem);

            if (response.IsSuccessStatusCode)
            {
                var updatedTask = await response.Content.ReadFromJsonAsync<TaskItem>();
                return ApiResult<TaskItem>.Ok(updatedTask!);
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            return ApiResult<TaskItem>.Fail(errorMessage);
        }

        public async Task<ApiResult> DeleteTaskAsync(int id, uint rowVersion)
        {
            AttachToken();

            var response = await _httpClient.DeleteAsync($"Tasks/{id}?rowVersion={rowVersion}");

            if (response.IsSuccessStatusCode)
            {
                return ApiResult.Ok();
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            return ApiResult.Fail(errorMessage);
        }
    }
}
