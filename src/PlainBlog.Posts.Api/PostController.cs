using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlainBlog.Store.Entities;

namespace PlainBlog.Posts.Api;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private static readonly Post[] Posts = new[]
        {
            new Post(){ Id = 1, Title ="Title1", Description="Description1", Content="Content1", AutorId= 1 },
            new Post(){ Id = 1, Title ="Title2", Description="Description2", Content="Content2", AutorId= 1 },
            new Post(){ Id = 1, Title ="Title3", Description="Description3", Content="Content3", AutorId= 1 },
            new Post(){ Id = 1, Title ="Title4", Description="Description4", Content="Content4", AutorId= 1 },
        };

    private readonly ILogger<PostController> _logger;

    public PostController(ILogger<PostController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetPosts")]
    public IEnumerable<Post> Get()
    {
        _logger.LogDebug("Calling PostController.Get");
        return Posts.ToArray();
    }
}
