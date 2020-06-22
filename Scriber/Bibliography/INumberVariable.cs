namespace Scriber.Bibliography
{
    public interface INumberVariable : IVariable
    {
        uint Min
        {
            get;
        }
        uint Max
        {
            get;
        }
        char Separator
        {
            get;
        }
    }
}
