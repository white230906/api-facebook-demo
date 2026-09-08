using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using facebook_demo.Service.Exceptions;
using Microsoft.Extensions.Configuration;

namespace facebook_demo.Service.FacebookService;

public class Service : IService
{
    private readonly HttpClient _httpClient;
    private readonly FacebookOptions _facebookOptions = new();

    public Service(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        configuration.GetSection(nameof(FacebookOptions)).Bind(_facebookOptions);
    }

    public async Task<CreateFacebookPostResponse> CreatePostAsync(CreateFacebookPostRequest request)
    {
        Validate.FacebookConfiguration(_facebookOptions);
        Validate.CreatePostRequest(request);

        const string facebookBaseUrl = "https://graph.facebook.com/v25.0";
        var photoIds = new List<string>();

        // Upload từng ảnh lên Facebook ở trạng thái chưa đăng.
        foreach (var imageUrl in request.ImageUrls)
        {
            var uploadPhotoApi = $"{facebookBaseUrl}/{_facebookOptions.PageId}/photos";

            using var uploadPhotoRequest = new HttpRequestMessage(HttpMethod.Post, uploadPhotoApi);
            uploadPhotoRequest.Headers.Authorization = new AuthenticationHeaderValue(
                _facebookOptions.TokenType,
                _facebookOptions.AccessToken);
            uploadPhotoRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["url"] = imageUrl,
                ["published"] = "false"
            });

            using var uploadPhotoResponse = await _httpClient.SendAsync(uploadPhotoRequest);

            if (!uploadPhotoResponse.IsSuccessStatusCode)
            {
                var facebookError = await uploadPhotoResponse.Content.ReadAsStringAsync();

                throw new FacebookApiException(
                    $"Upload photo failed ({uploadPhotoResponse.StatusCode}): {facebookError}");
            }

            var uploadedPhoto = await uploadPhotoResponse.Content
                .ReadFromJsonAsync<FacebookIdResponse>();

            if (string.IsNullOrWhiteSpace(uploadedPhoto?.Id))
            {
                throw new FacebookApiException("Facebook returned an empty photo ID.");
            }

            photoIds.Add(uploadedPhoto.Id);
        }

        // Gắn các ảnh đã upload vào bài viết và đăng ngay lên Page.
        var createPostApi = $"{facebookBaseUrl}/{_facebookOptions.PageId}/feed";
        var postForm = new List<KeyValuePair<string, string>>
        {
            new("message", request.Message),
            new("published", "true")
        };

        for (var index = 0; index < photoIds.Count; index++)
        {
            var attachedMedia = JsonSerializer.Serialize(new
            {
                media_fbid = photoIds[index]
            });

            postForm.Add(new KeyValuePair<string, string>(
                $"attached_media[{index}]",
                attachedMedia));
        }

        using var createPostRequest = new HttpRequestMessage(HttpMethod.Post, createPostApi);
        createPostRequest.Headers.Authorization = new AuthenticationHeaderValue(
            _facebookOptions.TokenType,
            _facebookOptions.AccessToken);
        createPostRequest.Content = new FormUrlEncodedContent(postForm);

        using var createPostResponse = await _httpClient.SendAsync(createPostRequest);

        if (!createPostResponse.IsSuccessStatusCode)
        {
            var facebookError = await createPostResponse.Content.ReadAsStringAsync();

            throw new FacebookApiException(
                $"Create post failed ({createPostResponse.StatusCode}): {facebookError}");
        }

        var facebookPost = await createPostResponse.Content
            .ReadFromJsonAsync<FacebookIdResponse>();

        if (string.IsNullOrWhiteSpace(facebookPost?.Id))
        {
            throw new FacebookApiException("Facebook returned an empty post ID.");
        }

        return new CreateFacebookPostResponse
        {
            PostId = facebookPost.Id,
            PhotoIds = photoIds,
            PostUrl = $"https://www.facebook.com/{facebookPost.Id}"
        };
    }
}
