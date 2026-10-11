using System.Net.Http.Json;
using ToDoApp.Common;
using ToDoApp.Extensions;
using ToDoApp.Models;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.Services.Implementations
{
    public class TaskApiService : ITaskApiService
    {
        private readonly HttpClient _httpClient;
        private readonly Supabase.Client _supabaseClient;
        private const string NoResponseMessage = "Could not reach the server. Check your connection and try again.";

        public TaskApiService(HttpClient httpClient, Supabase.Client supabaseClient)
        {
            _httpClient = httpClient;
            _supabaseClient = supabaseClient;
        }

        private static bool IsNetworkFailure(Exception ex)
        {
            return ex is HttpRequestException or TaskCanceledException;
        }

        private static async Task<ApiResult> SendAsync(Func<Task<ApiResult>> request)
        {
            try
            {
                return await request();
            }
            catch (Exception ex) when (IsNetworkFailure(ex))
            {
                return ApiResult.Fail(NoResponseMessage, ApiErrorType.NoResponse);
            }
        }

        private static async Task<ApiResult<T>> SendAsync<T>(Func<Task<ApiResult<T>>> request)
        {
            try
            {
                return await request();
            }
            catch (Exception ex) when (IsNetworkFailure(ex))
            {
                return ApiResult<T>.Fail(NoResponseMessage, ApiErrorType.NoResponse);
            }
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

        public async Task<ApiResult<List<TaskItem>>> GetTasksAsync()
        {
            AttachToken();

            return await SendAsync(async () =>
            {
                var response = await _httpClient.GetAsync("Tasks");

                if (response.IsSuccessStatusCode)
                {
                    var tasks = await response.Content.ReadFromJsonAsync<List<TaskItem>>();

                    return ApiResult<List<TaskItem>>.Ok(tasks ?? new List<TaskItem>());
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                var errorType = response.StatusCode.ToApiErrorType();

                return ApiResult<List<TaskItem>>.Fail(errorMessage, errorType);

            });
        }

        public async Task<ApiResult<TaskItem>> PostTaskAsync(TaskItem taskItem)
        {
            AttachToken();

            return await SendAsync(async () =>
            {
                var response = await _httpClient.PostAsJsonAsync("Tasks", taskItem);

                if (response.IsSuccessStatusCode)
                {
                    var createdTask = await response.Content.ReadFromJsonAsync<TaskItem>();

                    return ApiResult<TaskItem>.Ok(createdTask!);
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                var errorType = response.StatusCode.ToApiErrorType();

                return ApiResult<TaskItem>.Fail(errorMessage, errorType);
            });
        }

        public async Task<ApiResult<TaskItem>> PutTaskAsync(int id, TaskItem taskItem)
        {
            AttachToken();

            return await SendAsync(async () => 
            {
                var response = await _httpClient.PutAsJsonAsync($"Tasks/{id}", taskItem);

                if (response.IsSuccessStatusCode)
                {
                    var updatedTask = await response.Content.ReadFromJsonAsync<TaskItem>();

                    return ApiResult<TaskItem>.Ok(updatedTask!);
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                var errorType = response.StatusCode.ToApiErrorType();

                return ApiResult<TaskItem>.Fail(errorMessage, errorType);
            });

        }

        public async Task<ApiResult> DeleteTaskAsync(int id, uint rowVersion)
        {
            AttachToken();

            return await SendAsync(async() =>
            {
                var response = await _httpClient.DeleteAsync($"Tasks/{id}?rowVersion={rowVersion}");

                if (response.IsSuccessStatusCode)
                {
                    return ApiResult.Ok();
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                var errorType = response.StatusCode.ToApiErrorType();

                return ApiResult.Fail(errorMessage, errorType);
            });
        }
    }
}
