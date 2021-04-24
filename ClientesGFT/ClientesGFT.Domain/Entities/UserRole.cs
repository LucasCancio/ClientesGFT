namespace ClientesGFT.Domain.Entities
{
    public class UserRole
    {
        private UserRole()
        { }

        public UserRole(int userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public UserRole(User user, Role role)
        {
            User = user;
            UserId = user.Id;
            Role = role;
            RoleId = role.Id;
        }

        public int UserId { get; private set; }
        public int RoleId { get; private set; }

        public virtual Role Role { get; private set; }
        public virtual User User { get; private set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is UserRole))
                return false;

            var userRole = (UserRole)obj;

            return this.UserId == userRole.UserId && this.RoleId == userRole.RoleId;
        }
    }
}
