﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Verrukkulluk;
using Verrukkulluk.Data;

namespace Verrukkulluk.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICrud _crud;
        private IMapper _mapper;

        public CommentsController(ICrud crud, IMapper mapper)
        {
            _crud = crud;
            _mapper = mapper;
        }

        //// GET: api/Comments
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Comment>>> GetComment()
        //{
        //    return await _context.Comment.ToListAsync();
        //}

        //// GET: api/Comments/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Comment>> GetComment(int id)
        //{
        //    var comment = await _context.Comment.FindAsync(id);

        //    if (comment == null)
        //    {
        //        return NotFound();
        //    }

        //    return comment;
        //}

        //// PUT: api/Comments/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutComment(int id, Comment comment)
        //{
        //    if (id != comment.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(comment).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CommentExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Comments
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Comment>> PostComment(Comment comment)
        //{
        //    _context.Comment.Add(comment);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        //}

        //// DELETE: api/Comments/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteComment(int id)
        //{
        //    var comment = await _context.Comment.FindAsync(id);
        //    if (comment == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Comment.Remove(comment);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool CommentExists(int id)
        //{
        //    return _context.Comment.Any(e => e.Id == id);
        //}
    }
}
