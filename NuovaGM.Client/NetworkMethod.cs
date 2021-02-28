using System;
using CitizenFX.Core;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client
{
	public abstract class NetworkMethod : IDisposable
	{
		protected Delegate Callback { get; private set; }
		protected Delegate RegisteredCallback { get; }
		public string EventName { get; }

		public NetworkMethod(string eventName, Delegate callback = null)
		{
			EventName = eventName;
			Callback = callback;
			RegisteredCallback = GetRegisterCallback();
			if (callback != null) Client.Instance.AddEventHandler("lprp:serverCallBack:" + eventName, RegisteredCallback);
		}

		~NetworkMethod() { Dispose(); }

		protected virtual Delegate GetRegisterCallback() { return Callback; }

		public void Invoke() { InvokeInternal(); }

		public void InvokeNoArgs() { InvokeInternal(); }

		protected void InvokeInternal(params object[] args) { BaseScript.TriggerServerEvent("lprp:serverCallBack:" + EventName, args); }

		/// <inheritdoc />
		public void Dispose()
		{
			if (Callback != null)
			{
				Client.Instance.DeAddEventHandler("lprp:serverCallBack:" + EventName, RegisteredCallback);
				Callback = null;
			}

			GC.SuppressFinalize(this);
		}

		protected static T DeserializeObject<T>(string text)
		{
			if (text == null) return default;

			return text.Deserialize<T>();
		}

		protected static string SerializeObject(object o)
		{
			if (o == null) return null;

			return o.Serialize();
		}
	}

	public class NetworkMethod<T1> : NetworkMethod
	{
		public NetworkMethod(string eventName, Action<T1> callback = null) : base(eventName, callback) { }
		public void Invoke(T1 value1) { InvokeInternal(TypeCache<T1>.IsSimpleType ? (object)value1 : SerializeObject(value1)); }

		protected override Delegate GetRegisterCallback() { return new Action<object>(SerializedCallback); }

		private void SerializedCallback(object val1) { Callback.DynamicInvoke(TypeCache<T1>.IsSimpleType ? (T1)val1 : DeserializeObject<T1>((string)val1)); }
	}

	public class NetworkMethod<T1, T2> : NetworkMethod
	{
		public NetworkMethod(string eventName, Action<T1, T2> callback = null) : base(eventName, callback) { }
		public void Invoke(T1 value1, T2 value2) { InvokeInternal(TypeCache<T1>.IsSimpleType ? (object)value1 : SerializeObject(value1), TypeCache<T2>.IsSimpleType ? (object)value2 : SerializeObject(value2)); }

		protected override Delegate GetRegisterCallback() { return new Action<object, object>(SerializedCallback); }

		private void SerializedCallback(object val1, object val2) { Callback.DynamicInvoke(TypeCache<T1>.IsSimpleType ? (T1)val1 : DeserializeObject<T1>((string)val1), TypeCache<T2>.IsSimpleType ? (T2)val2 : DeserializeObject<T2>((string)val2)); }
	}

	public class NetworkMethod<T1, T2, T3> : NetworkMethod
	{
		public NetworkMethod(string eventName, Action<T1, T2, T3> callback = null) : base(eventName, callback) { }
		public void Invoke(T1 value1, T2 value2, T3 value3) { InvokeInternal(TypeCache<T1>.IsSimpleType ? (object)value1 : SerializeObject(value1), TypeCache<T2>.IsSimpleType ? (object)value2 : SerializeObject(value2), TypeCache<T3>.IsSimpleType ? (object)value3 : SerializeObject(value3)); }

		protected override Delegate GetRegisterCallback() { return new Action<object, object, object>(SerializedCallback); }

		private void SerializedCallback(object val1, object val2, object val3) { Callback.DynamicInvoke(TypeCache<T1>.IsSimpleType ? (T1)val1 : DeserializeObject<T1>((string)val1), TypeCache<T2>.IsSimpleType ? (T2)val2 : DeserializeObject<T2>((string)val2), TypeCache<T3>.IsSimpleType ? (T3)val3 : DeserializeObject<T3>((string)val3)); }
	}

	public class NetworkMethod<T1, T2, T3, T4> : NetworkMethod
	{
		public NetworkMethod(string eventName, Action<T1, T2, T3, T4> callback = null) : base(eventName, callback) { }
		public void Invoke(T1 value1, T2 value2, T3 value3, T4 value4) { InvokeInternal(TypeCache<T1>.IsSimpleType ? (object)value1 : SerializeObject(value1), TypeCache<T2>.IsSimpleType ? (object)value2 : SerializeObject(value2), TypeCache<T3>.IsSimpleType ? (object)value3 : SerializeObject(value3), TypeCache<T4>.IsSimpleType ? (object)value4 : SerializeObject(value4)); }

		protected override Delegate GetRegisterCallback() { return new Action<object, object, object, object>(SerializedCallback); }

		private void SerializedCallback(object val1, object val2, object val3, object val4) { Callback.DynamicInvoke(TypeCache<T1>.IsSimpleType ? (T1)val1 : DeserializeObject<T1>((string)val1), TypeCache<T2>.IsSimpleType ? (T2)val2 : DeserializeObject<T2>((string)val2), TypeCache<T3>.IsSimpleType ? (T3)val3 : DeserializeObject<T3>((string)val3), TypeCache<T4>.IsSimpleType ? (T4)val4 : DeserializeObject<T4>((string)val4)); }
	}

	public class NetworkMethod<T1, T2, T3, T4, T5> : NetworkMethod
	{
		public NetworkMethod(string eventName, Action<T1, T2, T3, T4, T5> callback = null) : base(eventName, callback) { }
		public void Invoke(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) { InvokeInternal(TypeCache<T1>.IsSimpleType ? (object)value1 : SerializeObject(value1), TypeCache<T2>.IsSimpleType ? (object)value2 : SerializeObject(value2), TypeCache<T3>.IsSimpleType ? (object)value3 : SerializeObject(value3), TypeCache<T4>.IsSimpleType ? (object)value4 : SerializeObject(value4), TypeCache<T5>.IsSimpleType ? (object)value5 : SerializeObject(value5)); }

		protected override Delegate GetRegisterCallback() { return new Action<object, object, object, object, object>(SerializedCallback); }

		private void SerializedCallback(object val1, object val2, object val3, object val4, object val5) { Callback.DynamicInvoke(TypeCache<T1>.IsSimpleType ? (T1)val1 : DeserializeObject<T1>((string)val1), TypeCache<T2>.IsSimpleType ? (T2)val2 : DeserializeObject<T2>((string)val2), TypeCache<T3>.IsSimpleType ? (T3)val3 : DeserializeObject<T3>((string)val3), TypeCache<T4>.IsSimpleType ? (T4)val4 : DeserializeObject<T4>((string)val4), TypeCache<T5>.IsSimpleType ? (T5)val5 : DeserializeObject<T5>((string)val5)); }
	}

	public class NetworkMethod<T1, T2, T3, T4, T5, T6> : NetworkMethod
	{
		public NetworkMethod(string eventName, Action<T1, T2, T3, T4, T5, T6> callback = null) : base(eventName, callback) { }
		public void Invoke(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6) { InvokeInternal(TypeCache<T1>.IsSimpleType ? (object)value1 : SerializeObject(value1), TypeCache<T2>.IsSimpleType ? (object)value2 : SerializeObject(value2), TypeCache<T3>.IsSimpleType ? (object)value3 : SerializeObject(value3), TypeCache<T4>.IsSimpleType ? (object)value4 : SerializeObject(value4), TypeCache<T5>.IsSimpleType ? (object)value5 : SerializeObject(value5), TypeCache<T6>.IsSimpleType ? (object)value6 : SerializeObject(value6)); }

		protected override Delegate GetRegisterCallback() { return new Action<object, object, object, object, object, object>(SerializedCallback); }

		private void SerializedCallback(object val1, object val2, object val3, object val4, object val5, object val6) { Callback.DynamicInvoke(TypeCache<T1>.IsSimpleType ? (T1)val1 : DeserializeObject<T1>((string)val1), TypeCache<T2>.IsSimpleType ? (T2)val2 : DeserializeObject<T2>((string)val2), TypeCache<T3>.IsSimpleType ? (T3)val3 : DeserializeObject<T3>((string)val3), TypeCache<T4>.IsSimpleType ? (T4)val4 : DeserializeObject<T4>((string)val4), TypeCache<T5>.IsSimpleType ? (T5)val5 : DeserializeObject<T5>((string)val5), TypeCache<T6>.IsSimpleType ? (T6)val6 : DeserializeObject<T6>((string)val6)); }
	}

	public class NetworkMethod<T1, T2, T3, T4, T5, T6, T7> : NetworkMethod
	{
		public NetworkMethod(string eventName, Action<T1, T2, T3, T4, T5, T6, T7> callback = null) : base(eventName, callback) { }
		public void Invoke(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7) { InvokeInternal(TypeCache<T1>.IsSimpleType ? (object)value1 : SerializeObject(value1), TypeCache<T2>.IsSimpleType ? (object)value2 : SerializeObject(value2), TypeCache<T3>.IsSimpleType ? (object)value3 : SerializeObject(value3), TypeCache<T4>.IsSimpleType ? (object)value4 : SerializeObject(value4), TypeCache<T5>.IsSimpleType ? (object)value5 : SerializeObject(value5), TypeCache<T6>.IsSimpleType ? (object)value6 : SerializeObject(value6), TypeCache<T7>.IsSimpleType ? (object)value7 : SerializeObject(value7)); }

		protected override Delegate GetRegisterCallback() { return new Action<object, object, object, object, object, object, object>(SerializedCallback); }

		private void SerializedCallback(object val1, object val2, object val3, object val4, object val5, object val6, object val7) { Callback.DynamicInvoke(TypeCache<T1>.IsSimpleType ? (T1)val1 : DeserializeObject<T1>((string)val1), TypeCache<T2>.IsSimpleType ? (T2)val2 : DeserializeObject<T2>((string)val2), TypeCache<T3>.IsSimpleType ? (T3)val3 : DeserializeObject<T3>((string)val3), TypeCache<T4>.IsSimpleType ? (T4)val4 : DeserializeObject<T4>((string)val4), TypeCache<T5>.IsSimpleType ? (T5)val5 : DeserializeObject<T5>((string)val5), TypeCache<T6>.IsSimpleType ? (T6)val6 : DeserializeObject<T6>((string)val6), TypeCache<T7>.IsSimpleType ? (T7)val7 : DeserializeObject<T7>((string)val7)); }
	}

	public class NetworkMethod<T1, T2, T3, T4, T5, T6, T7, T8> : NetworkMethod
	{
		public NetworkMethod(string eventName, Action<T1, T2, T3, T4, T5, T6, T7, T8> callback = null) : base(eventName, callback) { }
		public void Invoke(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8) { InvokeInternal(TypeCache<T1>.IsSimpleType ? (object)value1 : SerializeObject(value1), TypeCache<T2>.IsSimpleType ? (object)value2 : SerializeObject(value2), TypeCache<T3>.IsSimpleType ? (object)value3 : SerializeObject(value3), TypeCache<T4>.IsSimpleType ? (object)value4 : SerializeObject(value4), TypeCache<T5>.IsSimpleType ? (object)value5 : SerializeObject(value5), TypeCache<T6>.IsSimpleType ? (object)value6 : SerializeObject(value6), TypeCache<T7>.IsSimpleType ? (object)value7 : SerializeObject(value7), TypeCache<T8>.IsSimpleType ? (object)value8 : SerializeObject(value8)); }

		protected override Delegate GetRegisterCallback() { return new Action<object, object, object, object, object, object, object, object>(SerializedCallback); }

		private void SerializedCallback(object val1, object val2, object val3, object val4, object val5, object val6, object val7, object val8) { Callback.DynamicInvoke(TypeCache<T1>.IsSimpleType ? (T1)val1 : DeserializeObject<T1>((string)val1), TypeCache<T2>.IsSimpleType ? (T2)val2 : DeserializeObject<T2>((string)val2), TypeCache<T3>.IsSimpleType ? (T3)val3 : DeserializeObject<T3>((string)val3), TypeCache<T4>.IsSimpleType ? (T4)val4 : DeserializeObject<T4>((string)val4), TypeCache<T5>.IsSimpleType ? (T5)val5 : DeserializeObject<T5>((string)val5), TypeCache<T6>.IsSimpleType ? (T6)val6 : DeserializeObject<T6>((string)val6), TypeCache<T7>.IsSimpleType ? (T7)val7 : DeserializeObject<T7>((string)val7), TypeCache<T8>.IsSimpleType ? (T8)val8 : DeserializeObject<T8>((string)val8)); }
	}

	public class NetworkMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9> : NetworkMethod
	{
		public NetworkMethod(string eventName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback = null) : base(eventName, callback) { }
		public void Invoke(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9) { InvokeInternal(TypeCache<T1>.IsSimpleType ? (object)value1 : SerializeObject(value1), TypeCache<T2>.IsSimpleType ? (object)value2 : SerializeObject(value2), TypeCache<T3>.IsSimpleType ? (object)value3 : SerializeObject(value3), TypeCache<T4>.IsSimpleType ? (object)value4 : SerializeObject(value4), TypeCache<T5>.IsSimpleType ? (object)value5 : SerializeObject(value5), TypeCache<T6>.IsSimpleType ? (object)value6 : SerializeObject(value6), TypeCache<T7>.IsSimpleType ? (object)value7 : SerializeObject(value7), TypeCache<T8>.IsSimpleType ? (object)value8 : SerializeObject(value8), TypeCache<T9>.IsSimpleType ? (object)value9 : SerializeObject(value9)); }

		protected override Delegate GetRegisterCallback() { return new Action<object, object, object, object, object, object, object, object, object>(SerializedCallback); }

		private void SerializedCallback(object val1, object val2, object val3, object val4, object val5, object val6, object val7, object val8, object val9) { Callback.DynamicInvoke(TypeCache<T1>.IsSimpleType ? (T1)val1 : DeserializeObject<T1>((string)val1), TypeCache<T2>.IsSimpleType ? (T2)val2 : DeserializeObject<T2>((string)val2), TypeCache<T3>.IsSimpleType ? (T3)val3 : DeserializeObject<T3>((string)val3), TypeCache<T4>.IsSimpleType ? (T4)val4 : DeserializeObject<T4>((string)val4), TypeCache<T5>.IsSimpleType ? (T5)val5 : DeserializeObject<T5>((string)val5), TypeCache<T6>.IsSimpleType ? (T6)val6 : DeserializeObject<T6>((string)val6), TypeCache<T7>.IsSimpleType ? (T7)val7 : DeserializeObject<T7>((string)val7), TypeCache<T8>.IsSimpleType ? (T8)val8 : DeserializeObject<T8>((string)val8), TypeCache<T9>.IsSimpleType ? (T9)val9 : DeserializeObject<T9>((string)val9)); }
	}

	public class NetworkMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : NetworkMethod
	{
		public NetworkMethod(string eventName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback = null) : base(eventName, callback) { }
		public void Invoke(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10) { InvokeInternal(TypeCache<T1>.IsSimpleType ? (object)value1 : SerializeObject(value1), TypeCache<T2>.IsSimpleType ? (object)value2 : SerializeObject(value2), TypeCache<T3>.IsSimpleType ? (object)value3 : SerializeObject(value3), TypeCache<T4>.IsSimpleType ? (object)value4 : SerializeObject(value4), TypeCache<T5>.IsSimpleType ? (object)value5 : SerializeObject(value5), TypeCache<T6>.IsSimpleType ? (object)value6 : SerializeObject(value6), TypeCache<T7>.IsSimpleType ? (object)value7 : SerializeObject(value7), TypeCache<T8>.IsSimpleType ? (object)value8 : SerializeObject(value8), TypeCache<T9>.IsSimpleType ? (object)value9 : SerializeObject(value9), TypeCache<T10>.IsSimpleType ? (object)value10 : SerializeObject(value10)); }

		protected override Delegate GetRegisterCallback() { return new Action<object, object, object, object, object, object, object, object, object, object>(SerializedCallback); }

		private void SerializedCallback(object val1, object val2, object val3, object val4, object val5, object val6, object val7, object val8, object val9, object val10) { Callback.DynamicInvoke(TypeCache<T1>.IsSimpleType ? (T1)val1 : DeserializeObject<T1>((string)val1), TypeCache<T2>.IsSimpleType ? (T2)val2 : DeserializeObject<T2>((string)val2), TypeCache<T3>.IsSimpleType ? (T3)val3 : DeserializeObject<T3>((string)val3), TypeCache<T4>.IsSimpleType ? (T4)val4 : DeserializeObject<T4>((string)val4), TypeCache<T5>.IsSimpleType ? (T5)val5 : DeserializeObject<T5>((string)val5), TypeCache<T6>.IsSimpleType ? (T6)val6 : DeserializeObject<T6>((string)val6), TypeCache<T7>.IsSimpleType ? (T7)val7 : DeserializeObject<T7>((string)val7), TypeCache<T8>.IsSimpleType ? (T8)val8 : DeserializeObject<T8>((string)val8), TypeCache<T9>.IsSimpleType ? (T9)val9 : DeserializeObject<T9>((string)val9), TypeCache<T10>.IsSimpleType ? (T10)val10 : DeserializeObject<T10>((string)val10)); }
	}

	public class NetworkMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : NetworkMethod
	{
		public NetworkMethod(string eventName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> callback = null) : base(eventName, callback) { }
		public void Invoke(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11) { InvokeInternal(TypeCache<T1>.IsSimpleType ? (object)value1 : SerializeObject(value1), TypeCache<T2>.IsSimpleType ? (object)value2 : SerializeObject(value2), TypeCache<T3>.IsSimpleType ? (object)value3 : SerializeObject(value3), TypeCache<T4>.IsSimpleType ? (object)value4 : SerializeObject(value4), TypeCache<T5>.IsSimpleType ? (object)value5 : SerializeObject(value5), TypeCache<T6>.IsSimpleType ? (object)value6 : SerializeObject(value6), TypeCache<T7>.IsSimpleType ? (object)value7 : SerializeObject(value7), TypeCache<T8>.IsSimpleType ? (object)value8 : SerializeObject(value8), TypeCache<T9>.IsSimpleType ? (object)value9 : SerializeObject(value9), TypeCache<T10>.IsSimpleType ? (object)value10 : SerializeObject(value10), TypeCache<T11>.IsSimpleType ? (object)value11 : SerializeObject(value11)); }

		protected override Delegate GetRegisterCallback() { return new Action<object, object, object, object, object, object, object, object, object, object, object>(SerializedCallback); }

		private void SerializedCallback(object val1, object val2, object val3, object val4, object val5, object val6, object val7, object val8, object val9, object val10, object val11) { Callback.DynamicInvoke(TypeCache<T1>.IsSimpleType ? (T1)val1 : DeserializeObject<T1>((string)val1), TypeCache<T2>.IsSimpleType ? (T2)val2 : DeserializeObject<T2>((string)val2), TypeCache<T3>.IsSimpleType ? (T3)val3 : DeserializeObject<T3>((string)val3), TypeCache<T4>.IsSimpleType ? (T4)val4 : DeserializeObject<T4>((string)val4), TypeCache<T5>.IsSimpleType ? (T5)val5 : DeserializeObject<T5>((string)val5), TypeCache<T6>.IsSimpleType ? (T6)val6 : DeserializeObject<T6>((string)val6), TypeCache<T7>.IsSimpleType ? (T7)val7 : DeserializeObject<T7>((string)val7), TypeCache<T8>.IsSimpleType ? (T8)val8 : DeserializeObject<T8>((string)val8), TypeCache<T9>.IsSimpleType ? (T9)val9 : DeserializeObject<T9>((string)val9), TypeCache<T10>.IsSimpleType ? (T10)val10 : DeserializeObject<T10>((string)val10), TypeCache<T11>.IsSimpleType ? (T11)val11 : DeserializeObject<T11>((string)val11)); }
	}

	public class NetworkMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : NetworkMethod
	{
		public NetworkMethod(string eventName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> callback = null) : base(eventName, callback) { }
		public void Invoke(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12) { InvokeInternal(TypeCache<T1>.IsSimpleType ? (object)value1 : SerializeObject(value1), TypeCache<T2>.IsSimpleType ? (object)value2 : SerializeObject(value2), TypeCache<T3>.IsSimpleType ? (object)value3 : SerializeObject(value3), TypeCache<T4>.IsSimpleType ? (object)value4 : SerializeObject(value4), TypeCache<T5>.IsSimpleType ? (object)value5 : SerializeObject(value5), TypeCache<T6>.IsSimpleType ? (object)value6 : SerializeObject(value6), TypeCache<T7>.IsSimpleType ? (object)value7 : SerializeObject(value7), TypeCache<T8>.IsSimpleType ? (object)value8 : SerializeObject(value8), TypeCache<T9>.IsSimpleType ? (object)value9 : SerializeObject(value9), TypeCache<T10>.IsSimpleType ? (object)value10 : SerializeObject(value10), TypeCache<T11>.IsSimpleType ? (object)value11 : SerializeObject(value11), TypeCache<T12>.IsSimpleType ? (object)value12 : SerializeObject(value12)); }

		protected override Delegate GetRegisterCallback() { return new Action<object, object, object, object, object, object, object, object, object, object, object, object>(SerializedCallback); }

		private void SerializedCallback(object val1, object val2, object val3, object val4, object val5, object val6, object val7, object val8, object val9, object val10, object val11, object val12) { Callback.DynamicInvoke(TypeCache<T1>.IsSimpleType ? (T1)val1 : DeserializeObject<T1>((string)val1), TypeCache<T2>.IsSimpleType ? (T2)val2 : DeserializeObject<T2>((string)val2), TypeCache<T3>.IsSimpleType ? (T3)val3 : DeserializeObject<T3>((string)val3), TypeCache<T4>.IsSimpleType ? (T4)val4 : DeserializeObject<T4>((string)val4), TypeCache<T5>.IsSimpleType ? (T5)val5 : DeserializeObject<T5>((string)val5), TypeCache<T6>.IsSimpleType ? (T6)val6 : DeserializeObject<T6>((string)val6), TypeCache<T7>.IsSimpleType ? (T7)val7 : DeserializeObject<T7>((string)val7), TypeCache<T8>.IsSimpleType ? (T8)val8 : DeserializeObject<T8>((string)val8), TypeCache<T9>.IsSimpleType ? (T9)val9 : DeserializeObject<T9>((string)val9), TypeCache<T10>.IsSimpleType ? (T10)val10 : DeserializeObject<T10>((string)val10), TypeCache<T11>.IsSimpleType ? (T11)val11 : DeserializeObject<T11>((string)val11), TypeCache<T12>.IsSimpleType ? (T12)val12 : DeserializeObject<T12>((string)val12)); }
	}

	public class NetworkMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : NetworkMethod
	{
		public NetworkMethod(string eventName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> callback = null) : base(eventName, callback) { }
		public void Invoke(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13) { InvokeInternal(TypeCache<T1>.IsSimpleType ? (object)value1 : SerializeObject(value1), TypeCache<T2>.IsSimpleType ? (object)value2 : SerializeObject(value2), TypeCache<T3>.IsSimpleType ? (object)value3 : SerializeObject(value3), TypeCache<T4>.IsSimpleType ? (object)value4 : SerializeObject(value4), TypeCache<T5>.IsSimpleType ? (object)value5 : SerializeObject(value5), TypeCache<T6>.IsSimpleType ? (object)value6 : SerializeObject(value6), TypeCache<T7>.IsSimpleType ? (object)value7 : SerializeObject(value7), TypeCache<T8>.IsSimpleType ? (object)value8 : SerializeObject(value8), TypeCache<T9>.IsSimpleType ? (object)value9 : SerializeObject(value9), TypeCache<T10>.IsSimpleType ? (object)value10 : SerializeObject(value10), TypeCache<T11>.IsSimpleType ? (object)value11 : SerializeObject(value11), TypeCache<T12>.IsSimpleType ? (object)value12 : SerializeObject(value12), TypeCache<T13>.IsSimpleType ? (object)value13 : SerializeObject(value13)); }

		protected override Delegate GetRegisterCallback() { return new Action<object, object, object, object, object, object, object, object, object, object, object, object, object>(SerializedCallback); }

		private void SerializedCallback(object val1, object val2, object val3, object val4, object val5, object val6, object val7, object val8, object val9, object val10, object val11, object val12, object val13) { Callback.DynamicInvoke(TypeCache<T1>.IsSimpleType ? (T1)val1 : DeserializeObject<T1>((string)val1), TypeCache<T2>.IsSimpleType ? (T2)val2 : DeserializeObject<T2>((string)val2), TypeCache<T3>.IsSimpleType ? (T3)val3 : DeserializeObject<T3>((string)val3), TypeCache<T4>.IsSimpleType ? (T4)val4 : DeserializeObject<T4>((string)val4), TypeCache<T5>.IsSimpleType ? (T5)val5 : DeserializeObject<T5>((string)val5), TypeCache<T6>.IsSimpleType ? (T6)val6 : DeserializeObject<T6>((string)val6), TypeCache<T7>.IsSimpleType ? (T7)val7 : DeserializeObject<T7>((string)val7), TypeCache<T8>.IsSimpleType ? (T8)val8 : DeserializeObject<T8>((string)val8), TypeCache<T9>.IsSimpleType ? (T9)val9 : DeserializeObject<T9>((string)val9), TypeCache<T10>.IsSimpleType ? (T10)val10 : DeserializeObject<T10>((string)val10), TypeCache<T11>.IsSimpleType ? (T11)val11 : DeserializeObject<T11>((string)val11), TypeCache<T12>.IsSimpleType ? (T12)val12 : DeserializeObject<T12>((string)val12), TypeCache<T13>.IsSimpleType ? (T13)val13 : DeserializeObject<T13>((string)val13)); }
	}

	public class NetworkMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : NetworkMethod
	{
		public NetworkMethod(string eventName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> callback = null) : base(eventName, callback) { }
		public void Invoke(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14) { InvokeInternal(TypeCache<T1>.IsSimpleType ? (object)value1 : SerializeObject(value1), TypeCache<T2>.IsSimpleType ? (object)value2 : SerializeObject(value2), TypeCache<T3>.IsSimpleType ? (object)value3 : SerializeObject(value3), TypeCache<T4>.IsSimpleType ? (object)value4 : SerializeObject(value4), TypeCache<T5>.IsSimpleType ? (object)value5 : SerializeObject(value5), TypeCache<T6>.IsSimpleType ? (object)value6 : SerializeObject(value6), TypeCache<T7>.IsSimpleType ? (object)value7 : SerializeObject(value7), TypeCache<T8>.IsSimpleType ? (object)value8 : SerializeObject(value8), TypeCache<T9>.IsSimpleType ? (object)value9 : SerializeObject(value9), TypeCache<T10>.IsSimpleType ? (object)value10 : SerializeObject(value10), TypeCache<T11>.IsSimpleType ? (object)value11 : SerializeObject(value11), TypeCache<T12>.IsSimpleType ? (object)value12 : SerializeObject(value12), TypeCache<T13>.IsSimpleType ? (object)value13 : SerializeObject(value13), TypeCache<T14>.IsSimpleType ? (object)value14 : SerializeObject(value14)); }

		protected override Delegate GetRegisterCallback() { return new Action<object, object, object, object, object, object, object, object, object, object, object, object, object, object>(SerializedCallback); }

		private void SerializedCallback(object val1, object val2, object val3, object val4, object val5, object val6, object val7, object val8, object val9, object val10, object val11, object val12, object val13, object val14) { Callback.DynamicInvoke(TypeCache<T1>.IsSimpleType ? (T1)val1 : DeserializeObject<T1>((string)val1), TypeCache<T2>.IsSimpleType ? (T2)val2 : DeserializeObject<T2>((string)val2), TypeCache<T3>.IsSimpleType ? (T3)val3 : DeserializeObject<T3>((string)val3), TypeCache<T4>.IsSimpleType ? (T4)val4 : DeserializeObject<T4>((string)val4), TypeCache<T5>.IsSimpleType ? (T5)val5 : DeserializeObject<T5>((string)val5), TypeCache<T6>.IsSimpleType ? (T6)val6 : DeserializeObject<T6>((string)val6), TypeCache<T7>.IsSimpleType ? (T7)val7 : DeserializeObject<T7>((string)val7), TypeCache<T8>.IsSimpleType ? (T8)val8 : DeserializeObject<T8>((string)val8), TypeCache<T9>.IsSimpleType ? (T9)val9 : DeserializeObject<T9>((string)val9), TypeCache<T10>.IsSimpleType ? (T10)val10 : DeserializeObject<T10>((string)val10), TypeCache<T11>.IsSimpleType ? (T11)val11 : DeserializeObject<T11>((string)val11), TypeCache<T12>.IsSimpleType ? (T12)val12 : DeserializeObject<T12>((string)val12), TypeCache<T13>.IsSimpleType ? (T13)val13 : DeserializeObject<T13>((string)val13), TypeCache<T14>.IsSimpleType ? (T14)val14 : DeserializeObject<T14>((string)val14)); }
	}

	public class NetworkMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : NetworkMethod
	{
		public NetworkMethod(string eventName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> callback = null) : base(eventName, callback) { }
		public void Invoke(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14, T15 value15) { InvokeInternal(TypeCache<T1>.IsSimpleType ? (object)value1 : SerializeObject(value1), TypeCache<T2>.IsSimpleType ? (object)value2 : SerializeObject(value2), TypeCache<T3>.IsSimpleType ? (object)value3 : SerializeObject(value3), TypeCache<T4>.IsSimpleType ? (object)value4 : SerializeObject(value4), TypeCache<T5>.IsSimpleType ? (object)value5 : SerializeObject(value5), TypeCache<T6>.IsSimpleType ? (object)value6 : SerializeObject(value6), TypeCache<T7>.IsSimpleType ? (object)value7 : SerializeObject(value7), TypeCache<T8>.IsSimpleType ? (object)value8 : SerializeObject(value8), TypeCache<T9>.IsSimpleType ? (object)value9 : SerializeObject(value9), TypeCache<T10>.IsSimpleType ? (object)value10 : SerializeObject(value10), TypeCache<T11>.IsSimpleType ? (object)value11 : SerializeObject(value11), TypeCache<T12>.IsSimpleType ? (object)value12 : SerializeObject(value12), TypeCache<T13>.IsSimpleType ? (object)value13 : SerializeObject(value13), TypeCache<T14>.IsSimpleType ? (object)value14 : SerializeObject(value14), TypeCache<T15>.IsSimpleType ? (object)value15 : SerializeObject(value15)); }

		protected override Delegate GetRegisterCallback() { return new Action<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(SerializedCallback); }

		private void SerializedCallback(object val1, object val2, object val3, object val4, object val5, object val6, object val7, object val8, object val9, object val10, object val11, object val12, object val13, object val14, object val15) { Callback.DynamicInvoke(TypeCache<T1>.IsSimpleType ? (T1)val1 : DeserializeObject<T1>((string)val1), TypeCache<T2>.IsSimpleType ? (T2)val2 : DeserializeObject<T2>((string)val2), TypeCache<T3>.IsSimpleType ? (T3)val3 : DeserializeObject<T3>((string)val3), TypeCache<T4>.IsSimpleType ? (T4)val4 : DeserializeObject<T4>((string)val4), TypeCache<T5>.IsSimpleType ? (T5)val5 : DeserializeObject<T5>((string)val5), TypeCache<T6>.IsSimpleType ? (T6)val6 : DeserializeObject<T6>((string)val6), TypeCache<T7>.IsSimpleType ? (T7)val7 : DeserializeObject<T7>((string)val7), TypeCache<T8>.IsSimpleType ? (T8)val8 : DeserializeObject<T8>((string)val8), TypeCache<T9>.IsSimpleType ? (T9)val9 : DeserializeObject<T9>((string)val9), TypeCache<T10>.IsSimpleType ? (T10)val10 : DeserializeObject<T10>((string)val10), TypeCache<T11>.IsSimpleType ? (T11)val11 : DeserializeObject<T11>((string)val11), TypeCache<T12>.IsSimpleType ? (T12)val12 : DeserializeObject<T12>((string)val12), TypeCache<T13>.IsSimpleType ? (T13)val13 : DeserializeObject<T13>((string)val13), TypeCache<T14>.IsSimpleType ? (T14)val14 : DeserializeObject<T14>((string)val14), TypeCache<T15>.IsSimpleType ? (T15)val15 : DeserializeObject<T15>((string)val15)); }
	}
}