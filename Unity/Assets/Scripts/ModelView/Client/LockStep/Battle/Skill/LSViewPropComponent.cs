using System.Collections.Generic;

namespace ET.Client
{
	[ComponentOf]
	public class LSViewPropComponent : Entity, IAwake<LSUnit>, IDestroy
	{
		public Dictionary<NumericType, float> PropValueMapping = new ();
	}
}