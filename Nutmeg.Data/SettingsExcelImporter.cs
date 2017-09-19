//using NLog;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.IO;
using System.Linq;
using Nutmeg.Data;
using Action = Nutmeg.Data.Action;
using Nutmeg.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;

namespace Nutmeg.Data
{
	public class SettingsExcelImporter
	{
		#region Fields/Properties
		private readonly NutmegContext _context;
		//private readonly Logger _logger = LogManager.GetCurrentClassLogger();
		private readonly List<string> _errors = new List<string>();
		public List<string> Errors => _errors;
		private List<Action> _actions = new List<Action>();
		private List<User> _users = new List<User>();
		#endregion Fields/Properties

		#region Constructors
		public SettingsExcelImporter(NutmegContext context)
		{
			_context = context;
		}
		#endregion Constructors

		#region Methods
		public bool Import(FileInfo existingFile)
		{
			try
			{
				if (existingFile.Exists)
				{
					using (ExcelPackage package = new ExcelPackage(existingFile))
					{
						// remove all previous data beforehand
						_context.User.RemoveRange(_context.User);
						_context.AppDictionary.RemoveRange(_context.AppDictionary);
						_context.GroupRole.RemoveRange(_context.GroupRole);
						_context.Role.RemoveRange(_context.Role);
						_context.Group.RemoveRange(_context.Group);
						_context.ChallengeLevel.RemoveRange(_context.ChallengeLevel);
						_context.ChallengeAction.RemoveRange(_context.ChallengeAction);
						_context.Action.RemoveRange(_context.Action);
						_context.Challenge.RemoveRange(_context.Challenge);
						_context.Player.RemoveRange(_context.Player);
						_context.Team.RemoveRange(_context.Team);
						_context.Club.RemoveRange(_context.Club);
						_context.Coach.RemoveRange(_context.Coach);
						_context.UserAction.RemoveRange(_context.UserAction);
						_context.ClubManager.RemoveRange(_context.ClubManager);
						_context.UserChallenge.RemoveRange(_context.UserChallenge);

						_context.SaveChanges();
						
						//_context.Configuration.AutoDetectChangesEnabled = false;
						ImportUsers(package);
						// Make sure AppDictionary is before everything except Users,
						// so that other imports can use any AppConfiguration functions
						ImportAppDictionaries(package);

						//var roles = ImportRoles(package);
						//ImportGroupRoles(package, roles);
						var actions = ImportActions(package);
						var challenges = ImportChallenges(package);
						var userActions = ImportUserActions(package);
						if (Errors.Count == 0)
						{
							_context.SaveChanges();
						}
						UpdateUserChallenges();
					}
					return Errors.Count == 0;
				}
				return false;
			}
			catch (Exception ex)
			{
				//_logger.Error(ex, "Exception during import.");
				throw;
			}
		}

		private void LogError(Exception ex, string message)
		{
			//_logger.Error(ex, message);
			_errors.Add(message);
		}
		private void LogError(string message)
		{
			//_logger.Error(message);
			_errors.Add(message);
		}
		private void UpdateUserChallenges()
		{
			var challengeLevels = _context.ChallengeLevel.ToList();
			foreach (var userAction in _context.UserAction.Include(i => i.Action).Include(i => i.Action.ChallengeAction))
			{
				Action action = userAction.Action;
				if (action.IsEnabled)
				{
					int countOfThisActionToday = 0;//TODO:  Calculate this 

					if (countOfThisActionToday < action.LimitPerDay)
					{
						DateTime dateTimeOfLastSameAction = DateTime.MinValue;//TODO:  Calculate This
						double lastTimeOfSameAction = (DateTime.Now - dateTimeOfLastSameAction).TotalSeconds;
						if (lastTimeOfSameAction > action.ThrottleSeconds)
						{
							//var userAction = new UserAction
							//{
							//	Id = Guid.NewGuid(),
							//	ActionId = action.Id,
							//	UserId = userPlayer.Id,
							//	CreatedOn = createdOn.Value,

							//};
							//_context.UserActions.Add(userAction);
						}
					}
				}
				//Fetch all the “Enabled” Challenge this Action belongs to


				var challengeActions = _context.ChallengeAction.Where(i => i.ActionId == action.Id).ToList();
				foreach (var challengeAction in challengeActions)
				{
					UserChallenge userChallenge = _context.UserChallenge.Include(i => i.ChallengeLevel).Where(i => i.UserId == userAction.UserId && i.ChallengeId == challengeAction.ChallengeId).FirstOrDefault();
					if (userChallenge == null)
					{
						userChallenge = _context.UserChallenge.Local.Where(i => i.UserId == userAction.UserId && i.ChallengeId == challengeAction.ChallengeId).FirstOrDefault();
					}

					if (userChallenge == null)
					{
						//var firstLevel = challengeLevels.Where(i => i.ChallengeId == challenge.Id).OrderBy(i => i.TriggerPoint).First();
						userChallenge = new UserChallenge
						{
							Id = Guid.NewGuid(),
							ChallengeId = challengeAction.ChallengeId,
							UserId = userAction.UserId,
							//PointsTotal = action.Points,
							//PointsThisWeek = action.Points,
							//ChallengeLevelId = firstLevel.Id

						};
						_context.UserChallenge.Add(userChallenge);
					}
					userChallenge.PointsTotal += action.Points;
					userChallenge.PointsThisWeek += action.Points;

					#region Update Challenge Level
					var levels = challengeLevels.Where(i => i.ChallengeId == challengeAction.ChallengeId).OrderBy(i => i.TriggerPoint).ToList();
					ChallengeLevel userChallengeLevel = null;
					foreach (var level in levels)
					{
						if (userChallenge.PointsTotal >= level.TriggerPoint)
						{
							userChallengeLevel = level;
							break;
						}
					}
					if (userChallenge.ChallengeLevelId != userChallengeLevel.Id)
					{
						//leveled up!
						userChallenge.ChallengeLevelId = userChallengeLevel.Id;
					}
					#endregion Update Challenge Level

				}

			}
			_context.SaveChanges();
		}
		private List<User> ImportUsers(ExcelPackage package)
		{

			Dictionary<string, Team> teams = new Dictionary<string, Team>();
			Dictionary<string, Club> clubs = new Dictionary<string, Club>();

			var systemAdmin = new User
			{
				Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
				Name = "System Admin",
				Active = true,
				DisplayName = "System Admin",
			};
			_context.Users.Add(systemAdmin);

			try
			{
				ExcelWorksheet workSheet = package.Workbook.Worksheets["User"];

				int lastRow = workSheet.Dimension.End.Row;
				for (int row = 2; row <= lastRow; row++)
				{
					int col = 1;
					var first = workSheet.Cells[row, col++].GetValue<string>();
					var last = workSheet.Cells[row, col++].GetValue<string>();
					var displayName = workSheet.Cells[row, col++].GetValue<string>();
					var phoneNumber = workSheet.Cells[row, col++].GetValue<string>();
					var emails = workSheet.Cells[row, col++].GetValue<string>();
					var parentOfPlayers = workSheet.Cells[row, col++].GetValue<string>();

					var clubName = workSheet.Cells[row, col++].GetValue<string>();
					var isClubManager = workSheet.Cells[row, col++].GetValue<bool>();
					var coachOfTeams = workSheet.Cells[row, col++].GetValue<string>();
					var playerOnTeams = workSheet.Cells[row, col++].GetValue<string>();
					var gender = workSheet.Cells[row, col++].GetValue<string>();
					var birthdate = workSheet.Cells[row, col++].GetValue<string>();

					if (string.IsNullOrEmpty(first))
					{
						continue;
					}

					var user = new User
					{
						Id = Guid.NewGuid(),
						Name = displayName,
						Active = true,
						DisplayName = displayName,
					};

					_context.Users.Add(user);
					_users.Add(user);
					Club club = null;
					if (clubName != null && !clubs.TryGetValue(clubName.Trim(), out club))
					{
						club = new Club()
						{
							Id = Guid.NewGuid(),
							Name = clubName,
							CreatedById = Constants.SystemUserId,
							CreatedOn = DateTimeOffset.Now,
							LastModifiedById = Constants.SystemUserId,
							LastModifiedOn = DateTimeOffset.Now
						};
						clubs.Add(clubName, club);
						_context.Club.Add(club);

					}
					if (isClubManager)
					{
						ClubManager clubManager = new ClubManager()
						{
							Id = Guid.NewGuid(),
							UserId = user.Id,
							ClubId = club.Id,
							Name = "ClubManager",
							CreatedById = Constants.SystemUserId,
							CreatedOn = DateTimeOffset.Now,
							LastModifiedById = Constants.SystemUserId,
							LastModifiedOn = DateTimeOffset.Now


						};
						_context.ClubManager.Add(clubManager);
					}

					if (playerOnTeams != null)
					{
						List<string> teamNames = playerOnTeams.Split(',').Select(p => p.Trim()).ToList();
						foreach (var teamName in teamNames)
						{
							Team foundTeam = null;
							if (!teams.TryGetValue(teamName.Trim(), out foundTeam))
							{
								foundTeam = new Team
								{
									Id = Guid.NewGuid(),
									Name = teamName,
									ClubId = club.Id,
									CreatedById = Constants.SystemUserId,
									CreatedOn = DateTimeOffset.Now,
									LastModifiedById = Constants.SystemUserId,
									LastModifiedOn = DateTimeOffset.Now

								};
								teams.Add(teamName, foundTeam);
								_context.Team.Add(foundTeam);
							}
							Player newTeamPlayer = new Player()
							{
								Id = Guid.NewGuid(),
								UserId = user.Id,
								TeamId = foundTeam.Id,
								Name = displayName,
								CreatedById = Constants.SystemUserId,
								CreatedOn = DateTimeOffset.Now,
								LastModifiedById = Constants.SystemUserId,
								LastModifiedOn = DateTimeOffset.Now
							};
							_context.Player.Add(newTeamPlayer);
							//foundTeam.Players.Add(newTeamPlayer);
						}
					}
					if (parentOfPlayers != null)
					{
						var usersByDisplayName = _users.ToDictionary(i => i.DisplayName);
						List<string> children = parentOfPlayers.Split(',').Select(p => p.Trim()).ToList();
						foreach (var child in children)
						{
							User foundUser = null;
							if (usersByDisplayName.TryGetValue(child.Trim(), out foundUser))
							{
								Parent parent = new Parent
								{
									Id = Guid.NewGuid(),
									Name = displayName,
									ChildUserId = foundUser.Id,
									ParentUserId = user.Id,
									CreatedById = Constants.SystemUserId,
									CreatedOn = DateTimeOffset.Now,
									LastModifiedById = Constants.SystemUserId,
									LastModifiedOn = DateTimeOffset.Now

								};
								_context.Parent.Add(parent);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogError(ex, $"Exception in {nameof(ImportUsers)}()");
			}

			return _users;
		}
		private List<Role> ImportRoles(ExcelPackage package)
		{
			List<Role> roles = new List<Role>();

			try
			{
				ExcelWorksheet workSheet = package.Workbook.Worksheets["Role"];

				int lastRow = workSheet.Dimension.End.Row;
				for (int row = 2; row <= lastRow; row++)
				{
					var name = workSheet.Cells[row, 1].GetValue<string>();

					if (string.IsNullOrEmpty(name))
					{
						continue;
					}

					var role = new Role
					{
						Id = Guid.NewGuid(),
						Name = name,
						CreatedById = Constants.SystemUserId,
						CreatedOn = DateTimeOffset.Now,
						LastModifiedById = Constants.SystemUserId,
						LastModifiedOn = DateTimeOffset.Now
					};

					//_context.Roles.Add(role);
					roles.Add(role);
				}
			}
			catch (Exception ex)
			{
				LogError(ex, $"Exception in {nameof(ImportRoles)}()");
			}

			return roles;
		}
		private List<GroupRole> ImportGroupRoles(ExcelPackage package, List<Role> roles)
		{
			List<GroupRole> groupRoles = new List<GroupRole>();

			try
			{
				ExcelWorksheet workSheet = package.Workbook.Worksheets["GroupRole"];

				int lastRow = workSheet.Dimension.End.Row;
				for (int row = 2; row <= lastRow; row++)
				{
					var groupName = workSheet.Cells[row, 1].GetValue<string>();
					var roleNames = workSheet.Cells[row, 2].GetValue<string>();

					if (string.IsNullOrEmpty(groupName))
					{
						continue;
					}

					var group = new Group
					{
						Id = Guid.NewGuid(),
						Name = groupName,
						CreatedById = Constants.SystemUserId,
						CreatedOn = DateTimeOffset.Now,
						LastModifiedById = Constants.SystemUserId,
						LastModifiedOn = DateTimeOffset.Now
					};
					_context.Group.Add(group);

					foreach (var roleLower in roleNames.ToLower().Split(','))
					{
						var foundRole = roles.FirstOrDefault(x => x.Name.ToLower() == roleLower.Trim());
						if (foundRole == null)
						{
							LogError($"GroupRole {group} has undefined role of {roleLower}.");
							continue;
						}

						var groupRole = new GroupRole
						{
							Id = Guid.NewGuid(),
							GroupId = group.Id,
							RoleId = foundRole.Id
						};

						_context.GroupRole.Add(groupRole);
						groupRoles.Add(groupRole);
					}
				}
			}
			catch (Exception ex)
			{
				LogError(ex, $"Exception in {nameof(ImportGroupRoles)}()");
			}

			return groupRoles;
		}

		//HACK:  Totally hacked this in because I can't get GetValue<bool> to ever return true
		private bool StringToBool(string excelText)
		{
			if (string.IsNullOrWhiteSpace(excelText))
			{
				return false;
			}
			var lc = excelText.ToLower();
			return lc == "true" || lc == "1" || lc == "yes";

		}
		private bool ConvertToBoolean(ExcelWorksheet workSheet, int row, int col)
		{
			string textValue = workSheet.Cells[row, col].ToString();//GetValue<string>;
			bool isTrueOrFalse = false;
			switch (textValue.ToLower())
			{
				case "true":
				case "yes":
				case "x":
				case "1":
					isTrueOrFalse = true;
					break;
				default:
					isTrueOrFalse = false;
					break;
			}
			//isTrueOrFalse = !string.IsNullOrEmpty(textValue);

			return isTrueOrFalse;
		}
		private DateTimeOffset? ConvertToDateTimeOffsetorNull(ExcelWorksheet workSheet, int row, int col)
		{
			string textValue = workSheet.Cells[row, col].GetValue<string>();

			if (!string.IsNullOrEmpty(textValue))
			{
				DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTimeOffset.Now.Hour, 0, 0);
				if (DateTime.TryParse(textValue, out dateTime) == true)
				{
					DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime);
					return dateTimeOffset;
				}
				else
				{
					double oaDateNumber;
					if (double.TryParse(textValue, out oaDateNumber))
					{
						dateTime = DateTime.FromOADate(oaDateNumber);
						DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime);
						return dateTimeOffset;
					}
				}

			}
			return null;
		}
		private double ConvertToDouble(ExcelWorksheet workSheet, int row, int col)
		{
			string textValue = workSheet.Cells[row, col].GetValue<string>();
			return !string.IsNullOrEmpty(textValue) ? Convert.ToDouble(textValue) : 0;
		}
		private int ConvertToInt32(ExcelWorksheet workSheet, int row, int col)
		{
			string textValue = workSheet.Cells[row, col].GetValue<string>();
			return !string.IsNullOrEmpty(textValue) ? Convert.ToInt32(textValue) : 0;
		}
		private int? ConvertToInt32Nullable(ExcelWorksheet workSheet, int row, int col)
		{
			string textValue = workSheet.Cells[row, col].GetValue<string>();
			return !string.IsNullOrEmpty(textValue) ? Convert.ToInt32(textValue) : (int?)null;
		}
		private double? ConvertToDoubleNullable(ExcelWorksheet workSheet, int row, int col)
		{
			string textValue = workSheet.Cells[row, col].GetValue<string>();
			return !string.IsNullOrEmpty(textValue) ? Convert.ToDouble(textValue) : (double?)null;
		}
		private List<AppDictionary> ImportAppDictionaries(ExcelPackage package)
		{
			List<AppDictionary> appDictionaries = new List<AppDictionary>();

			try
			{
				ExcelWorksheet workSheet = package.Workbook.Worksheets["AppDictionary"];

				int lastRow = workSheet.Dimension.End.Row;
				for (int row = 2; row <= lastRow; row++)
				{
					var key = workSheet.Cells[row, 1].GetValue<string>();
					var value = workSheet.Cells[row, 2].GetValue<string>();
					var description = workSheet.Cells[row, 3].GetValue<string>();
					var isVisible = !string.IsNullOrEmpty(workSheet.Cells[row, 4].GetValue<string>());

					if (string.IsNullOrEmpty(key))
					{
						continue;
					}

					var appDictionary = new AppDictionary
					{
						Key = key,
						Value = value,
						Description = description,
						IsVisible = isVisible,
						CreatedById = Constants.SystemUserId,
						CreatedOn = DateTimeOffset.Now,
						LastModifiedById = Constants.SystemUserId,
						LastModifiedOn = DateTimeOffset.Now
					};

					_context.AppDictionary.Add(appDictionary);
					appDictionaries.Add(appDictionary);
				}
			}
			catch (Exception ex)
			{
				LogError(ex, $"Exception in {nameof(ImportAppDictionaries)}()");
			}

			return appDictionaries;
		}

		private List<Action> ImportActions(ExcelPackage package)
		{
			try
			{
				ExcelWorksheet workSheet = package.Workbook.Worksheets["Action"];

				int lastRow = workSheet.Dimension.End.Row;
				for (int row = 2; row <= lastRow; row++)
				{
					var code = workSheet.Cells[row, 1].GetValue<string>();
					var name = workSheet.Cells[row, 2].GetValue<string>();
					var points = workSheet.Cells[row, 3].GetValue<int>();
					var limitsPerDay = workSheet.Cells[row, 4].GetValue<int>();
					var throttleSeconds = workSheet.Cells[row, 5].GetValue<int>();
					var isEnabled = workSheet.Cells[row, 6].GetValue<bool>();
					var requiresVerification = workSheet.Cells[row, 7].GetValue<bool>();
					var description = workSheet.Cells[row, 8].GetValue<string>();

					if (string.IsNullOrEmpty(code))
					{
						continue;
					}

					var action = new Action
					{
						Id = Guid.NewGuid(),
						Code = code,
						Name = name,
						Description = description,
						IsEnabled = isEnabled,
						LimitPerDay = limitsPerDay,
						Points = points,
						ThrottleSeconds = throttleSeconds,
						RequiresVerification = requiresVerification,

						CreatedById = Constants.SystemUserId,
						CreatedOn = DateTimeOffset.Now,
						LastModifiedById = Constants.SystemUserId,
						LastModifiedOn = DateTimeOffset.Now
					};

					_context.Action.Add(action);
					_actions.Add(action);
				}
			}
			catch (Exception ex)
			{
				LogError(ex, $"Exception in {nameof(ImportActions)}()");
			}

			return _actions;
		}
		private List<Challenge> ImportChallenges(ExcelPackage package)
		{
			var actionByCode = _actions.ToDictionary(i => i.Code);
			List<Challenge> items = new List<Challenge>();
			try
			{
				ExcelWorksheet workSheet = package.Workbook.Worksheets["Challenge"];

				int lastRow = workSheet.Dimension.End.Row;
				for (int row = 3; row <= lastRow; row++)
				{
					//var id = workSheet.Cells[row, 1].GetValue<string>();
					var name = workSheet.Cells[row, 1].GetValue<string>();
					var actions = workSheet.Cells[row, 2].GetValue<string>();
					if (string.IsNullOrEmpty(name))
					{
						continue;
					}

					var challenge = new Challenge
					{
						Id = Guid.NewGuid(),
						Name = name,
						CreatedById = Constants.SystemUserId,
						CreatedOn = DateTimeOffset.Now,
						LastModifiedById = Constants.SystemUserId,
						LastModifiedOn = DateTimeOffset.Now
					};

					_context.Challenge.Add(challenge);
					List<string> codes = actions.Split(',').Select(p => p.Trim()).ToList();
					foreach (var code in codes)
					{
						Action foundAction = null;
						if (actionByCode.TryGetValue(code.Trim(), out foundAction))
						{
							var challengeAction = new ChallengeAction
							{
								Id = Guid.NewGuid(),
								Action = foundAction,
								Challenge = challenge

							};
							_context.ChallengeAction.Add(challengeAction);
						}
					}
					#region Create Levels
					for (int levelCounter = 0; levelCounter < 3; levelCounter++)
					{
						int levelStartColIndex = 3 + levelCounter * 2;
						var levelName = workSheet.Cells[row, levelStartColIndex].GetValue<string>();
						var triggerPoints = workSheet.Cells[row, levelStartColIndex + 1].GetValue<int>();

						var challengeLevel = new ChallengeLevel()
						{
							Challenge = challenge,
							Id = Guid.NewGuid(),
							Name = levelName,
							TriggerPoint = triggerPoints,
							IconUrl = string.Empty,
							Description = string.Empty,
							LockedIconUrl = string.Empty,
							Hyperlink = string.Empty

						};
						_context.ChallengeLevel.Add(challengeLevel);
					}
					#endregion
					items.Add(challenge);
				}
			}
			catch (Exception ex)
			{
				LogError(ex, $"Exception in {nameof(ImportChallenges)}()");
			}

			return items;
		}
		private List<Action> ImportUserActions(ExcelPackage package)
		{

			try
			{
				ExcelWorksheet workSheet = package.Workbook.Worksheets["UserAction"];

				int lastRow = workSheet.Dimension.End.Row;
				for (int row = 2; row <= lastRow; row++)
				{
					var code = workSheet.Cells[row, 1].GetValue<string>();
					var createdOn = ConvertToDateTimeOffsetorNull(workSheet, row, 2);
					var player = workSheet.Cells[row, 3].GetValue<string>();
					var score = ConvertToDoubleNullable(workSheet, row, 4);

					if (string.IsNullOrEmpty(code) || createdOn == null || score == null)
					{
						continue;
					}
					var actionByCode = _actions.ToDictionary(i => i.Code);
					Action action = null;
					if (actionByCode.TryGetValue(code, out action))
					{
						var usersByDisplayName = _users.ToDictionary(i => i.DisplayName);
						User userPlayer = null;
						if (usersByDisplayName.TryGetValue(player, out userPlayer) && score != null)
						{
							var userAction = new UserAction
							{
								Id = Guid.NewGuid(),
								ActionId = action.Id,
								UserId = userPlayer.Id,
								Score = score.Value,
								CreatedOn = createdOn.Value,

							};
							_context.UserAction.Add(userAction);
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogError(ex, $"Exception in {nameof(ImportUserActions)}()");
			}

			return _actions;
		}
		#endregion Methods

		#region Classes
		private class ExcelStrategyNode
		{
			public Guid NodeId { get; set; }
			public int RootNodeId { get; set; }
			public int? BranchOnTrueNodeId { get; set; }
			public int? BranchOnFalseNodeId { get; set; }
			public int? ContinueOnNotFoundNodeId { get; set; }
		}
		#endregion Classes
	}
}
