using data.ApplicationDbContext;
using Reviews.Models;
using Reviews.DTOs;
using Microsoft.EntityFrameworkCore;
using Reviews.Migrations;

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

    public async Task<IEnumerable<ReviewReadRankingDto>> GetMostReviewedMovies(int page, int perPage)
    {
        if (page <= 0) { page = 1; };

        if (perPage <= 0) { perPage = 10; };

        var query = _context.Reviews
            .GroupBy(r => r.MovieId)
            .Select(g => new ReviewReadRankingDto
            {
                MovieId = g.Key,
                ReviewsCount = g.Count()
            })
            .OrderByDescending(x => x.ReviewsCount)
            .Skip((page - 1) * perPage)
            .Take(perPage);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<ReviewRankingRatingDto>> GetBestReviewedMovies(int page, int perPage)
    {
        if (page <= 0) { page = 1; }
        ;

        if (perPage <= 0) { perPage = 10; }
        ;

        var query = _context.Reviews
                    .GroupBy(r => r.MovieId)
                    .Select(g => new ReviewRankingRatingDto
                    {
                        MovieId = g.Key,
                        Rating = g.Average(r => r.Rating)
                    })
                    .OrderByDescending(x => x.Rating)
                    .Skip((page - 1) * perPage)
                    .Take(perPage);

        return await query.ToListAsync();

    }
    
    public async Task<IEnumerable<ReviewRankingRatingDto>> GetWorstReviewedMovies (int page, int perPage)
    {
        if (page <= 0) { page = 1; };

        if (perPage <= 0) { perPage = 10; };

        var query = _context.Reviews
                    .GroupBy(r => r.MovieId)
                    .Select(g => new ReviewRankingRatingDto
                    {
                        MovieId = g.Key,
                        Rating = g.Average(r => r.Rating)
                    })
                    .OrderBy(x => x.Rating)
                    .Skip((page - 1) * perPage)
                    .Take(perPage);

        return await query.ToListAsync();

    }
    public async Task<IResult> UpdateReview(int reviewId, string userId, ReviewUpdateDto reviewUpdateDto)
    {
        var review = await _context.Reviews.FindAsync(reviewId);

        if (review == null) { return Results.NotFound(new { message = "Review not found", reviewId }); }

        if (review.UserId != userId)
        {
            return Results.Problem(
                detail: "Only the review's creator can modify it",
                statusCode: 403,
                title: "Forbidden"
            );
        }

        if (reviewUpdateDto.Rating != null) { review.Rating = (int)reviewUpdateDto.Rating; }
        ;

        if (reviewUpdateDto.Text != null) { review.Text = reviewUpdateDto.Text; }
        ;

        await _context.SaveChangesAsync();

        return Results.Ok(new { message = "Review correctly updated", reviewId });

    }

    public async Task<IResult> DeleteReview(int reviewId,string userId, string userRole)
    {
        var review = await _context.Reviews.FindAsync(reviewId);

        if (review == null) { return Results.NotFound(new { message = "Review not found", reviewId }); }

        if (review.UserId != userId && userRole != "Admin")
        {
            return Results.Problem(
                detail: "Only the review's creator or the admin can delete it",
                statusCode: 403,
                title: "Forbidden"
            );
        }
        _context.Reviews.Remove(review);

        await _context.SaveChangesAsync();
        
        return Results.Ok(new { message = "Review correctly deleted", reviewId });

    }
}