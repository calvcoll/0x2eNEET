namespace DotNEET.Xml.DataAccess
{
    public interface IDataContext
    {
        IParentDiff ParentDiff { get; }

        void Sync();
    }
}