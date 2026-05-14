using Microsoft.EntityFrameworkCore;
using MiniBlog.Data;
using MiniBlog.Models;

namespace MiniBlog.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;

        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task CreatePostAsync(Post post)
        {
            post.CreatedAt = DateTime.Now;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
    }
}