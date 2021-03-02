using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newsy.Models.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Services;
using BLL.Models.Dtos;
using BLL;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Newsy.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private ILogger<ArticleController> _logger;
        private IArticleService _articleService;

        public ArticleController(ILogger<ArticleController> logger,
            IArticleService articleService)
        {
            _logger = logger;
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int pageSize = 10, int page = 1)
        {
            _logger.LogInformation("Getting articles started.");

            var articles = await _articleService.GetNewestArticlesAsync(pageSize, page);

            _logger.LogInformation("Getting articles finished.");

            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"Getting an article {id} started.");

            var article = await _articleService.GetArticleByIdAsync(id);

            if (article == null)
			{
                _logger.LogInformation($"Article {id} not found. Getting an article finished.");
                return NotFound();
			}
            
            _logger.LogInformation($"Getting an article {id} finished.");

            return Ok(article);
        }

        [Authorize(Roles = UserRoles.Author)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ArticleBindingModel articleModel)
        {
            _logger.LogInformation($"Posting a new article");

            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var articleDto = new ArticleDto
            {
                Content = articleModel.Content,
                Description = articleModel.Description,
                Title = articleModel.Title
            };
            var article = await _articleService.InsertAsync(articleDto, userId);

            _logger.LogInformation($"Posting a new article finished!");

            return Ok(article);
        }

        [Authorize(Roles = UserRoles.Author)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ArticleBindingModel articleModel)
        {
            _logger.LogInformation($"Modification of the article {id} started.");

            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var isOwner = await _articleService.IsOwnerAsync(id, userId);
            if (!isOwner)
			{
                _logger.LogWarning($"User {userId} is not allowed to modify article {id}.");
                return Forbid();
			}

            var articleDto = new ArticleDto
            {
                Content = articleModel.Content,
                Description = articleModel.Description,
                Title = articleModel.Title
            };
            articleDto = await _articleService.UpdateAsync(id, articleDto);

            _logger.LogInformation($"Modification of the article {id} finished.");

            return Ok(articleDto);
        }

        [Authorize(Roles = UserRoles.Author)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deletion of the article {id} started.");

            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var isOwner = await _articleService.IsOwnerAsync(id, userId);
            if (!isOwner)
            {
                _logger.LogWarning($"User {userId} is not allowed to delete article {id}.");
                return Forbid();
            }

            await _articleService.DeleteAsync(id);

            _logger.LogInformation($"Deletion of the article {id} finished.");

            return Ok();
        }
    }
}
