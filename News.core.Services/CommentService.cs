using Microsoft.EntityFrameworkCore;
using News.core.IRepository;
using News.core.IServices;
using News.core.Model;
using News.core.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.core.Services
{

    public class CommentService : BaseService<Model.Entities.Comments>, ICommentService
    {
        private readonly ICommentRepostory _commentRepostory;
        private readonly IUserRepository _userRepository;
        private readonly ICommentChildRepository _commentChildRepository;
        private readonly INewsRepository _newsRepository;

        public CommentService(ICommentRepostory commentRepostory, IUserRepository userRepository, ICommentChildRepository commentChildRepository, INewsRepository newsRepository)
        {


            _commentRepostory = commentRepostory ?? throw new ArgumentNullException(nameof(commentRepostory));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _commentChildRepository = commentChildRepository ?? throw new ArgumentNullException(nameof(commentChildRepository));
            _newsRepository = newsRepository ?? throw new ArgumentNullException(nameof(newsRepository));
            base.BaseDal = commentRepostory;
        }

        public async Task<bool> DelAllComment(int commentId)
        {
            var commentChildList = await _commentChildRepository.Query(m => m.CommentsId == commentId);

            if (commentChildList != null)
            {
                foreach (var item in commentChildList)
                {
                    await _commentChildRepository.Delete(item);
                }
            }

            return await _commentRepostory.Delete(new Model.Entities.Comments() { Id = commentId });
        }

        public async Task<PageModel<Model.ViewModel.CommentViewModel>> GetAll(int newsId, QueryModel queryModel)
        {


            var news = await _newsRepository.GetOneById(newsId);

            List<Model.ViewModel.CommentViewModel> commentViewModels = new List<Model.ViewModel.CommentViewModel>();
            PageModel<Model.ViewModel.CommentViewModel> pageModel = new Model.PageModel<Model.ViewModel.CommentViewModel>();
            if (news != null)
            {
                //查出该博客所有一级评论
                var commentsList = await _commentRepostory.Query(m => m.NewsId == newsId);
                foreach (var comments in commentsList)
                {
                    Model.ViewModel.CommentViewModel commentViewModel = new Model.ViewModel.CommentViewModel();
                    commentViewModel.Id = comments.Id;
                    comments.User = await _userRepository.GetOneById(comments.UserId.Value);
                    commentViewModel.Comments.Add(comments);
                    commentViewModels.Add(commentViewModel);
                }
                var childCommentList = await _commentChildRepository.GetAll();
                childCommentList = childCommentList.OrderBy(m => m.CreateTime).ToList();
                //子评论添加到对应1级评论下边
                foreach (var item in commentViewModels)
                {


                    item.commentsChildren = childCommentList.FindAll(s => s.CommentsId == item.Id).ToArray();
                    foreach (var childComment in childCommentList)
                    {
                        childComment.User = await _userRepository.GetOneById(childComment.UserId.Value);
                    }
                }

                pageModel.dataCount = commentsList.Count;
                pageModel.pageCount = (int)Math.Ceiling(pageModel.dataCount / (double)queryModel.pageSize);
                pageModel.pageSize = queryModel.pageSize;
                pageModel.pageIndex = queryModel.pageIndex;
                var ss = commentViewModels.AsQueryable();
                pageModel.data = ss
                    .Skip((pageModel.pageIndex - 1) * pageModel.pageSize)
                    .Take(pageModel.pageSize)
                    .ToList() ?? null;
            }
            return pageModel;
        }

        public async Task<PageModel<CommentViewModel>> GetAllByUserId(int userId, QueryModel queryModel)
        {
            List<Model.ViewModel.CommentViewModel> commentViewModels = new List<Model.ViewModel.CommentViewModel>();
            PageModel<Model.ViewModel.CommentViewModel> pageModel = new Model.PageModel<Model.ViewModel.CommentViewModel>();
            var commentsList = await _commentRepostory.Query(m => m.UserId == userId);

            foreach (var comments in commentsList)
            {
                Model.ViewModel.CommentViewModel commentViewModel = new Model.ViewModel.CommentViewModel();
                commentViewModel.Id = comments.Id;
                commentViewModel.News = await _newsRepository.GetOneById(comments.NewsId.Value);
                comments.User = await _userRepository.GetOneById(comments.UserId.Value);
                commentViewModel.Comments.Add(comments);
                commentViewModels.Add(commentViewModel);
            }
            var childCommentList = await _commentChildRepository.GetAll();
            childCommentList = childCommentList.OrderBy(m => m.CreateTime).ToList();
            //子评论添加到对应1级评论下边
            foreach (var item in commentViewModels)
            {


                item.commentsChildren = childCommentList.FindAll(s => s.CommentsId == item.Id).ToArray();
                foreach (var childComment in childCommentList)
                {
                    childComment.User = await _userRepository.GetOneById(childComment.UserId.Value);
                }
            }
            pageModel.dataCount = commentsList.Count;
            pageModel.pageCount = (int)Math.Ceiling(pageModel.dataCount / (double)queryModel.pageSize);
            pageModel.pageSize = queryModel.pageSize;
            pageModel.pageIndex = queryModel.pageIndex;
            var ss = commentViewModels.AsQueryable();
            pageModel.data = ss
                .Skip((pageModel.pageIndex - 1) * pageModel.pageSize)
                .Take(pageModel.pageSize)
                .ToList() ?? null;

            return pageModel;
        }
    }
}
