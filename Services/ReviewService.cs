using data.ApplicationDbContext;
using Reviews.Models;
using Reviews.DTOs;
using Microsoft.EntityFrameworkCore;
using Namotion.Reflection;

public class ReviewService : IReviewService
{
    private readonly ReviewsDbContext _context;

    public ReviewService(ReviewsDbContext context)
    {
        _context = context;
    }

    public async Task<IResult> PostReview(ReviewCreateDto reviewCreateDto, string userId)
    {
        var review = new Review
        {
            UserId = userId,
            MovieId = reviewCreateDto.MovieId,
            Text = reviewCreateDto.Text,
            Rating = reviewCreateDto.Rating,
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return TypedResults.Created($"/reviews/{review.ReviewId}", review);
    }

    public async Task<IEnumerable<ReviewReadDto>> GetReviewsByUser(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId)) { return Enumerable.Empty<ReviewReadDto>(); }

        var query = _context.Reviews.AsQueryable();

        query = query.Where(r =>
            EF.Functions.Like(r.UserId, userId)
        );

        var result = await query.Select(r => new ReviewReadDto
        {
            ReviewId = r.ReviewId,
            UserId = r.UserId,
            MovieId = r.MovieId,
            Text = r.Text,
            Rating = r.Rating

        }).ToListAsync();

        return result;
    }

    public async Task<IEnumerable<ReviewReadDto>> GetReviewsByMovie(int movieId)
    {
        var query = _context.Reviews.AsQueryable();
        var result = await query.Where(
            r => r.MovieId == movieId
        ).Select(

            r => new ReviewReadDto
            {
                ReviewId = r.ReviewId,
                UserId = r.UserId,
                MovieId = r.MovieId,
                Text = r.Text,
                Rating = r.Rating

            }
        ).ToListAsync();

        return result;
    }
    
    public async Task<ReviewReadDto> GetReviewById(int reviewId)
    {
        var review = await _context.Reviews.FindAsync(reviewId);
        var result = new ReviewReadDto
        {
            ReviewId = review.ReviewId,
            UserId = review.UserId,
            MovieId = review.MovieId,
            Text = review.Text,
            Rating = review.Rating
        };
        
        return result;    
    }
}