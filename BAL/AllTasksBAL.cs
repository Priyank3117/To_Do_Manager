 using Entities.ViewModels.AllTasksViewModel;
using Entities.ViewModels.HomeViewModels;
using Repository.Interface;

namespace BAL
{
    public class AllTasksBAL
    {
        private readonly IAllTasksRepository _AllTasksRepo;

        public AllTasksBAL(IAllTasksRepository allTasksRepo)
        {
            _AllTasksRepo = allTasksRepo;
        }

        /// <summary>
        /// Get All Task In All Task Page Team Wise
        /// </summary>
        /// <param name="filter">filter params like Team Name, Task Name, Start Date, End Date and Task Status</param>
        /// <returns>List of All Task Team wise</returns>
        public List<AllTasksOfAllTeams> GetAllTasks(Filter filter)
        {
            return _AllTasksRepo.GetAllTasks(filter);
        }

        public List<ListOfUsers> GetForAddTaskToToDo(long teamId, long userId)
        {
            return _AllTasksRepo.GetForAddTaskToToDo(teamId, userId);
        }

        /// <summary>
        /// Add Task To To-Do Page
        /// </summary>
        /// <param name="taskDetail">Teak Detail View Model for storing Task Id, Team Id and User Id</param>
        /// <returns>True - If Successfully Added alse False</returns>
        public bool AddTaskToTodayTask(TaskDetailViewModel taskDetail)
        {
            return _AllTasksRepo.AddTaskToTodayTask(taskDetail);
        }

        public bool AddTaskToTodayTaskForTeamMember(TaskDetailViewModel taskDetail)
        {
            return _AllTasksRepo.AddTaskToTodayTaskForTeamMember(taskDetail);
        }

        /// <summary>
        /// Get All Task Name for Calender View
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of all task for that user with task details like 
        /// Start Date, End Date and Task Status whether it is completed or not</returns>
        public List<AllTaskForCalenderView> GetTasksForCalenderView(long userId)
        {
            return _AllTasksRepo.GetTasksForCalenderView(userId);
        }
    }
}
