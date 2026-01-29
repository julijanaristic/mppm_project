namespace FTN.Services.NetworkModelService.DataModel.Core
{
	public class PowerSystemResource : IdentifiedObject
	{	
		public PowerSystemResource(long globalId)
			: base(globalId)
		{
		}

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
