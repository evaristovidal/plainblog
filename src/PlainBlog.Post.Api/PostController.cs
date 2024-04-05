using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlainBlog.Post.Abstractions;
using PlainBlog.Store.Entities;

namespace PlainBlog.Post.Api;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostManagementService _postManagementService;
    private readonly ILogger<PostController> _logger;

    public PostController(IPostManagementService postManagementService, ILogger<PostController> logger)
    {
        _postManagementService = postManagementService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken token)
    {
        _logger.LogDebug("Calling PostController.Get");
        var posts = await _postManagementService.GetAsync(token);

        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id, [FromQuery] bool? includeAuthor, CancellationToken token)
    {
        _logger.LogDebug("Calling PostController.Get");
        includeAuthor = includeAuthor ?? false;
        var post = await _postManagementService.GetAsync(id, includeAuthor.Value, token);
        if(post == null)
        {
            return NotFound();
        }

        return Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(PostSave model, CancellationToken token)
    {
        _logger.LogDebug("Calling PostController.Get");
        try
        {
            var newPostId = await _postManagementService.CreateAsync(model, token);
            var response = CreatedAtAction(nameof(CreateAsync), new { id = newPostId });
            return response;
        }
        catch (FluentValidation.ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { ErrorCode = e.ErrorCode, PropertyName = e.PropertyName, ErrorMessage = e.ErrorMessage });
            return BadRequest(errors);
        }
    }
}
