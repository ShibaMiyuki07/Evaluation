namespace Evaluation.Models.Csv
{
	public class Commissions
	{
        public string Type { get; set; }

		private string _commission;
		public string Commission
		{
			get 
			{
				return _commission;	
			} 
			set
			{
				_commission = value.Replace("%","");
			}
		}
    }
}
