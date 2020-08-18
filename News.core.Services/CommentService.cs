using News.core.IRepository;
using News.core.IServices;
using System;
using System.Collections.Generic;
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

        public async Task<List<Model.ViewModel.CommentViewModel>> GetAll(int newsId)
        {


            var news = await _newsRepository.GetOneById(newsId);

            List<Model.ViewModel.CommentViewModel> commentViewModels = new List<Model.ViewModel.CommentViewModel>();
            if (news != null)
            {


                #region MyRegion
                //查出该博客所有一级评论
                //var commentsList = (await _commentRepostory.GetAll()).FindAll(m => m.NewsId == newsId);
                ////处理一级评论数据
                //foreach (var item in commentsList)
                //{
                //    Model.ViewModel.CommentViewModel commentViewModel = new Model.ViewModel.CommentViewModel();
                //    commentViewModel.UserId = item.UserId;
                //    commentViewModel.User = await _userRepository.GetOneById(item.UserId.Value);
                //    commentViewModel.Id = item.Id;
                //    commentViewModel.Content = item.Content;
                //    commentViewModels.Add(commentViewModel);
                //}
                ////获取到所有子评论
                //var childCommentList = await _commentChildRepository.GetAll();
                ////子评论添加到对应1级评论下边
                //foreach (var item in commentViewModels)
                //{


                //    item.commentsChildren = childCommentList.FindAll(s => s.CommentsId == item.Id).ToArray();
                //    foreach (var childComment in childCommentList)
                //    {
                //        childComment.User = await _userRepository.GetOneById(childComment.UserId.Value);
                //    }
                //}
                #endregion

                //查出该博客所有一级评论
                var commentsList = (await _commentRepostory.GetAll()).FindAll(m => m.NewsId == newsId);
                foreach (var comments in commentsList)
                {
                    Model.ViewModel.CommentViewModel commentViewModel = new Model.ViewModel.CommentViewModel();
                    commentViewModel.Id = comments.Id;
                    comments.User = await _userRepository.GetOneById(comments.UserId.Value);
                    commentViewModel.Comments.Add(comments);
                    commentViewModels.Add(commentViewModel);
                }
                var childCommentList = await _commentChildRepository.GetAll();
                //子评论添加到对应1级评论下边
                foreach (var item in commentViewModels)
                {


                    item.commentsChildren = childCommentList.FindAll(s => s.CommentsId == item.Id).ToArray();
                    foreach (var childComment in childCommentList)
                    {
                        childComment.User = await _userRepository.GetOneById(childComment.UserId.Value);
                    }
                }
            }
            return commentViewModels;
        }
    }
}
