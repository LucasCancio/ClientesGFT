namespace ClientesGFT.Domain.Entities
{
    public class Phone
    {
        public Phone(string number)
        {
            Number = number;
        }

        public Phone(int id, string number)
        {
            Id = id;
            Number = number;
        }

        public int Id { get; private set; }
        public string Number { get; private set; }

        public int? ClientId { get; set; }
        public virtual Client Client { get; private set; }




        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Phone))
                return false;

            var telefone = (Phone)obj;

            return this.Number == telefone.Number && this.Id == telefone.Id;
        }
    }
}
