﻿using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IPersonRepository _personRepository;
        private readonly IPictureRepository _pictureRepository;
        private readonly IMapper _mapper;

        public PostRepository(InternShipAppSystemContext context, IPersonRepository personRepository, IPictureRepository pictureRepository, IMapper mapper)
        {
            _context = context;
            _personRepository = personRepository;
            _pictureRepository = pictureRepository;
            _mapper = mapper;
        }

        public PostDto CreatePost(PostDto post)
        {
            var postToAdd = _mapper.Map<Post>(post);
            postToAdd.DateOfPost = DateTime.Now;
            _context.Posts.Add(postToAdd);
            return SaveAll() ? _mapper.Map<PostDto>(postToAdd) : null;
        }

        public bool DeletePost(int postId)
        {
            var post = _mapper.Map<Post>(GetPost(postId));
            if (post == null)
                return false;
            var allcomments = _context.Comments.Where(d => d.PostId == postId);
            foreach (var comment in allcomments)
            {
                comment.Deleted = true;
                _context.Comments.Update(comment);
            }
            post.Deleted = true;
            _context.Posts.Update(post);
            return true;
        }

        public IEnumerable<PostDto> GetAllPosts()
        {
            var allPosts = _context.Posts.Where(p => p.Deleted == false)
                .OrderByDescending(p => p.DateOfPost);
            var allPostsDto = _mapper.Map<IEnumerable<PostDto>>(allPosts);
            foreach (var a in allPostsDto)
            {
                a.Person = _personRepository.GetPersonById(a.PersonId.Value);
                var isUp = _context.PostCommentReactions.SingleOrDefault(p => p.PostId == a.PostId && p.PersonId == a.PersonId && p.Deleted == false);
                if (isUp == null)
                {
                    a.Upvote = false;
                    a.Downvote = false;
                }
                else
                {
                    a.Upvote = isUp.Upvote.Value;
                    a.Downvote = isUp.DownVote.Value;

                }
                var likes = _context.PostCommentReactions.Where(p =>
                p.Deleted == false
                && p.PostId != null
                && p.PostId == a.PostId
                && p.CommentId == null
                && p.Upvote == true
                && p.DownVote == false).Count();
                var dislikes = _context.PostCommentReactions.Where(p =>
                p.Deleted == false
                && p.PostId != null
                && p.PostId == a.PostId
                && p.CommentId == null
                && p.Upvote == false
                && p.DownVote == true).Count();

                a.Picture = a.PictureId == null ? null : _pictureRepository.GetById(a.PictureId.Value);
                a.Karma = likes - dislikes;
                a.Views = _context.PostViews.Where(pw => pw.Deleted == false && pw.PostId == a.PostId).Count();
            }
            return allPostsDto;
        }

        public IEnumerable<PostDto> GetAllPostsByPersonId(int personId)
        {
            return GetAllPosts().Where(p=>p.PersonId.Value == personId);
        }

        public PostDto GetPost(int postId)
        {
            return GetAllPosts().SingleOrDefault(p => p.PostId == postId);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public PostDto UpdatePost(PostDto post)
        {
            var postToUpdate = GetPost(post.PostId);
            if (postToUpdate == null)
                return null;
            postToUpdate.Title = post.Title == null ? postToUpdate.Title : post.Title;
            postToUpdate.Content = post.Content == null ? postToUpdate.Content : post.Content;
            _context.Posts.Update(_mapper.Map<Post>(postToUpdate));
            return SaveAll() ? GetPost(post.PostId) : null;
        }
    }
}
