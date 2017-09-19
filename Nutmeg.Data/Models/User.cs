using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Nutmeg.Data;

namespace Nutmeg.Data
{
    public partial class User: IdentityUser<Guid>, IKeyed<Guid>
	{
        public User()
        {
            ActionCreatedBy = new HashSet<Action>();
            ActionLastModifiedBy = new HashSet<Action>();
            AppDictionaryCreatedBy = new HashSet<AppDictionary>();
            AppDictionaryLastModifiedBy = new HashSet<AppDictionary>();
            AppNotification = new HashSet<AppNotification>();
            ChallengeCreatedBy = new HashSet<Challenge>();
            ChallengeLastModifiedBy = new HashSet<Challenge>();
            ClubCreatedBy = new HashSet<Club>();
            ClubLastModifiedBy = new HashSet<Club>();
            ClubManagerCreatedBy = new HashSet<ClubManager>();
            ClubManagerLastModifiedBy = new HashSet<ClubManager>();
            ClubManagerUser = new HashSet<ClubManager>();
            CoachCreatedBy = new HashSet<Coach>();
            CoachLastModifiedBy = new HashSet<Coach>();
            CoachUser = new HashSet<Coach>();
            GroupCreatedBy = new HashSet<Group>();
            GroupLastModifiedBy = new HashSet<Group>();
            NotificationSubscription = new HashSet<NotificationSubscription>();
            ParentChildUser = new HashSet<Parent>();
            ParentCreatedBy = new HashSet<Parent>();
            ParentLastModifiedBy = new HashSet<Parent>();
            ParentParentUser = new HashSet<Parent>();
            PlayerCreatedBy = new HashSet<Player>();
            PlayerLastModifiedBy = new HashSet<Player>();
            PlayerUser = new HashSet<Player>();
            RoleCreatedBy = new HashSet<Role>();
            RoleLastModifiedBy = new HashSet<Role>();
            TeamCreatedBy = new HashSet<Team>();
            TeamLastModifiedBy = new HashSet<Team>();
            UserAction = new HashSet<UserAction>();
            UserChallenge = new HashSet<UserChallenge>();
        }

        //public Guid Id { get; set; }
        public long IndexId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }

        public virtual ICollection<Action> ActionCreatedBy { get; set; }
        public virtual ICollection<Action> ActionLastModifiedBy { get; set; }
        public virtual ICollection<AppDictionary> AppDictionaryCreatedBy { get; set; }
        public virtual ICollection<AppDictionary> AppDictionaryLastModifiedBy { get; set; }
        public virtual ICollection<AppNotification> AppNotification { get; set; }
        public virtual ICollection<Challenge> ChallengeCreatedBy { get; set; }
        public virtual ICollection<Challenge> ChallengeLastModifiedBy { get; set; }
        public virtual ICollection<Club> ClubCreatedBy { get; set; }
        public virtual ICollection<Club> ClubLastModifiedBy { get; set; }
        public virtual ICollection<ClubManager> ClubManagerCreatedBy { get; set; }
        public virtual ICollection<ClubManager> ClubManagerLastModifiedBy { get; set; }
        public virtual ICollection<ClubManager> ClubManagerUser { get; set; }
        public virtual ICollection<Coach> CoachCreatedBy { get; set; }
        public virtual ICollection<Coach> CoachLastModifiedBy { get; set; }
        public virtual ICollection<Coach> CoachUser { get; set; }
        public virtual ICollection<Group> GroupCreatedBy { get; set; }
        public virtual ICollection<Group> GroupLastModifiedBy { get; set; }
        public virtual ICollection<NotificationSubscription> NotificationSubscription { get; set; }
        public virtual ICollection<Parent> ParentChildUser { get; set; }
        public virtual ICollection<Parent> ParentCreatedBy { get; set; }
        public virtual ICollection<Parent> ParentLastModifiedBy { get; set; }
        public virtual ICollection<Parent> ParentParentUser { get; set; }
        public virtual ICollection<Player> PlayerCreatedBy { get; set; }
        public virtual ICollection<Player> PlayerLastModifiedBy { get; set; }
        public virtual ICollection<Player> PlayerUser { get; set; }
        public virtual ICollection<Role> RoleCreatedBy { get; set; }
        public virtual ICollection<Role> RoleLastModifiedBy { get; set; }
        public virtual ICollection<Team> TeamCreatedBy { get; set; }
        public virtual ICollection<Team> TeamLastModifiedBy { get; set; }
        public virtual ICollection<UserAction> UserAction { get; set; }
        public virtual ICollection<UserChallenge> UserChallenge { get; set; }
    }
}
