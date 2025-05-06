using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRS.Models;

namespace PRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly PRSDBContext _context;

        public RequestsController(PRSDBContext context)
        {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Requests
        [HttpPost] //use DTO to prevent circular references to Request, Line Item, Product
        public async Task<ActionResult<Request>> PostRequest(RequestDTO requestDto)
        {
            var request = new Request
            {
                UserId = requestDto.UserId,
                RequestNumber = GetNextRequestNumber(),
                Description = requestDto.Description,
                Justification = requestDto.Justification,
                DateNeeded = requestDto.DateNeeded,
                DeliveryMode = requestDto.DeliveryMode,
                Status = requestDto.Status,
                Total = requestDto.Total,
                SubmittedDate = requestDto.SubmittedDate,
                ReasonForRejection = requestDto.ReasonForRejection
            };

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }


        // Helper method to generate the next request number
        private string GetNextRequestNumber()
        {
            // Request number format: RYYMMDDXXXX
            string requestNbr = "R";

            // Add YYMMDD string
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            requestNbr += today.ToString("yyMMdd");

            // Get the maximum request number from the database
            string? maxReqNbr = _context.Requests
                .Where(r => r.RequestNumber != null && r.RequestNumber.StartsWith(requestNbr))
                .Max(r => r.RequestNumber);

            string reqNbr;
            if (maxReqNbr != null)
            {
                // Extract the last 4 digits, increment, and pad with leading zeros
                string tempNbr = maxReqNbr.Substring(7);
                int nbr = int.Parse(tempNbr) + 1;
                reqNbr = nbr.ToString().PadLeft(4, '0');
            }
            else
            {
                // Start with "0001" if no existing request numbers
                reqNbr = "0001";
            }

            requestNbr += reqNbr;
            return requestNbr;
        }


        //_context.Requests.Add(request);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        //}

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }

        [HttpPost("login")]
        public ActionResult<User> GetPassword([FromBody] UserLoginDTO userlogin)
        {
            // Use FirstOrDefault to retrieve a single user or null
            var user = _context.Users.FirstOrDefault(u => u.UserName == userlogin.Username && u.Password == userlogin.Password);

            if (user == null)
            {
                // Return 404 Not Found if no user matches the credentials
                return NotFound("Username and password not found");
            }

            // Return 200 OK with the user if found
            return Ok(user);
        }

    }
}
   
// use post body to get the username and password.  From body only accepts a single object
//define the object (class UserLoginDTO) with the properties Username and Password