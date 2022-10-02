using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		public void Delete(string key);
	}
}
