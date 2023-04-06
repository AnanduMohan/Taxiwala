namespace Taxiwala.Models.Constant
{
    public class BaseEntity
    {

        public Guid Id { get; private set; }
        public BaseEntity()
        {
            Id = Guid.NewGuid();

        }
    }
}
