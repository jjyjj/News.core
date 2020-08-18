using Microsoft.AspNetCore.Mvc;
using News.core.IServices;
using News.core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.core.Controllers
{
    public class CommentChildController : BaseController
    {
        private readonly ICommentChildService _commentChildService;
        private readonly ICommentService _commentService;
        private readonly INewsService _newsService;
        private readonly IUserService _userService;

        public CommentChildController(ICommentChildService commentChildService, ICommentService commentService, INewsService newsService, IUserService userService)
        {
            _commentChildService = commentChildService ?? throw new ArgumentNullException(nameof(commentChildService));
            _commentService = commentService ?? throw new ArgumentNullException(nameof(commentService));
            _newsService = newsService ?? throw new ArgumentNullException(nameof(newsService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }


      
    }
}
