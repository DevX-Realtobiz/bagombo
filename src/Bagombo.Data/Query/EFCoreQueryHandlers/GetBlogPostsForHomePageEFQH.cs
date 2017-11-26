﻿using Bagombo.Data.Query.Queries;
using Bagombo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bagombo.EFCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetBlogPostsForHomePageEFQH : EFQHBase, IQueryHandlerAsync<GetBlogPostsForHomePageQuery, PaginatedList<BlogPost>>
  {
    public GetBlogPostsForHomePageEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<PaginatedList<BlogPost>> HandleAsync(GetBlogPostsForHomePageQuery query)
    {
      var posts = _context.BlogPosts.Include(bp => bp.Comments)
        .Where(bp => bp.PublishOn < DateTime.Now && bp.Public == true)
        .Include(bp => bp.Author)
        .OrderByDescending(bp => bp.PublishOn);

      return await PaginatedList<BlogPost>.CreateAsync(posts, query.CurrentPage, query.PageSize);
    }
  }
}