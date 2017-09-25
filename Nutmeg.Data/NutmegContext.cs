using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Nutmeg.Data;
using Action = Nutmeg.Data.Action;
using Microsoft.AspNetCore.Identity;

namespace Nutmeg.Data
{
	public partial class NutmegContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
	{
		#region Fields and Props
		public virtual DbSet<Action> Action { get; set; }
		public virtual DbSet<AppRole> AppRole { get; set; }
		public virtual DbSet<AppDictionary> AppDictionary { get; set; }
		public virtual DbSet<AppNotification> AppNotification { get; set; }
		public virtual DbSet<Challenge> Challenge { get; set; }
		public virtual DbSet<ChallengeAction> ChallengeAction { get; set; }
		public virtual DbSet<ChallengeLevel> ChallengeLevel { get; set; }
		public virtual DbSet<Club> Club { get; set; }
		public virtual DbSet<ClubManager> ClubManager { get; set; }
		public virtual DbSet<Coach> Coach { get; set; }
		public virtual DbSet<Group> Group { get; set; }
		public virtual DbSet<GroupRole> GroupRole { get; set; }
		public virtual DbSet<LogFile> LogFile { get; set; }
		public virtual DbSet<LogFileData> LogFileData { get; set; }
		public virtual DbSet<NotificationSubscription> NotificationSubscription { get; set; }
		public virtual DbSet<Parent> Parent { get; set; }
		public virtual DbSet<Player> Player { get; set; }
		public virtual DbSet<Role> Role { get; set; }
		public virtual DbSet<Team> Team { get; set; }
		public virtual DbSet<User> User { get; set; }
		public virtual DbSet<UserAction> UserAction { get; set; }
		public virtual DbSet<UserChallenge> UserChallenge { get; set; }
		public virtual DbSet<VersionHistory> VersionHistory { get; set; }
		#endregion Fields and Props
		#region Constructor
		public NutmegContext()//: base("name=FusionContext")
        {
		}
		public NutmegContext(DbContextOptions<NutmegContext> options) : base(options)
		{

		}
		#endregion Constructors
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			// Customize the ASP.NET Identity model and override the defaults if needed.
			// For example, you can rename the ASP.NET Identity table names and more.
			// Add your customizations after calling base.OnModelCreating(builder);
			modelBuilder.Entity<Action>(entity =>
			{
				entity.HasKey(i => i.Id)
					.ForSqlServerIsClustered(false);

				entity.HasIndex(e => e.CreatedById)
					.HasName("IX_Action_CreatedById");

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_Action_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.HasIndex(e => e.LastModifiedById)
					.HasName("IX_Action_LastModifiedById");

				entity.Property(e => e.Id).ValueGeneratedNever();

				entity.Property(e => e.Code)
					.IsRequired()
					.HasMaxLength(6);

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(500);

				entity.HasOne(d => d.CreatedBy)
					.WithMany(p => p.ActionCreatedBy)
					.HasForeignKey(d => d.CreatedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Action_CreatedByUser");

				entity.HasOne(d => d.LastModifiedBy)
					.WithMany(p => p.ActionLastModifiedBy)
					.HasForeignKey(d => d.LastModifiedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Action_LastModifiedByUser");
			});

			modelBuilder.Entity<AppDictionary>(entity =>
			{
				entity.HasKey(e => e.Key)
					.HasName("PK_AppDictionary");

				entity.Property(e => e.Key).HasMaxLength(50);

				entity.Property(e => e.IsVisible).HasDefaultValueSql("1");

				entity.Property(e => e.Value).IsRequired();

				entity.HasOne(d => d.CreatedBy)
					.WithMany(p => p.AppDictionaryCreatedBy)
					.HasForeignKey(d => d.CreatedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_AppDictionary_CreatedByUser");

				entity.HasOne(d => d.LastModifiedBy)
					.WithMany(p => p.AppDictionaryLastModifiedBy)
					.HasForeignKey(d => d.LastModifiedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_AppDictionary_LastModifiedByUser");
			});

			modelBuilder.Entity<AppNotification>(entity =>
			{
				entity.HasKey(i => i.Id)
					.ForSqlServerIsClustered(false);

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_AppNotification_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.HasIndex(e => e.UserId)
					.HasName("IX_AppNotification_UserId");

				entity.Property(e => e.Id).HasDefaultValueSql("newid()");

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.HasOne(d => d.User)
					.WithMany(p => p.AppNotification)
					.HasForeignKey(d => d.UserId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_AppNotification_UserId");
			});

			modelBuilder.Entity<Challenge>(entity =>
			{
				entity.HasKey(i => i.Id)
					.ForSqlServerIsClustered(false);

				entity.HasIndex(e => e.CreatedById)
					.HasName("IX_Challenge_CreatedById");

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_Challenge_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.HasIndex(e => e.LastModifiedById)
					.HasName("IX_Challenge_LastModifiedById");

				entity.Property(e => e.Id).ValueGeneratedNever();

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(500);

				entity.HasOne(d => d.CreatedBy)
					.WithMany(p => p.ChallengeCreatedBy)
					.HasForeignKey(d => d.CreatedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Challenge_CreatedByUser");

				entity.HasOne(d => d.LastModifiedBy)
					.WithMany(p => p.ChallengeLastModifiedBy)
					.HasForeignKey(d => d.LastModifiedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Challenge_LastModifiedByUser");
			});

			modelBuilder.Entity<ChallengeAction>(entity =>
			{
				entity.HasKey(i => i.Id)
				.ForSqlServerIsClustered(false);

				entity.HasIndex(e => e.ActionId)
					.HasName("IX_ActionId");

				entity.HasIndex(e => e.ChallengeId)
					.HasName("IX_ChallengeId");

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_ChallengeAction_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.Property(e => e.Id).ValueGeneratedNever();

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.HasOne(d => d.Action)
					.WithMany(p => p.ChallengeAction)
					.HasForeignKey(d => d.ActionId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_ChallengeAction_Action");

				entity.HasOne(d => d.Challenge)
					.WithMany(p => p.ChallengeAction)
					.HasForeignKey(d => d.ChallengeId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_ChallengeAction_Challenge");
			});

			modelBuilder.Entity<ChallengeLevel>(entity =>
			{
				entity.HasKey(i => i.Id)
				.ForSqlServerIsClustered(false);

				entity.HasIndex(e => e.ChallengeId)
					.HasName("IX_ChallengeId");

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_ChallengeLevel_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.Property(e => e.Id).ValueGeneratedNever();

				entity.Property(e => e.Hyperlink)
					.IsRequired()
					.HasMaxLength(500);

				entity.Property(e => e.IconUrl)
					.IsRequired()
					.HasMaxLength(500);

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.Property(e => e.LockedIconUrl)
					.IsRequired()
					.HasMaxLength(500);

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(500);

				entity.HasOne(d => d.Challenge)
					.WithMany(p => p.ChallengeLevel)
					.HasForeignKey(d => d.ChallengeId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_ChallengeLevel_Challenge");
			});

			modelBuilder.Entity<Club>(entity =>
			{

				entity.HasKey(i => i.Id)
					.ForSqlServerIsClustered(false)
					.HasName("PK_Club");

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_Club_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.Property(e => e.Id)
					.ValueGeneratedNever();

				entity.Property(e => e.IndexId)
					.ValueGeneratedOnAddOrUpdate();

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(400);

				entity.HasIndex(e => e.CreatedById)
					.HasName("IX_Club_CreatedById");

				entity.HasOne(d => d.CreatedBy)
					.WithMany(p => p.ClubCreatedBy)
					.HasForeignKey(d => d.CreatedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Club_CreatedByUser");

				entity.HasIndex(e => e.LastModifiedById)
					.HasName("IX_Club_LastModifiedById");

				entity.HasOne(d => d.LastModifiedBy)
					.WithMany(p => p.ClubLastModifiedBy)
					.HasForeignKey(d => d.LastModifiedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Club_LastModifiedByUser");
			});

			modelBuilder.Entity<ClubManager>(entity =>
			{
				entity.HasKey(i => i.Id)
					.ForSqlServerIsClustered(false);

				entity.HasIndex(e => e.ClubId)
					.HasName("IX_ClubId");

				entity.HasIndex(e => e.CreatedById)
					.HasName("IX_ClubManager_CreatedById");

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_ClubManager_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.HasIndex(e => e.LastModifiedById)
					.HasName("IX_ClubManager_LastModifiedById");

				entity.HasIndex(e => e.UserId)
					.HasName("IX_UserId");

				entity.Property(e => e.Id).ValueGeneratedNever();

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(500);

				entity.HasOne(d => d.Club)
					.WithMany(p => p.ClubManager)
					.HasForeignKey(d => d.ClubId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_ClubManager_Club");

				entity.HasOne(d => d.CreatedBy)
					.WithMany(p => p.ClubManagerCreatedBy)
					.HasForeignKey(d => d.CreatedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_ClubManager_CreatedByUser");

				entity.HasOne(d => d.LastModifiedBy)
					.WithMany(p => p.ClubManagerLastModifiedBy)
					.HasForeignKey(d => d.LastModifiedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_ClubManager_LastModifiedByUser");

				entity.HasOne(d => d.User)
					.WithMany(p => p.ClubManagerUser)
					.HasForeignKey(d => d.UserId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_ClubManager_User");
			});

			modelBuilder.Entity<Coach>(entity =>
			{
				entity.HasKey(i => i.Id)
					.ForSqlServerIsClustered(false);
				entity.HasIndex(e => e.CreatedById)
					.HasName("IX_Coach_CreatedById");

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_Coach_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.HasIndex(e => e.LastModifiedById)
					.HasName("IX_Coach_LastModifiedById");

				entity.HasIndex(e => e.TeamId)
					.HasName("IX_TeamId");

				entity.HasIndex(e => e.UserId)
					.HasName("IX_UserId");

				entity.Property(e => e.Id).ValueGeneratedNever();

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(500);

				entity.HasOne(d => d.CreatedBy)
					.WithMany(p => p.CoachCreatedBy)
					.HasForeignKey(d => d.CreatedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Coach_CreatedByUser");

				entity.HasOne(d => d.LastModifiedBy)
					.WithMany(p => p.CoachLastModifiedBy)
					.HasForeignKey(d => d.LastModifiedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Coach_LastModifiedByUser");

				entity.HasOne(d => d.Team)
					.WithMany(p => p.Coach)
					.HasForeignKey(d => d.TeamId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Coach_Team");

				entity.HasOne(d => d.User)
					.WithMany(p => p.CoachUser)
					.HasForeignKey(d => d.UserId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Coach_User");
			});

			modelBuilder.Entity<Group>(entity =>
			{
				entity.HasKey(i => i.Id)
					.ForSqlServerIsClustered(false);

				entity.HasIndex(e => e.CreatedById)
					.HasName("IX_Group_CreatedById");

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_Group_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.HasIndex(e => e.LastModifiedById)
					.HasName("IX_Group_LastModifiedById");

				entity.Property(e => e.Id).HasDefaultValueSql("newid()");

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(500);

				entity.HasOne(d => d.CreatedBy)
					.WithMany(p => p.GroupCreatedBy)
					.HasForeignKey(d => d.CreatedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Group_CreatedByUser");

				entity.HasOne(d => d.LastModifiedBy)
					.WithMany(p => p.GroupLastModifiedBy)
					.HasForeignKey(d => d.LastModifiedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Group_LastModifiedByUser");
			});

			modelBuilder.Entity<GroupRole>(entity =>
			{
				entity.HasKey(i => i.Id)
					.ForSqlServerIsClustered(false);

				entity.HasIndex(e => e.GroupId)
					.HasName("IX_GroupId");

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_GroupRole_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.HasIndex(e => e.RoleId)
					.HasName("IX_RoleId");

				entity.Property(e => e.Id).HasDefaultValueSql("newid()");

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.HasOne(d => d.Group)
					.WithMany(p => p.GroupRole)
					.HasForeignKey(d => d.GroupId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_GroupRole_Group");

				entity.HasOne(d => d.Role)
					.WithMany(p => p.GroupRole)
					.HasForeignKey(d => d.RoleId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_GroupRole_Role");
			});

			modelBuilder.Entity<LogFile>(entity =>
			{
				entity.HasKey(i => i.Id)
					.ForSqlServerIsClustered(false);

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_LogFile_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.Property(e => e.Id).ValueGeneratedNever();

				entity.Property(e => e.FileName)
					.IsRequired()
					.HasMaxLength(200);

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.Property(e => e.SourceName)
					.IsRequired()
					.HasMaxLength(200);
			});

			modelBuilder.Entity<LogFileData>(entity =>
			{
				entity.HasKey(i => i.Id)
					.ForSqlServerIsClustered(false);

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_LogFileData_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.HasIndex(e => e.LogFileId)
					.HasName("IX_LogFileData_LogFileId")
					.IsUnique();

				entity.Property(e => e.Id).ValueGeneratedNever();

				entity.Property(e => e.FileData).IsRequired();

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.HasOne(d => d.LogFile)
					.WithOne(p => p.LogFileData)
					.HasForeignKey<LogFileData>(d => d.LogFileId)
					.HasConstraintName("FK_LogFileData_LogFile");
			});

			modelBuilder.Entity<NotificationSubscription>(entity =>
			{

				entity.HasKey(i => i.Id)
					.ForSqlServerIsClustered(false);

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_NotificationSubscription_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.HasIndex(e => e.UserId)
					.HasName("IX_NotificationSubscription_UserId");

				entity.Property(e => e.Id).HasDefaultValueSql("newid()");

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.HasOne(d => d.User)
					.WithMany(p => p.NotificationSubscription)
					.HasForeignKey(d => d.UserId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_NotificationSubscription_UserId");
			});

			modelBuilder.Entity<Parent>(entity =>
			{
				entity.HasKey(i => i.Id)
					.ForSqlServerIsClustered(false);

				entity.HasIndex(e => e.CreatedById)
					.HasName("IX_Parent_CreatedById");

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_Parent_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.HasIndex(e => e.LastModifiedById)
					.HasName("IX_Parent_LastModifiedById");

				entity.Property(e => e.Id).ValueGeneratedNever();

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(500);

				entity.HasOne(d => d.ChildUser)
					.WithMany(p => p.ParentChildUser)
					.HasForeignKey(d => d.ChildUserId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Parent_ChildUser");

				entity.HasOne(d => d.CreatedBy)
					.WithMany(p => p.ParentCreatedBy)
					.HasForeignKey(d => d.CreatedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Parent_CreatedByUser");

				entity.HasOne(d => d.LastModifiedBy)
					.WithMany(p => p.ParentLastModifiedBy)
					.HasForeignKey(d => d.LastModifiedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Parent_LastModifiedByUser");

				entity.HasOne(d => d.ParentUser)
					.WithMany(p => p.ParentParentUser)
					.HasForeignKey(d => d.ParentUserId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Parent_ParentUser");
			});

			modelBuilder.Entity<Player>(entity =>
			{
				entity.HasKey(i => i.Id)
	.				ForSqlServerIsClustered(false);


				entity.HasIndex(e => e.CreatedById)
					.HasName("IX_Player_CreatedById");

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_Player_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.HasIndex(e => e.LastModifiedById)
					.HasName("IX_Player_LastModifiedById");

				entity.HasIndex(e => e.TeamId)
					.HasName("IX_TeamId");

				entity.HasIndex(e => e.UserId)
					.HasName("IX_UserId");

				entity.Property(e => e.Id).ValueGeneratedNever();

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(500);

				entity.HasOne(d => d.CreatedBy)
					.WithMany(p => p.PlayerCreatedBy)
					.HasForeignKey(d => d.CreatedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Player_CreatedByUser");

				entity.HasOne(d => d.LastModifiedBy)
					.WithMany(p => p.PlayerLastModifiedBy)
					.HasForeignKey(d => d.LastModifiedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Player_LastModifiedByUser");

				entity.HasOne(d => d.Team)
					.WithMany(p => p.Player)
					.HasForeignKey(d => d.TeamId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Player_Team");

				entity.HasOne(d => d.User)
					.WithMany(p => p.PlayerUser)
					.HasForeignKey(d => d.UserId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Player_User");
			});

			modelBuilder.Entity<Role>(entity =>
			{
				entity.HasKey(i => i.Id)
					.ForSqlServerIsClustered(false);

				entity.HasIndex(e => e.CreatedById)
					.HasName("IX_Role_CreatedById");

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_Role_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.HasIndex(e => e.LastModifiedById)
					.HasName("IX_Role_LastModifiedById");

				entity.HasIndex(e => e.Name)
					.HasName("UX_Role_Name")
					.IsUnique();

				entity.Property(e => e.Id).HasDefaultValueSql("newid()");

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(250);

				entity.HasOne(d => d.CreatedBy)
					.WithMany(p => p.RoleCreatedBy)
					.HasForeignKey(d => d.CreatedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Role_CreatedByUser");

				entity.HasOne(d => d.LastModifiedBy)
					.WithMany(p => p.RoleLastModifiedBy)
					.HasForeignKey(d => d.LastModifiedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Role_LastModifiedByUser");
			});

			modelBuilder.Entity<Team>(entity =>
			{
				entity.HasKey(i => i.Id)
					.ForSqlServerIsClustered(false);

				entity.HasIndex(e => e.ClubId)
					.HasName("IX_ClubId");

				entity.HasIndex(e => e.CreatedById)
					.HasName("IX_Team_CreatedById");

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_Team_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.HasIndex(e => e.LastModifiedById)
					.HasName("IX_Team_LastModifiedById");

				entity.Property(e => e.Id).ValueGeneratedNever();

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(500);

				entity.HasOne(d => d.Club)
					.WithMany(p => p.Team)
					.HasForeignKey(d => d.ClubId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Team_Club");

				entity.HasOne(d => d.CreatedBy)
					.WithMany(p => p.TeamCreatedBy)
					.HasForeignKey(d => d.CreatedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Team_CreatedByUser");

				entity.HasOne(d => d.LastModifiedBy)
					.WithMany(p => p.TeamLastModifiedBy)
					.HasForeignKey(d => d.LastModifiedById)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Team_LastModifiedByUser");
			});

			modelBuilder.Entity<User>(entity =>
			{
				entity.ToTable(name: "User");
				entity.Property(e => e.Id).HasColumnName("Id");

				entity.HasKey(i => i.Id)
					.ForSqlServerIsClustered(false);

				entity.HasIndex(e => e.IndexId)
					.HasName("UX_User_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.HasIndex(e => e.Name)
					.HasName("UX_User_Column")
					.IsUnique();

				entity.Property(e => e.Id).HasDefaultValueSql("newid()");

				entity.Property(e => e.Active).HasDefaultValueSql("1");

				entity.Property(e => e.DisplayName).HasMaxLength(500);

				entity.Property(e => e.EmailAddress).HasMaxLength(500);

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(200);
			});

			modelBuilder.Entity<UserAction>(entity =>
			{
				entity.HasKey(i => i.Id)
					.ForSqlServerIsClustered(false);

				entity.HasIndex(e => e.ActionId)
					.HasName("IX_ActionId");

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_UserAction_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.HasIndex(e => e.UserId)
					.HasName("IX_UserId");

				entity.Property(e => e.Id).ValueGeneratedNever();

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.HasOne(d => d.Action)
					.WithMany(p => p.UserAction)
					.HasForeignKey(d => d.ActionId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_UserAction_Action");

				entity.HasOne(d => d.User)
					.WithMany(p => p.UserAction)
					.HasForeignKey(d => d.UserId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_UserAction_User");
			});

			modelBuilder.Entity<UserChallenge>(entity =>
			{
				entity.HasKey(i => i.Id)
					.ForSqlServerIsClustered(false);

				entity.HasIndex(e => e.ChallengeId)
					.HasName("IX_ChallengeId");

				entity.HasIndex(e => e.ChallengeLevelId)
					.HasName("IX_ChallengeLevelId");

				entity.HasIndex(e => e.IndexId)
					.HasName("IX_UserChallenge_IndexId")
					.ForSqlServerIsClustered(true)
					.IsUnique();

				entity.HasIndex(e => e.UserId)
					.HasName("IX_UserId");

				entity.Property(e => e.Id).ValueGeneratedNever();

				entity.Property(e => e.IndexId).ValueGeneratedOnAddOrUpdate();

				entity.HasOne(d => d.Challenge)
					.WithMany(p => p.UserChallenge)
					.HasForeignKey(d => d.ChallengeId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_UserChallenge_Challenge");

				entity.HasOne(d => d.ChallengeLevel)
					.WithMany(p => p.UserChallenge)
					.HasForeignKey(d => d.ChallengeLevelId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_UserChallengeLevel_ChallengeLevel");

				entity.HasOne(d => d.User)
					.WithMany(p => p.UserChallenge)
					.HasForeignKey(d => d.UserId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_UserChallenge_User");
			});

			modelBuilder.Entity<VersionHistory>(entity =>
			{
				entity.HasKey(e => e.Version)
					.HasName("PK_VersionHistory");

				entity.Property(e => e.Version).HasMaxLength(50);
			});


		}
	}
}