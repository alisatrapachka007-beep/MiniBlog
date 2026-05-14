using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBlog.Models;
using MiniBlog.Services;
using MiniBlog.ViewModels;

namespace MiniBlog.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        // GET: /Posts
        public async Task<IActionResult> Index()
        {
            var posts = await _postService.GetAllPostsAsync();
            return View(posts);
        }

        // GET: /Posts/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null) return NotFound();
            return View(post);
        }

        // GET: /Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var post = new Post
                {
                    Title = viewModel.Title,
                    Content = viewModel.Content,
                    Author = viewModel.Author,
                    CreatedAt = DateTime.Now
                };

                await _postService.CreatePostAsync(post);
                TempData["SuccessMessage"] = "Пост опубликован!";
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: /Posts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null) return NotFound();
            return View(post);
        }

        // POST: /Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post post)
        {
            if (id != post.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _postService.UpdatePostAsync(post);
                    TempData["SuccessMessage"] = "Пост обновлён!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _postService.GetPostByIdAsync(id) == null)
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: /Posts/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null) return NotFound();
            return View(post);
        }

        // POST: /Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _postService.DeletePostAsync(id);
            TempData["SuccessMessage"] = "Пост удалён!";
            return RedirectToAction(nameof(Index));
        }
    }
}