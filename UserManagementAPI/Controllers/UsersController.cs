using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Entities;
using UserManagementAPI.Repository;
using UserManagementAPI.Repository.Contracts;

namespace UserManagementAPI.Controllers
{
    /// <summary>
    /// End point used to intract with the User in the  UserManagment's Database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public class UserController : ControllerBase
    {
        private readonly IUserRepository _UserRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _Mapper;
        private readonly IAudit_logs _audit_Logs;
        private readonly IUsersRoleRepository _UserRoleRepository;
        /// <summary>
        /// User Constroller Constructor
        /// </summary>
        /// <param name="UserRepository"></param>
        /// <param name="logger"></param>
        /// <param name="Mapper"></param>
        /// <param name="audit_Logs"></param>
        /// /// <param name="UserRoleRepository"></param>
        public UserController(IUserRepository UserRepository, ILoggerService logger,
           IMapper Mapper, IAudit_logs audit_Logs, IUsersRoleRepository UserRoleRepository)
        {
            _UserRepository = UserRepository;
            _logger = logger;
            _Mapper = Mapper;
            _audit_Logs = audit_Logs;
            _UserRoleRepository = UserRoleRepository;
        }
        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns>List of Users</returns>

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                _logger.LogInfo("Attempted Get all the Users");
                var Users = await _UserRepository.FindAll();
                var respose = _Mapper.Map<IList<UsersDTO>>(Users);
                _logger.LogInfo("Successfully got all the Users");
                return Ok(respose);
            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message}-{ex.InnerException}");

            }

        }
        /// <summary>
        /// Get An User by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User's record</returns>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                _logger.LogInfo($"Attempted Get the Users with id:{id}");
                var Users = await _UserRepository.FindById(id);
                if (Users == null)
                {
                    _logger.LogWarn($"User with id:{id} was not found");
                    return NotFound();
                }
                var respose = _Mapper.Map<UsersDTO>(Users);
                _logger.LogInfo($"Successfully got the User with id:{id}");
                return Ok(respose);
            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message}-{ex.InnerException}");
            }

        }
        /// <summary>
        /// Create an User
        /// </summary>
        /// <param name="UserDTO"></param>
        /// <returns></returns>
        [HttpPost]
      //  [Authorize]
        [Authorize(Roles ="admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] UsersCreateDTO UserDTO)
        {
            try
            {
                _logger.LogInfo("Attempted submission attempted");

                if (UserDTO == null)
                {
                    _logger.LogWarn("Empty request submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn("User data was incomplete");
                    return BadRequest(ModelState);
                }
                var User = _Mapper.Map<Users>(UserDTO);
                var isSuccess = await _UserRepository.Create(User);
                if (!isSuccess)
                {
                    return InternalError("User creation failed");
                }
                _logger.LogInfo("User created");
                Audit_logs audit = new Audit_logs()
                {
                    uid =User.id ,
                    action ="Create" ,
                    log =$"{User.id} has created,Name:{User.username},FirstName:{User.fname},LastName:{User.lname},Email:{User.Email},DOB:{User.dob},Phone:{User.Phone}" ,
                    datetime = DateTime.Now
                };
                await _audit_Logs.Create(audit);
                return Created("Create", new { User });
            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message}-{ex.InnerException}");
            }

        }
        /// <summary>
        /// Update User details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] UsersUpdateDTO UserDTO)
        {
            try
            {
                _logger.LogInfo($"User detail update with id:{id}");
                if (id < 1 || UserDTO == null || id != UserDTO.id)
                {
                    _logger.LogWarn("User update failed with bad data");
                    return BadRequest();
                }
                var isExist = await _UserRepository.isExist(id);
                if (!isExist)
                {
                    _logger.LogWarn($"User with id:{id} was not found");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn("User data was incomplete");
                    return BadRequest(ModelState);
                }
                var User = _Mapper.Map<Users>(UserDTO);
                var isSuccess = await _UserRepository.Update(User);
                if (!isSuccess)
                {
                    return InternalError("User operation failed");
                }
                _logger.LogInfo("User updated");
                Audit_logs audit = new Audit_logs()
                {
                    uid = User.id,
                    action = "Update",
                    log = $"{User.id} has Updated,Name:{User.username},FirstName:{User.fname},LastName:{User.lname},Email:{User.Email},DOB:{User.dob},Phone:{User.Phone}",
                    datetime = DateTime.Now
                };
                await _audit_Logs.Create(audit);
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message}-{ex.InnerException}");
            }
        }
        /// <summary>
        /// delete the User by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
    //    [Authorize]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInfo($"User detail delete with id:{id}");
                if (id < 1)
                {
                    _logger.LogWarn("User details not avaible");
                    return BadRequest();
                }
                var isExist = await _UserRepository.isExist(id);
                if (!isExist)
                {
                    _logger.LogWarn($"User with id:{id} was not found");
                    return NotFound();
                }
                var User = await _UserRepository.FindById(id);
                var isSuccess = await _UserRepository.Delete(User);
                if (!isSuccess)
                {
                    return InternalError("User delete failed");
                }
                _logger.LogInfo("User delete");
                Audit_logs audit = new Audit_logs()
                {
                    uid = User.id,
                    action = "Delete",
                    log = $"{User.id} has deleted,Name:{User.username},FirstName:{User.fname},LastName:{User.lname},Email:{User.Email},DOB:{User.dob},Phone:{User.Phone}",
                    datetime = DateTime.Now
                };
                await _audit_Logs.Create(audit);
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message}-{ex.InnerException}");
            }
        }
        /// <summary>
        /// Assign User Role 
        /// </summary>
        /// <param name="assignUserRole"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateUserRole")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserRole([FromBody] AssignUserRole assignUserRole)
        {
            try
            {
                _logger.LogInfo("Attempted submission attempted");

                if (assignUserRole == null)
                {
                    _logger.LogWarn("Empty request submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn("User data was incomplete");
                    return BadRequest(ModelState);
                }
                int UID =  _UserRoleRepository.AssignRoleUser(assignUserRole);
                var UserAudit = await _UserRepository.FindByUserName(assignUserRole.username.ToString());
                 var User_new = _Mapper.Map<IList<Users>>(UserAudit);
                _logger.LogInfo("User Role created");
                
                Audit_logs audit = new Audit_logs()
                {
                    uid = User_new[0].id,
                    action = "Create User Role",
                    log = $"{assignUserRole.rolename} Role has created for USER:{assignUserRole.username}",
                    datetime = DateTime.Now
                };
                await _audit_Logs.Create(audit);
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message}-{ex.InnerException}");
            }

        }
        /// <summary>
        /// Update User Role 
        /// </summary>
        /// <param name="updateassignUserRole"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateUserRole")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateAssignUserRole updateassignUserRole)
        {
            try
            {
                _logger.LogInfo("Attempted submission attempted");

                if (updateassignUserRole == null)
                {
                    _logger.LogWarn("Empty request submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn("User data was incomplete");
                    return BadRequest(ModelState);
                }
                int UID = _UserRoleRepository.UpdateAssignRoleUser(updateassignUserRole);
                var UserAudit = await _UserRepository.FindByUserName(updateassignUserRole.username.ToString());
                var User_new = _Mapper.Map<IList<Users>>(UserAudit);
                _logger.LogInfo("User Role created");

                Audit_logs audit = new Audit_logs()
                {
                    uid = User_new[0].id,
                    action = "Update User Role",
                    log = $"{updateassignUserRole.username} Role has updated to{updateassignUserRole.rolename}",
                    datetime = DateTime.Now
                };
                await _audit_Logs.Create(audit);
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message}-{ex.InnerException}");
            }
        }
        /// <summary>
        /// Delete User Role
        /// </summary>
        /// <param name="deleteassignUserRole"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteUserRole")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUserRole([FromBody] DeleteAssignUserRole deleteassignUserRole)
        {
            try
            {
                _logger.LogInfo("Attempted submission attempted");

                if (deleteassignUserRole == null)
                {
                    _logger.LogWarn("Empty request submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn("User data was incomplete");
                    return BadRequest(ModelState);
                }
                int UID = _UserRoleRepository.DeleteAssignRoleUser(deleteassignUserRole);
                var UserAudit = await _UserRepository.FindByUserName(deleteassignUserRole.username.ToString());
                var User_new = _Mapper.Map<IList<Users>>(UserAudit);
                _logger.LogInfo("Role has deleted");

                Audit_logs audit = new Audit_logs()
                {
                    uid = User_new[0].id,
                    action = "Delete User Role",
                    log = $"{deleteassignUserRole.username} Role has deleted",
                    datetime = DateTime.Now
                };
                await _audit_Logs.Create(audit);
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message}-{ex.InnerException}");
            }
        }
        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong,Please contact the Adminstrator");
        }
    }
}
