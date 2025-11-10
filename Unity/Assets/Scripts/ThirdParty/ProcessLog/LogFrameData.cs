using System.Collections.Generic;

namespace ProcessLog
{
	public class LogFrameData
	{
		public int FrameIndex;
		public long Hash;
		public List<ushort> Ids = new List<ushort>(1024);	//本帧所有的函数id
		public List<long> Args =new List<long>(1024);		//本帧所有的函数参数
		public List<string> Strings = new List<string>(32);	//本帧所有的lua函数调用

		public void Clear()
		{
			Ids.Clear();
			Args.Clear();
			Strings.Clear();
		}
		
	}
}