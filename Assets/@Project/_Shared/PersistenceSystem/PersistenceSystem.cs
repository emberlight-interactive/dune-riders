using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

namespace DuneRiders.Shared.PersistenceSystem {
	public interface IPersistent
    {
		public bool DisablePersistence { get; }
        public void Save(IPersistenceUtil persistUtil);
        public void Load(IPersistenceUtil persistUtil);
    }

	public interface IPersistenceUtil {
		public void Save<T>(string key, T data);
		public T Load<T>(string key);
	}

	public interface IPersistenceUtilInternal : IPersistenceUtil {
		public void Delete(string key);
		public bool SaveFileExists();
		public void PrimeObjectForAsyncSave<T>(string key, T data);
		public JobHandle SaveAsync();
		public void ClearObjectsPrimedForAsyncSave();
	}
}
