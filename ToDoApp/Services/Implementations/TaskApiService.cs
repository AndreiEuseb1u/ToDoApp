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

        public TaskApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TaskItem>> GetTasksAsync(Guid id)
        {
            var tasks = await _httpClient.GetFromJsonAsync<List<TaskItem>>($"Tasks?userId={id}");

            return tasks ?? new List<TaskItem>();
        }

        public async Task<TaskItem?> PostTaskAsync(TaskItem taskItem)
        {
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
            await _httpClient.PutAsJsonAsync($"Tasks/{id}", taskItem);
        }

        public async Task DeleteTaskAsync(int id)
        {
            await _httpClient.DeleteAsync($"Tasks/{id}");
        }
    }
}
