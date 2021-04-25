namespace CheckOver.Service
{
    public interface IUserService
    {
        bool CheckIfUserHasPermission(string permissionString, int groupId, string userId = null);

        string GetUserEmail();

        string GetUserId();

        bool ifCreator(int id);
    }
}