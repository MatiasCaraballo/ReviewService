using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Reviews.DTOs;

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
            var userId = this.User.Claims.ToList()[1].Value;

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


}