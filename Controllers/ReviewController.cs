using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Reviews.DTOs;
using System.Security.Claims;

[ApiController]
[Route("[controller]")]

public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Admin,Client")]

    public async Task<ActionResult<ReviewCreateDto>> PostReview(ReviewCreateDto reviewCreateDto)
    {

        try
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;


            var review = await _reviewService.PostReview(reviewCreateDto, userId);

            if (review == null) { return NotFound(); }
            return Ok(review);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");
        }

    }

    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Admin,Client")]

    public async Task<ActionResult<IEnumerable<ReviewReadDto>>> GetReviewsByUser(string userId)
    {
        try
        {
            var reviews = await _reviewService.GetReviewsByUser(userId);
            return Ok(reviews);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");
        }
    }

    [HttpGet("movie/{movieId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Admin,Client")]

    public async Task<ActionResult<IEnumerable<ReviewReadDto>>> GetReviewsByMovie(int movieId)
    {
        try
        {
            var reviews = await _reviewService.GetReviewsByMovie(movieId);
            return Ok(reviews);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");
        }
    }

    [HttpGet("/{reviewId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Admin,Client")]

    public async Task<ActionResult<ReviewReadDto>> GetReviewById(int reviewId)
    {
        try
        {
            var review = await _reviewService.GetReviewById(reviewId);
            return review;
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");
        }
    }

    [HttpGet("most-reviewed-movies-page/{page}-perpage/{perPage}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Admin,Client")]

    public async Task<ActionResult<IEnumerable<ReviewReadDto>>> GetMostReviewedMovies(int page, int perPage)
    {
        try
        {
            var reviews = await _reviewService.GetMostReviewedMovies(page, perPage);
            return Ok(reviews);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");
        }
    }

    [HttpGet("/best-reviewed-movies/{page}-perpage/{perPage}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReviewRankingRatingDto>> GetBestReviewedMovies(int page, int perPage)
    {
        try
        {
            var reviews = await _reviewService.GetBestReviewedMovies(page, perPage);
            return Ok(reviews);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");
        }
    }
    
    [HttpGet("/worst-reviewed-movies/{page}-perpage/{perPage}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReviewRankingRatingDto>> GetWorstReviewedMovies(int page, int perPage)
    {
        try
        {
            var reviews = await _reviewService.GetWorstReviewedMovies(page, perPage);
            return Ok(reviews); 
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");
        }
    }

    [HttpPatch()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Admin,Client")]

    public async Task<ActionResult> UpdateReview(int reviewId, ReviewUpdateDto reviewUpdateDto)
    {
        try
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var review = await _reviewService.UpdateReview(reviewId, userId, reviewUpdateDto);
            return Ok(review);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");
        }

    }

    [HttpDelete()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Admin,Client")]

    public async Task<ActionResult> DeleteReview(int reviewId)
    {
        try
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string userRole = User.FindFirst(ClaimTypes.Role).Value;

            var review = await _reviewService.DeleteReview(reviewId,userId,userRole);
            return Ok(review);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");
        }
    }



}