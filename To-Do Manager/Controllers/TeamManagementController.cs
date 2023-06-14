using BAL;
using Entities.ViewModels.TeamManagement;
using Microsoft.AspNetCore.Mvc;
using To_Do_Manager.Filters;

namespace To_Do_Manager.Controllers
{
    [CheckSessionFilter]
    public class TeamManagementController : Controller
    {
        private TeamManagementBAL _TeamManagementBAL;
        private HomeBAL _HomeBAL;

        public TeamManagementController(TeamManagementBAL teamManagementBAL, HomeBAL homeBAL)
        {
            _TeamManagementBAL = teamManagementBAL;
            _HomeBAL = homeBAL;
        }

        /// <summary>
        /// Get Team Management Page
        /// </summary>
        /// <returns>Team Management Page</returns>
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Avatar = HttpContext.Session.GetString("Avatar");

            return View();
        }

        /// <summary>
        /// Get Available All Teams
        /// </summary>
        /// <returns>List of available team with their name</returns>
        public IActionResult GetAllTeams()
        {
            return PartialView("~/Views/PartialViews/TeamManagement/_AllTeamsForJoinTeam.cshtml", _HomeBAL.GetAllTeams("", long.Parse(HttpContext.Session.GetString("UserId")!)));
        }

        /// <summary>
        /// Get All Team Details Partial View
        /// </summary>
        /// <returns>All Team Details Partial View</returns>
        public IActionResult GetAllTeamDetailsPartialView()
        {
            return PartialView("~/Views/PartialViews/TeamManagement/_AllTeamWithDetails.cshtml", _TeamManagementBAL.GetAllTeamsDetails(long.Parse(HttpContext.Session.GetString("UserId")!)));
        }

        #region ManageUserInTeam
        /// <summary>
        /// Add User In Team
        /// </summary>
        /// <param name="addUsers">Email of user</param>
        /// <returns>True - If successfully Added alse False</returns>
        public bool AddUserInTeam(AddUsersInTeam addUsers)
        {
            return _TeamManagementBAL.AddUser(addUsers);
        }

        /// <summary>
        /// Remove User from Team
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="teamId">Team Id</param>
        /// <returns>True - If succesfully removed alse False</returns>
        public bool RemoveUserFromTeam(long userId, long teamId)
        {
            return _TeamManagementBAL.RemoveUserFromTeam(userId, teamId);
        }
        #endregion

        #region ManageRepotingPerson
        /// <summary>
        /// Get Al lMember To Set Reporting Person
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="teamId">Team Id</param>
        /// <returns>List of User's details</returns>
        public List<UserDetailOfTeam> GetAllMemberToSetReportingPerson(long userId, long teamId)
        {
            return _TeamManagementBAL.GetAllMemberToSetReportingPerson(userId, teamId);
        }

        /// <summary>
        /// Set Reporting Person
        /// </summary>
        /// <param name="userIdOfTeamMember">User Id of Team Member</param>
        /// <param name="userIdOfReportingPerson">User Id of Reporting Person</param>
        /// <param name="teamId">Team Id</param>
        /// <returns>True - If successfully set alse False</returns>
        public bool SetReportingPerson(long userIdOfTeamMember, long userIdOfReportingPerson, long teamId)
        {
            return _TeamManagementBAL.SetReportingPerson(userIdOfTeamMember, userIdOfReportingPerson, teamId);
        }

        /// <summary>
        /// Remove Reporting Person
        /// </summary>
        /// <param name="teamMemberUserId">User Id of Team Member</param>
        /// <param name="reportingPersonUserId">User Id of Reporting Person</param>
        /// <param name="teamId">Team Id</param>
        /// <returns>True - If successfully removed alse False</returns>
        public bool RemoveReportingPerson(long teamMemberUserId, long reportingPersonUserId, long teamId)
        {
            return _TeamManagementBAL.RemoveReportingPerson(teamMemberUserId, reportingPersonUserId, teamId);
        }
        #endregion

        #region ManageJoinRequest
        /// <summary>
        /// Accept Join Request in Team
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="teamId">Team Id</param>
        /// <returns>True - If successfully accept the request</returns>
        public bool AcceptJoinRequest(long userId, long teamId)
        {
            return _TeamManagementBAL.AcceptJoinRequest(userId, teamId);
        }

        /// <summary>
        /// Decline Join Request
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="teamId">Team Id</param>
        /// <returns>True - If successfully declined else False</returns>
        public bool DeclineJoinRequest(long userId, long teamId)
        {
            return _TeamManagementBAL.RemoveUserFromTeam(userId, teamId);
        }
        #endregion

        #region ManageLeaveRequest
        /// <summary>
        /// Accept Leave Request
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="teamId">Team Id of User</param>
        /// <returns>True - If successfuly removed from team else Fasle</returns>
        public bool AcceptLeaveRequest(long userId, long teamId)
        {
            return _TeamManagementBAL.RemoveUserFromTeam(userId, teamId);
        }

        /// <summary>
        /// Decline Leave Request
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="teamId">Team Id of User</param>
        /// <returns>True - If successfuly declined join request else Fasle</returns>
        public bool DeclineLeaveRequest(long userId, long teamId)
        {
            return _TeamManagementBAL.DeclineLeaveRequest(userId, teamId);
        }
        #endregion

        /// <summary>
        /// Delete Team
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <returns>True - If successfully team deleted else False</returns>
        public bool DeleteTeam(long teamId)
        {
            return _TeamManagementBAL.DeleteTeam(teamId);
        }
    }
}
