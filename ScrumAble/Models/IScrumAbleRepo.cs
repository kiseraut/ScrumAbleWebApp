﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ScrumAble.Areas.Identity.Data;

namespace ScrumAble.Models
{
    public interface IScrumAbleRepo
    {
        //Task methods
        public ScrumAbleTask GetTaskById(int id);
        public ScrumAbleTask PopulateTaskMetadata(ScrumAbleTask task);
        public void SaveToDb(ScrumAbleTask task);
        public void DeleteFromDb(ScrumAbleTask task);
        public bool IsAuthorized(ScrumAbleTask task, string userId);

        //User methods
        public ScrumAbleUser GetUserById(string id);
        public ScrumAbleUser GetUserByUsername(string username);
        public void SaveToDb(ScrumAbleUser user);
        public void DeleteFromDb(ScrumAbleUser user);
        public void SetCurrentRelease(string userId, int releaseId);
        public void SetCurrentSprint(string userId, int sprintId);
        public void SetCurrentTeam(string userId, int TeamId);

        //Sprint methods
        public ScrumAbleSprint GetSprintById(int id);
        public bool IsAuthorized(ScrumAbleSprint sprint, string userId);

        //Story methods
        public ScrumAbleStory GetStoryById(int id);
        public bool IsAuthorized(ScrumAbleStory story, string userId);

        //ViewModelTaskAggregate methods
        public ViewModelTaskAggregate GetTaskAggregateData(string userId);

        //Team methods
        public ScrumAbleTeam GetTeamById(int id);
        public bool IsAuthorized(ScrumAbleTeam team, string userId);
        public List<ScrumAbleTeam> getAllUserTeams(string userID);
        public void DeleteFromDb(ScrumAbleRelease release);

        //Release methods
        public ScrumAbleRelease GetReleaseById(int id);
        public bool IsAuthorized(ScrumAbleRelease release, string userId);
        public void SaveToDb(ScrumAbleRelease scrumAbleRelease);

        //UserTeamMapping methods
        public void SaveToDb(ScrumAbleTeam team, List<IScrumAbleUser> users);
        public void DeleteFromDb(ScrumAbleTeam team);


        
    }
}