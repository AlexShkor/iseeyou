using System;
using ISeeYou.Domain.Aggregates.User.Commands;
using ISeeYou.Domain.Aggregates.User.Events;
using ISeeYou.Platform.Domain;
using ISeeYou.Platform.Extensions;

namespace ISeeYou.Domain.Aggregates.User
{
    public class UserAggregate : Aggregate<UserState>
    {
        public void Create(
            string id, 
            string userName, 
            string passwordHash, 
            string passwordSalt, 
            string email,
            string facebookId)
        {
            if (State.Id.HasValue())
            {
                if (State.UserWasDeleted)
                {
                    Apply(new UserReCreated
                    {
                        Id = State.Id,
                        UserName = State.UserName,
                        Email = State.Email,
                        PasswordHash = State.PasswordHash,
                        PasswordSalt = State.PasswordSalt,
                        CreationDate = State.CreationDate
                    });
                }
                else
                {
                    throw new InvalidOperationException("User with same ID already exist.");
                }
            }
            else
            {
                Apply(new UserCreated
                {
                    Id = id,
                    UserName = userName,
                    Email = email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    CreationDate = DateTime.Now,
                    FacebookId = facebookId,
                    Cash = 5000
                });
            }
        }

        public void ChangePassword(string passwordHash, string passwordSalt, bool isChangedByAdmin)
        {

            Apply(new PasswordChanged
            {
                Id = State.Id,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                WasChangedByAdmin = isChangedByAdmin,
            });
        }

        public void Delete(DeleteUser c)
        {
            Apply(new UserDeleted
            {
                Id = c.Id,
                DeletedByUserId = c.DeletedByUserId,
            });
        }

        public void UpdateDetails(UpdateUserDetails c)
        {
            Apply(new UserDetailsUpdated
            {
                Id = c.Id,
                UserName = c.UserName
            });
        }

        public void SetProfileAvatar(string avatarId)
        {
            Apply(new ProfileAvatarSet
            {
                Id = State.Id,
                AvatarId = avatarId
            });
        }
    }
}