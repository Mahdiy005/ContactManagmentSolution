using ContactManagment.DTOs;
using ContactManagment.DTOs.GlobaResponse;
using ContactManagment.Helper;
using ContactManagment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ContactManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ContactContext contactContext;

        public ContactController(ContactContext contactContext)
        {
            this.contactContext = contactContext;
        }


        [HttpPost]
        [Authorize]
        public IActionResult Add(ContactDTO contact)
        {
            // Get current user id
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var currentUserId))
                return BadRequest(ApiResponse<string>.FailedResponse("Invalid User Id"));

            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.FailedResponse("Validation Error", ModelState.GetErrors()));

            // Map DTO → Entity
            var contactEntity = new Contact
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                Birthdate = contact.Birthdate,
                UserId = currentUserId
            };

            contactContext.Contacts.Add(contactEntity);
            contactContext.SaveChanges();

            // Map back to DTO
            var savedDto = new ContactDTO
            {
                Id = contactEntity.Id,
                FirstName = contactEntity.FirstName,
                LastName = contactEntity.LastName,
                Email = contactEntity.Email,
                PhoneNumber = contactEntity.PhoneNumber,
                Birthdate = contactEntity.Birthdate
            };

            return CreatedAtAction(nameof(Display), new { id = contactEntity.Id }, ApiResponse<ContactDTO>.SuccessResponse(savedDto, "Addes Successfully"));
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Display(int id)
        {
            // Get current user id
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var currentUserId))
                return BadRequest(ApiResponse<string>.FailedResponse("Invalid User Id"));

            // Find contact for current user
            var contact = contactContext.Contacts
                .FirstOrDefault(c => c.Id == id && c.UserId == currentUserId);

            if (contact == null)
                return BadRequest(ApiResponse<string>.FailedResponse("Invalid Contact"));

            // Map to DTO
            var contactDto = new ContactDTO
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                Birthdate = contact.Birthdate
            };

            return Ok(ApiResponse<ContactDTO>.SuccessResponse(contactDto, "Retrieved Successfully"));
        }


        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            // Get current user id from claims
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var currentUserId))
                return BadRequest(ApiResponse<string>.FailedResponse("Invalid User Id"));

            // Find contact belonging to current user
            var contact = contactContext.Contacts
                .FirstOrDefault(c => c.Id == id && c.UserId == currentUserId);

            if (contact == null)
                return NotFound(ApiResponse<string>.FailedResponse("Contact not found"));

            // Remove contact
            contactContext.Contacts.Remove(contact);
            contactContext.SaveChanges();

            return Ok(ApiResponse<string>.SuccessResponse(null, "Deleted successfully"));
        }



        [HttpGet]
        [Authorize]
        public IActionResult GetAll(int page = 1,int pageSize = 3,string sortBy = "FirstName", string sortDirection = "asc")
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var currentUserId))
                return BadRequest(ApiResponse<string>.FailedResponse("Invalid User Id"));

            var query = contactContext.Contacts.Where(c => c.UserId == currentUserId);

            // Apply sorting
            query = sortBy.ToLower() switch
            {
                "lastname" => (sortDirection == "desc" ? query.OrderByDescending(c => c.LastName) : query.OrderBy(c => c.LastName)),
                "birthdate" => (sortDirection == "desc" ? query.OrderByDescending(c => c.Birthdate) : query.OrderBy(c => c.Birthdate)),
                _ => (sortDirection == "desc" ? query.OrderByDescending(c => c.FirstName) : query.OrderBy(c => c.FirstName))
            };

            var totalRecords = query.Count();

            // Apply pagination
            var contacts = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ContactDTO
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    Birthdate = c.Birthdate
                })
                .ToList();

            return Ok(new PaginatedResponse<ContactDTO>(
                contacts,
                page,
                pageSize,
                totalRecords,
                "Retrieved Successfully"
            ));
        }



    }
}
