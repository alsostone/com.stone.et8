﻿
namespace ET.Client
{
	[ComponentOf]
	public class LSViewPlayerComponent : Entity, IAwake<long>
	{
		public long BindViewId { get; set; }
	}
}