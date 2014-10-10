namespace VkAPIAsync.Wrappers.Groups
{
    public class GroupsFilter
    {
        public enum GroupsFilterEnum
        {
            Admin,
            Groups,
            Publics,
            Events
        }

        public GroupsFilter(GroupsFilterEnum filter)
        {
            switch (filter)
            {
                case GroupsFilterEnum.Admin:
                    Value = "admin";
                    break;
                case GroupsFilterEnum.Groups:
                    Value = "groups";
                    break;
                case GroupsFilterEnum.Publics:
                    Value = "publics";
                    break;
                case GroupsFilterEnum.Events:
                    Value = "events";
                    break;
            }
        }

        public string Value { get; private set; }
    }
}