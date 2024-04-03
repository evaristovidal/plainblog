﻿namespace PlainBlog.Post.Abstractions;

public interface IPostManagementService
{
    Task<IEnumerable<Abstractions.Post>> GetAsync(CancellationToken token);
    Task<Abstractions.Post?> GetAsync(int postId, CancellationToken token);
    Task<int> CreateAsync(PostSave model, CancellationToken token);
}
